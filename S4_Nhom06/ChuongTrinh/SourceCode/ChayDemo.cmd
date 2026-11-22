@echo off
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0KhoiChay.ps1"
if errorlevel 1 pause
