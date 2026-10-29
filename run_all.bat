@echo off
title AgroSense AI - Services Launcher
echo ========================================================
echo   Starting AgroSense AI (Full-Stack Precision Agriculture)
echo ========================================================
echo.

echo [1/3] Starting Python ML Service (Step 2: Abdur Rahman)...
start "AgroSense ML Service (Port 8000)" cmd /k "cd /d %~dp0ml-service && python -m uvicorn app.main:app --host 0.0.0.0 --port 8000 --reload"

timeout /t 3 /nobreak > nul

echo [2/3] Starting ASP.NET Core Backend (Step 1: Md. Tahmidul Alam Ahad)...
start "AgroSense Web API (Port 5000)" cmd /k "cd /d %~dp0backend\src\AgroSense.API && dotnet run --urls=http://localhost:5000"

timeout /t 4 /nobreak > nul

echo [3/3] Starting React Web Dashboard (Step 3: Mohammad Tareq Aziz)...
start "AgroSense Frontend (Port 5173)" cmd /k "cd /d %~dp0frontend && pnpm run dev"

echo.
echo ========================================================
echo   Services are now running in separate windows:
echo   - Web Dashboard : http://localhost:5173
echo   - Backend Swagger: http://localhost:5000/swagger
echo   - ML Service Docs: http://localhost:8000/docs
echo ========================================================
echo.
pause

