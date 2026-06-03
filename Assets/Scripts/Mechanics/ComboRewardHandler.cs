using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.Mechanics
{
    public class ComboRewardHandler : MonoBehaviour
    {
        [Header("スコア")]
        public int scorePerCombo = 100;

        [Header("フラッシュUI (全画面オーバーレイ Image)")]
        public Image flashImage;
        public float flashDuration = 0.35f;

        public int Score { get; private set; }

        public System.Action<int> OnScoreChanged;

        ComboStateMachine combo;
        HPManager hpManager;

        void Awake()
        {
            combo     = GetComponent<ComboStateMachine>();
            hpManager = GetComponent<HPManager>();
        }

        void OnEnable()
        {
            combo.OnComboSuccess += HandleSuccess;
            combo.OnComboFailed  += HandleFail;
        }

        void OnDisable()
        {
            combo.OnComboSuccess -= HandleSuccess;
            combo.OnComboFailed  -= HandleFail;
        }

        void HandleSuccess()
        {
            Score += scorePerCombo;
            OnScoreChanged?.Invoke(Score);
            Flash(new Color(0.2f, 1f, 0.3f, 0.5f));
        }

        void HandleFail()
        {
            hpManager?.TakeDamage(1);
            Flash(new Color(1f, 0.15f, 0.15f, 0.5f));
        }

        void Flash(Color color)
        {
            if (flashImage == null) return;
            StopAllCoroutines();
            StartCoroutine(FlashCoroutine(color));
        }

        IEnumerator FlashCoroutine(Color color)
        {
            flashImage.color = color;
            float t = flashDuration;
            while (t > 0f)
            {
                t -= Time.deltaTime;
                var c = flashImage.color;
                c.a = Mathf.Lerp(0f, color.a, t / flashDuration);
                flashImage.color = c;
                yield return null;
            }
            flashImage.color = Color.clear;
        }
    }
}
