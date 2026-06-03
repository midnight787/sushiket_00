using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.UI
{
    public class ComboDisplay : MonoBehaviour
    {
        public Text comboText;

        ComboStateMachine combo;

        void Start()
        {
            var player = FindFirstObjectByType<PlayerController>();
            if (player == null) return;

            combo = player.GetComponent<ComboStateMachine>();
            if (combo == null) return;

            combo.OnStateChanged  += UpdateDisplay;
            combo.OnComboSuccess  += () => ShowMessage("SUCCESS!", Color.yellow);
            combo.OnComboFailed   += () => ShowMessage("MISS...", Color.red);
            UpdateDisplay(ComboStateMachine.State.Idle);
        }

        void OnDestroy()
        {
            if (combo == null) return;
            combo.OnStateChanged -= UpdateDisplay;
        }

        void UpdateDisplay(ComboStateMachine.State state)
        {
            if (comboText == null) return;
            comboText.color = Color.white;
            comboText.text = state switch
            {
                ComboStateMachine.State.Idle     => "[ _ _ _ ]",
                ComboStateMachine.State.Step1    => "[ E _ _ ]",
                ComboStateMachine.State.Step2    => "[ E * _ ]",
                ComboStateMachine.State.Complete => "[ E * * ]",
                _ => ""
            };
        }

        void ShowMessage(string msg, Color color)
        {
            if (comboText == null) return;
            comboText.text = msg;
            comboText.color = color;
        }
    }
}
