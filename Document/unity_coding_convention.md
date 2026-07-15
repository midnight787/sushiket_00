# Unity C# コーディング規約（たたき台）

> このドキュメントはチーム内での議論のたたき台です。プロジェクト特性・メンバーのスキルレベルに応じて調整してください。
> バージョン: v0.1 / 最終更新: 2026-07-15

---

## 0. 目的とスタンス

- **目的**: 「読める・触れる・壊れにくい」コードをチーム全員が書けるようにする
- **原則**: ルールのためのルールにしない。**迷ったら「レビュアーが一番早く理解できる書き方」を選ぶ**
- 本規約は blameless（人を責めない）文化を前提とする。規約違反はコードの指摘であり、人格の指摘ではない
- 例外を認める場合は、コード内コメントで **理由** を明記すること（`// NOTE: 〜のため意図的に〜`）

---

## 1. 命名規則

| 種別 | 規則 | 例 |
|---|---|---|
| クラス / 構造体 / enum | PascalCase | `PlayerController`, `EnemyState` |
| インターフェース | `I` + PascalCase | `IDamageable` |
| public メソッド / プロパティ | PascalCase | `TakeDamage()`, `CurrentHp` |
| private / protected フィールド | `_camelCase` | `_currentHp` |
| public フィールド（SerializeField推奨のため原則非公開） | PascalCase | - |
| ローカル変数 / 引数 | camelCase | `damageAmount` |
| 定数 (`const` / `static readonly`) | PascalCase または `SCREAMING_SNAKE_CASE`（チームで統一） | `MaxHp`, `MAX_HP` |
| イベント | `On` + 動詞過去形 / 現在形 | `OnDamaged`, `OnDeath` |
| コルーチン | 動詞 + `Coroutine` 明示 or `_Co` 接尾辞 | `AttackRoutine()` |
| bool | `Is` / `Has` / `Can` / `Should` から始める | `IsGrounded`, `HasKey` |

**禁止事項**:
- ハンガリアン記法（`strName`, `iCount` 等）は使わない
- 略語の乱用禁止（`mgr` → `Manager`、`tmp` は一時変数のみ許容）

---

## 2. フォーマット

- インデント: **スペース4つ**（タブ禁止、`.editorconfig` で強制）
- 波括弧: **Allman形式**（開き括弧を次の行に）
  ```csharp
  if (condition)
  {
      DoSomething();
  }
  ```
- 1行の長さ: 120文字を目安（超える場合は改行して整形）
- `using` は上部にまとめ、未使用は削除（IDE警告ゼロを維持）
- ファイル末尾は改行1つ

---

## 3. クラス設計

### 3.1 責務分離
- **1クラス1責務**を原則とする。「Manager」「Controller」「Utility」の乱造に注意
- MonoBehaviourは可能な限り薄くし、ロジックは非MonoBehaviourクラス（POCO）に逃がす
  - → テスト容易性・再利用性のため
- Singletonパターンは慎重に使用（乱用するとテスト困難・依存関係が不透明になる）。使う場合はチームで合意を取る

### 3.2 アクセス修飾子
- **デフォルトはprivate**。外部公開が必要な場合のみpublic/protected/internalにする
- `[SerializeField] private` を優先し、`public` フィールドの直接公開は避ける
  ```csharp
  [SerializeField] private float _moveSpeed = 5f;
  public float MoveSpeed => _moveSpeed; // 読み取り専用公開はプロパティで
  ```

### 3.3 Unity ライフサイクルメソッド
- 記述順序を統一する（例）:
  1. `Awake`
  2. `OnEnable`
  3. `Start`
  4. `Update` / `FixedUpdate` / `LateUpdate`
  5. `OnDisable` / `OnDestroy`
  6. その他 public メソッド
  7. private メソッド
- `Update()` 内での重い処理・GC Allocを伴う処理は原則禁止（キャッシュ・プーリングを検討）

---

## 4. Unity特有の注意点

### 4.1 パフォーマンス
- `GetComponent` / `Find` 系はキャッシュする（`Update`内で毎フレーム呼ばない）
- 文字列比較でのタグ判定は `CompareTag()` を使う（`==` より高速）
- `foreach` でのアロケーションに注意（対象コレクション型によりboxing/GC発生）
- Coroutineの多用 vs `async/await` (UniTask等) はプロジェクト方針として明記する

### 4.2 null チェック
- Unityオブジェクトの `null` 判定は `Object.==` のオーバーロードに依存する特殊挙動があるため、`??` や `is null` パターンの扱いをチームで明文化する
  ```csharp
  // Unityオブジェクトに対しては ?. や ?? を安易に使わない（意図しない挙動の可能性）
  if (_target == null) { ... } // これが安全な書き方
  ```

### 4.3 シリアライズ
- `[SerializeField]` に付けるコメント/Tooltipを推奨（`[Tooltip("...")]`）
- Inspector上の並び順は `[Header("...")]` でグルーピング

---

## 5. コメント & ドキュメンテーション

- public API（他人が使うメソッド・クラス）には **XML Doc コメント** (`/// <summary>`) を必須とする
- 「何をしているか」ではなく「**なぜそうしているか**」をコメントする
  - NG: `// iを1増やす` `i++;`
  - OK: `// 敵の湧き数上限に達したら以降のスポーンをスキップする`
- `TODO:` / `FIXME:` / `NOTE:` を統一書式で使い、担当者と日付を残す
  ```csharp
  // TODO(tagomori, 2026-07-15): アニメーションブレンド未実装
  ```

---

## 6. Git / レビュー運用（DoubleEdge的観点）

- **1PR = 1目的**を原則とする（差分が肥大化するとレビューの敵対的検証が機能しない）
- コミットメッセージ: `[種別] 概要` 形式を推奨（例: `[fix] プレイヤー着地判定の誤検知を修正`）
- レビュー観点チェックリスト（レビュアー用）:
  - [ ] 命名規則に沿っているか
  - [ ] MonoBehaviourにロジックを詰め込みすぎていないか
  - [ ] Update系メソッドでのGCアロケーション/重い処理がないか
  - [ ] null許容性・Unityオブジェクト特有のnull挙動を踏まえているか
  - [ ] マジックナンバーが定数化されているか
  - [ ] 例外・エッジケース（0除算、配列範囲外など）への配慮があるか
  - [ ] テスト（Play Mode / Edit Mode Test）が必要な変更か検討したか
- レビューコメントは「指摘」であり「攻撃」ではない。blameless文化を前提に、**問題点とその根拠（なぜ問題か）** をセットで書く

---

## 7. アセット/フォルダ構成との連携（任意）

- スクリプトの配置規則（例: `Assets/Scripts/<Feature>/` 単位）をこの規約と合わせて別途「プロジェクト構成規約」として策定することを推奨
- ScriptableObjectを使ったデータ駆動設計の指針も、必要であれば別ドキュメント化

---

## 8. 今後の検討事項（要チーム議論）

- [ ] async/await (UniTask) を正式採用するか、Coroutine中心で行くか
- [ ] DI（VContainer / Zenject等）を導入するか
- [ ] 静的解析ツール（Roslyn Analyzer, .editorconfig強制）の導入
- [ ] ユニットテスト方針（Edit Mode Test / Play Mode Testの適用範囲）
- [ ] ネーミングにおける定数の書式統一（PascalCase vs SCREAMING_SNAKE_CASE）

---

*本規約はたたき台です。実際の運用の中で摩擦が生じた箇所は都度アップデートし、「メンテナンス=後退」ではなく「規約もまた育てるもの」として扱ってください。*
