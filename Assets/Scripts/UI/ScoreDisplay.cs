using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        public Text scoreText;

        ComboRewardHandler rewardHandler;

        void Start()
        {
            var player = FindFirstObjectByType<PlayerController>();
            if (player == null) return;

            rewardHandler = player.GetComponent<ComboRewardHandler>();
            if (rewardHandler == null) return;

            rewardHandler.OnScoreChanged += UpdateDisplay;
            UpdateDisplay(0);
        }

        void OnDestroy()
        {
            if (rewardHandler != null)
                rewardHandler.OnScoreChanged -= UpdateDisplay;
        }

        void UpdateDisplay(int score)
        {
            if (scoreText != null)
                scoreText.text = $"SCORE: {score}";
        }
    }
}
