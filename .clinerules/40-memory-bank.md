# Memory Bank（セッション開始時に必ず読む）

## このプロジェクトのコンテキストファイル

新しいセッションを開始したとき、または「何を作っているか分からない」と感じたとき、
**必ず以下のファイルをこの順番で読んでから作業を始める。**

```
cline_docs/projectbrief.md   # ゲーム概要・コアメカニクス・チーム構成
cline_docs/techContext.md    # 技術スタック・アーキテクチャ・注意事項
cline_docs/activeContext.md  # 今どこを作っているか・直近の状態
cline_docs/progress.md       # 完了済み・進行中・未着手タスク一覧
```

## 更新ルール

以下の作業が完了したとき、該当ファイルを更新してからコミットする:

| 完了した作業 | 更新するファイル |
|-------------|----------------|
| タスクが完了した | `progress.md` のチェックボックスを ✅ に |
| 新しいタスクが発生した | `progress.md` の未着手に追加 |
| 今日の作業フォーカスが変わった | `activeContext.md` を書き直す |
| 技術的な決定をした | `techContext.md` に追記 |
| ゲーム仕様が変わった | `projectbrief.md` を更新 |

## 更新のタイミング

- タスク完了時（コミット前）
- 長いセッションの終わりに
- 次の担当者にバトンを渡す前

**更新をサボると次のセッションで的外れな提案をしてしまう。必ず更新する。**

## 更新に使うスラッシュコマンド

| コマンド | 用途 | 参照ファイル |
|----------|------|------------|
| `/task-done` | タスク完了を `progress.md` に記録 | `workflows/task-done.md` |
| `/focus-update` | 作業フォーカスを `activeContext.md` に反映 | `workflows/focus-update.md` |
| `/save` | ローカルにコミット（push なし） | `workflows/save.md` |
| `/commit-push` | コミット＆リモートへ push | `workflows/commit-push.md` |

典型的なセッション終了フロー:
1. `/task-done` → 完了タスクを記録
2. `/focus-update` → 次回のフォーカスを更新
3. `/save` または `/commit-push` → コミット
