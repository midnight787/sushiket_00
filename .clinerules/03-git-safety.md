---
description: Git を最後の安全網として常に経由させる規律。破壊的作業時に適用。
globs: ["**/*"]
---

# Git Safety Net

## 作業単位のコミット規律
- 破壊的なUnity操作を含むセッションの **前** に、作業ツリーがクリーンで
  あること（コミット済み or stash済み）を確認するようユーザーに促す。
  未コミットの変更がある状態で破壊的操作を始めない。
- まとまった作業が終わったら、Conventional Commits 形式のコミットメッセージを
  提案する（例: `feat(scene): add main house layout`, `fix(script): null guard on YanariController`）。

## 戻せる状態を常に保つ
- 「気に入らなければ `git restore .` または `git reset --hard HEAD` で戻せる」
  状態を前提に作業する。戻せない操作（追跡外ファイルの削除等）は特に慎重に扱い、
  実行前に必ず確認する。

## .gitignore（Unity）
- Unity プロジェクトでは `Library/` `Temp/` `Logs/` `Obj/` `Build/` `Builds/`
  `*.csproj` `*.sln` `UserSettings/` 等を除外すること（GitHub公式 Unity .gitignore 準拠）。
- 除外が効いていないと差分が読めなくなり、AIが何を書き換えたかの検知が困難になる。
  差分が綺麗に見える状態を維持することは安全装置の一部である。

## 差分レビュー
- 破壊的操作の後、変更されたファイルの差分が人間にとって読める粒度に
  収まっているか意識する。一度に巨大な差分を生む操作は、Plan段階で
  チャンク分割を提案する。
