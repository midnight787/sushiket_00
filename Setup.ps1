<#
  Setup.ps1 — Cline + DeepSeek 簡易セットアップ
  通常は はじめに.bat から呼ばれる。直接実行も可。

  ProjectPath を省略すると「このスクリプトが置かれているフォルダ」を
  プロジェクトルートとみなす。
  → フォルダ一式をプロジェクト直下に置いて bat を叩くだけでOK。
#>
[CmdletBinding()]
param(
    [string]$ProjectPath,
    [ValidateSet('deepseek-v4-flash', 'deepseek-v4-pro')]
    [string]$Model = 'deepseek-v4-flash',
    [string]$Remote = 'origin',
    [string]$Branch = 'main'
)

$ErrorActionPreference = 'Stop'

# このスクリプトの置き場所
$here = Split-Path -Parent $MyInvocation.MyCommand.Path

# ProjectPath 未指定なら、置き場所をそのままプロジェクトルートに
if ([string]::IsNullOrWhiteSpace($ProjectPath)) {
    $ProjectPath = $here
}
$ProjectPath = (Resolve-Path $ProjectPath).Path

function Ok    { param($m) Write-Host "  [OK] $m" -ForegroundColor Green }
function Info  { param($m) Write-Host "`n=== $m ===" -ForegroundColor Cyan }
function Warn  { param($m) Write-Host "  [!]  $m" -ForegroundColor Yellow }
function Has   { param($n) [bool](Get-Command $n -ErrorAction SilentlyContinue) }

Write-Host @"

  ============================================
   Cline + DeepSeek かんたんセットアップ
  ============================================
   対象フォルダ: $ProjectPath

"@ -ForegroundColor Cyan

# ---- 前提チェック ----
Info "前提ツールの確認"
$ng = $false
if (Has git)  { Ok "git あり" } else { Warn "git が無い → https://git-scm.com/download/win から入れてね"; $ng = $true }
$hasCode = Has code
if ($hasCode) { Ok "VSCode あり" } else { Warn "VSCode の code コマンドが無い（拡張は手動で入れる）" }
if ($ng) {
    Write-Host "`n  必要なソフトを入れてから、もう一度 はじめに.bat を実行してね。" -ForegroundColor Yellow
    Read-Host "`n  Enterで終了"
    return
}

# ---- Cline 拡張 ----
Info "Cline 拡張のインストール"
$extId = 'saoudrizwan.claude-dev'
if ($hasCode) {
    $list = (& code --list-extensions) 2>$null
    if ($list -contains $extId) { Ok "Cline は導入済み" }
    else { & code --install-extension $extId --force | Out-Null; Ok "Cline を入れた" }
}
else {
    Warn "VSCodeを開いて Ctrl+Shift+X → 'Cline' を検索して入れてね"
}

# ---- DeepSeek APIキー ----
Info "DeepSeek APIキー"
$cur = [Environment]::GetEnvironmentVariable('DEEPSEEK_API_KEY', 'User')
if ($cur) {
    Ok "APIキーは設定済み"
}
else {
    Write-Host "  DeepSeekのAPIキーを貼り付けてEnter（画面には出ません）" -ForegroundColor Yellow
    Write-Host "  まだ無い人: https://platform.deepseek.com/ で発行できる" -ForegroundColor DarkGray
    $sec = Read-Host "  APIキー" -AsSecureString
    $b = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($sec)
    $p = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($b)
    [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($b)
    if ([string]::IsNullOrWhiteSpace($p)) {
        Warn "空でした。あとでCline設定画面から入れてもOK"
    }
    else {
        [Environment]::SetEnvironmentVariable('DEEPSEEK_API_KEY', $p, 'User')
        Ok "APIキーを保存した（VSCode再起動で反映）"
        $p = $null
    }
}

# ---- .gitignore（無ければ） ----
Info "Git の準備"
Push-Location $ProjectPath
try {
    if (-not (Test-Path (Join-Path $ProjectPath '.git'))) {
        git init | Out-Null
        git symbolic-ref HEAD "refs/heads/$Branch" 2>$null
        Ok "git init 完了"
    } else { Ok "gitリポジトリは準備済み" }

    $gi = Join-Path $ProjectPath '.gitignore'
    if (-not (Test-Path $gi)) {
        @'
.DS_Store
Thumbs.db
desktop.ini
.vs/
.idea/
*.key
*.pem
.env
.env.*
secrets.*
*.local
*.tmp
*.bak
*~
'@ | Set-Content -Path $gi -Encoding ascii
        Ok ".gitignore を置いた"
    } else { Ok ".gitignore は既にある" }

    git config --local pull.rebase false  | Out-Null
    git config --local core.autocrlf true | Out-Null
    git config --local advice.detachedHead false | Out-Null
    Ok "安全なgit設定を適用"
}
finally { Pop-Location }

# ---- .clinerules の存在確認（同梱されている前提） ----
Info ".clinerules の確認"
if (Test-Path (Join-Path $ProjectPath '.clinerules')) {
    Ok ".clinerules（ルール＆ワークフロー）あり"
}
else {
    Warn ".clinerules が見つからない。zipを丸ごとこのフォルダに展開できているか確認してね"
}

# ---- 完了 ----
Write-Host @"

  ============================================
   セットアップ完了！
  ============================================

  このあとやること:
   1. VSCode を再起動する
   2. 左の Cline アイコンを開いて接続設定
        Provider : OpenAI Compatible
        Base URL : https://api.deepseek.com
        Model    : $Model
   3. Cline のチャットで  /save  や  /commit-push  と打って使う

  くわしくは README.md を見てね。

"@ -ForegroundColor Green

Read-Host "  Enterで終了"
