# プロジェクト構成・スコープ（常時適用）

## このプロジェクトについて

Unity 2D アクションゲーム（Platformer Microgame改造）。
チーム4人で開発中。WebGL（Unityroom）向けビルド。

| ハンドル | 役割 | 担当 |
|----------|------|------|
| しなり（Prgr A） | PM兼任 | キャラ制御・コンボシステム |
| ポテト（Prgr B） | プログラマ | 敵AI・ステージ・BOSS |
| デンちょス（Dsgn A） | デザイナ | キャラスプライト（4コマ・4fps） |
| てくてく（Dsgn B） | デザイナ | 背景・UI・エフェクト |

## 触っていいフォルダ

```
Assets/Scripts/          ← メイン作業場所
Assets/Character/        ← アニメ・スプライト（差し替え時）
Assets/Prefabs/          ← Prefab調整
Assets/Scenes/           ← シーン編集
Assets/ScriptableObjects/ ← パラメータ管理（存在すれば）
Document/                ← ドキュメント・進捗管理
```

## 触ってはいけないフォルダ・ファイル（要確認）

```
Assets/Plugins/          ← サードパーティ。変更前にユーザーに確認
Assets/TextMesh Pro/     ← 自動生成。変更禁止
Assets/Tutorials/        ← 不使用。変更禁止
ProjectSettings/         ← 変更前にユーザーに確認
Packages/                ← 変更禁止
```

## スクリプト配置ルール

新規 `.cs` を作る場合（承認済みの場合のみ）は以下に配置する:

```
Assets/Scripts/
├── Core/        # ゲームシステム基盤（Simulation, EventBus等）
├── Mechanics/   # キャラ・物理・ゲームルール
├── Gameplay/    # イベント・ゲームフロー
├── Model/       # データモデル
├── UI/          # UI制御
└── View/        # 表示系
```

`Assets/Editor/` への追加は **明示的な指示がない限り禁止**（→ 20-implementation-approach.md 参照）。

## バイナリアセット（LFS管理）の扱い

`.png` / `.wav` / `.ttf` / `.pdf` はGit LFSで管理されている。
- これらのファイルは **直接編集しない**（スプライト差し替えはUnityエディタ経由）
- `git add` する際に誤って含めないよう注意する

## 命名規則（C#）

| 種別 | 規則 | 例 |
|------|------|----|
| クラス・メソッド | PascalCase | `PlayerController`, `TakeDamage()` |
| privateフィールド | camelCase | `moveSpeed`, `isGrounded` |
| SerializeField | camelCase | `[SerializeField] float moveSpeed` |
| インターフェース | I + PascalCase | `IDamageable` |
| namespace | `ProjectName.Layer` | `Platformer.Mechanics` |
