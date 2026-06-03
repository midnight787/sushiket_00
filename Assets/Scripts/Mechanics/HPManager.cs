using System;
using System.Collections;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    public class HPManager : MonoBehaviour
    {
        public int maxHP = 3;
        public float invincibilityDuration = 1.5f;

        public int CurrentHP { get; private set; }
        public bool IsAlive => CurrentHP > 0;
        public bool IsInvincible { get; private set; }

        public event Action<int, int> OnHPChanged;

        Animator animator;

        void Awake()
        {
            CurrentHP = maxHP;
            animator = GetComponent<Animator>();
        }

        public void TakeDamage(int amount)
        {
            if (IsInvincible || !IsAlive) return;
            CurrentHP = Mathf.Max(0, CurrentHP - amount);
            OnHPChanged?.Invoke(CurrentHP, maxHP);
            if (CurrentHP <= 0)
                Schedule<PlayerDeath>();
            else
            {
                animator?.SetTrigger("hurt");
                StartCoroutine(InvincibilityCoroutine());
            }
        }

        public void RestoreFullHP()
        {
            CurrentHP = maxHP;
            OnHPChanged?.Invoke(CurrentHP, maxHP);
        }

        IEnumerator InvincibilityCoroutine()
        {
            IsInvincible = true;
            yield return new WaitForSeconds(invincibilityDuration);
            IsInvincible = false;
        }
    }
}
