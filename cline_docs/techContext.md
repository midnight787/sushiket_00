# 技術コンテキスト

## 開発環境
- **エンジン:** Unity（Platformer Microgame改造ベース）
- **レンダリング:** URP（Universal Render Pipeline）
- **ビルドターゲット:** WebGL（Unityroom公開）
- **IDE:** Visual Studio Code + Cline（DeepSeek V3 Pro）
- **バージョン管理:** Git + Git LFS

## アーキテクチャ
Platformer Microgame のシミュレーションイベントパターン（`Platformer.Core.Simulation`）を継承。

```
Assets/Scripts/
├── Core/        Simulation, HeapQueue, Fuzzy（イベント基盤・改変禁止）
├── Mechanics/   PlayerController, EnemyController, AnimationController 等
├── Gameplay/    PlayerDeath, PlayerJumped, EnemyDeath 等のイベント
├── Model/       PlatformerModel
├── UI/          MainUIController, MetaGameController
└── View/        AnimatedTile, ParallaxLayer
```

## アニメーション仕様
- **スプライト:** 4コマ（デンちょス制作）
- **再生fps:** 4fps（m_SampleRate=4, フレーム間隔0.25秒）
- **対象:** Player全8モーション、Enemy全4モーション

## Git LFS 管理ファイル
`.png` / `.wav` / `.ttf` / `.pdf` はLFS管理。
**Unityエディタ外から直接編集・git addしない。**

## .gitignore 追加済み
- `*.bak` / `*.bak.meta`（アニメ変換バックアップ）
- `Library/` / `Temp/` / `Logs/` / `UserSettings/`

## 依存パッケージ（主要）
`Packages/manifest.json` 参照。TextMesh Pro、URP、Input System、Cinemachine など。

## ブランチ運用
- `main` ブランチ直コミット運用（現状）
- force push 禁止、コンフリクト時は `git pull --no-rebase`
