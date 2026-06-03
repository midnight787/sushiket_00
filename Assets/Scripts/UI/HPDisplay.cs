using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.UI
{
    public class HPDisplay : MonoBehaviour
    {
        public Text hpText;

        HPManager hpManager;

        void Start()
        {
            var player = FindFirstObjectByType<PlayerController>();
            if (player == null) return;

            hpManager = player.GetComponent<HPManager>();
            if (hpManager == null) return;

            hpManager.OnHPChanged += UpdateDisplay;
            UpdateDisplay(hpManager.CurrentHP, hpManager.maxHP);
        }

        void OnDestroy()
        {
            if (hpManager != null)
                hpManager.OnHPChanged -= UpdateDisplay;
        }

        void UpdateDisplay(int current, int max)
        {
            if (hpText != null)
                hpText.text = $"HP: {current} / {max}";
        }
    }
}
