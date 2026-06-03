using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer.Mechanics
{
    public class AttackController : MonoBehaviour
    {
        public float attackCooldown = 0.4f;

        public event Action OnSlashAttack;
        public event Action OnBakeAttack;
        public event Action OnFryAttack;

        public bool IsAttacking { get; private set; }

        Animator animator;
        float cooldownTimer;

        InputAction m_SlashAction;
        InputAction m_BakeAction;
        InputAction m_FryAction;

        void Awake()
        {
            animator = GetComponent<Animator>();
            m_SlashAction = InputSystem.actions.FindAction("Player/SlashAttack");
            m_BakeAction  = InputSystem.actions.FindAction("Player/BakeAttack");
            m_FryAction   = InputSystem.actions.FindAction("Player/FryAttack");
            m_SlashAction?.Enable();
            m_BakeAction?.Enable();
            m_FryAction?.Enable();
        }

        void Update()
        {
            if (cooldownTimer > 0f)
            {
                cooldownTimer -= Time.deltaTime;
                return;
            }

            var pc = GetComponent<PlayerController>();
            if (pc != null && !pc.controlEnabled) return;

            if (m_SlashAction != null && m_SlashAction.WasPressedThisFrame())
                TriggerAttack("slash", OnSlashAttack);
            else if (m_BakeAction != null && m_BakeAction.WasPressedThisFrame())
                TriggerAttack("bake", OnBakeAttack);
            else if (m_FryAction != null && m_FryAction.WasPressedThisFrame())
                TriggerAttack("fry", OnFryAttack);
        }

        void TriggerAttack(string attackType, Action callback)
        {
            IsAttacking = true;
            cooldownTimer = attackCooldown;
            animator?.SetTrigger("attack");
            animator?.SetInteger("attackType", AttackTypeToInt(attackType));
            callback?.Invoke();
        }

        // アニメーションイベントから呼ぶ（アニメ末尾に配置）
        public void OnAttackEnd()
        {
            IsAttacking = false;
        }

        static int AttackTypeToInt(string t) => t switch
        {
            "slash" => 0,
            "bake"  => 1,
            "fry"   => 2,
            _       => 0
        };
    }
}
