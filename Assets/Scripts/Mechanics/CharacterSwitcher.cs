using UnityEngine;

namespace Platformer.Mechanics
{
    // E/R/T 入力に合わせてアクティブキャラのスプライトに切り替える（仮実装）
    public class CharacterSwitcher : MonoBehaviour
    {
        [Header("各キャラの仮スプライト (Idle)")]
        public Sprite shinariSprite;
        public Sprite tekunoSprite;
        public Sprite denchosSprite;

        public enum ActiveCharacter { None, Shinari, Tekuno, Denchos }
        public ActiveCharacter Current { get; private set; } = ActiveCharacter.None;

        SpriteRenderer spriteRenderer;
        AttackController attackCtrl;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            attackCtrl = GetComponent<AttackController>();
        }

        void OnEnable()
        {
            attackCtrl.OnSlashAttack += OnSlash;
            attackCtrl.OnBakeAttack  += OnBake;
            attackCtrl.OnFryAttack   += OnFry;
        }

        void OnDisable()
        {
            attackCtrl.OnSlashAttack -= OnSlash;
            attackCtrl.OnBakeAttack  -= OnBake;
            attackCtrl.OnFryAttack   -= OnFry;
        }

        void OnSlash() => SwitchTo(ActiveCharacter.Shinari);
        void OnBake()  => SwitchTo(ActiveCharacter.Tekuno);
        void OnFry()   => SwitchTo(ActiveCharacter.Denchos);

        void SwitchTo(ActiveCharacter ch)
        {
            Current = ch;
            Sprite s = ch switch
            {
                ActiveCharacter.Shinari => shinariSprite,
                ActiveCharacter.Tekuno  => tekunoSprite,
                ActiveCharacter.Denchos => denchosSprite,
                _ => null
            };
            if (s != null && spriteRenderer != null)
                spriteRenderer.sprite = s;
        }
    }
}
