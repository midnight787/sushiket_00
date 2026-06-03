using System;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class ComboStateMachine : MonoBehaviour
    {
        public float stepTimeout = 3f;

        public enum State { Idle, Step1, Step2, Complete }
        public State CurrentState { get; private set; } = State.Idle;

        // Step1で押されたアクション（bake or fry）を記憶してStep2で残りを要求
        AttackType step1Action;

        float timer;
        AttackController attackCtrl;

        public event Action<State> OnStateChanged;
        public event Action OnComboSuccess;
        public event Action OnComboFailed;

        enum AttackType { Slash, Bake, Fry }

        void Awake()
        {
            attackCtrl = GetComponent<AttackController>();
        }

        void OnEnable()
        {
            attackCtrl.OnSlashAttack += HandleSlash;
            attackCtrl.OnBakeAttack  += HandleBake;
            attackCtrl.OnFryAttack   += HandleFry;
        }

        void OnDisable()
        {
            attackCtrl.OnSlashAttack -= HandleSlash;
            attackCtrl.OnBakeAttack  -= HandleBake;
            attackCtrl.OnFryAttack   -= HandleFry;
        }

        void Update()
        {
            if (CurrentState == State.Idle || CurrentState == State.Complete) return;
            timer -= Time.deltaTime;
            if (timer <= 0f) Fail();
        }

        void HandleSlash()
        {
            switch (CurrentState)
            {
                case State.Idle:
                    SetState(State.Step1);
                    break;
                // Step1/Step2でSlashは順序ミス
                default:
                    Fail();
                    break;
            }
        }

        void HandleBake()
        {
            switch (CurrentState)
            {
                case State.Idle:
                    Fail(); // Slash first required
                    break;
                case State.Step1:
                    step1Action = AttackType.Bake;
                    SetState(State.Step2);
                    break;
                case State.Step2 when step1Action == AttackType.Fry:
                    SetState(State.Complete);
                    break;
                default:
                    Fail();
                    break;
            }
        }

        void HandleFry()
        {
            switch (CurrentState)
            {
                case State.Idle:
                    Fail(); // Slash first required
                    break;
                case State.Step1:
                    step1Action = AttackType.Fry;
                    SetState(State.Step2);
                    break;
                case State.Step2 when step1Action == AttackType.Bake:
                    SetState(State.Complete);
                    break;
                default:
                    Fail();
                    break;
            }
        }

        void SetState(State next)
        {
            CurrentState = next;
            OnStateChanged?.Invoke(next);

            if (next == State.Complete)
            {
                OnComboSuccess?.Invoke();
                // 少し待ってIdleに戻す
                Invoke(nameof(ResetToIdle), 0.5f);
            }
            else if (next != State.Idle)
            {
                timer = stepTimeout;
            }
        }

        void Fail()
        {
            OnComboFailed?.Invoke();
            ResetToIdle();
        }

        void ResetToIdle()
        {
            CancelInvoke(nameof(ResetToIdle));
            CurrentState = State.Idle;
            OnStateChanged?.Invoke(State.Idle);
        }
    }
}
