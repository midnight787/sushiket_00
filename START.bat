@echo off
chcp 65001 >nul
cd /d "%~dp0"
echo.
echo  Starting Cline + DeepSeek setup...
echo.
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Setup.ps1"
echo.
pause
