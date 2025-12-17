@echo off
REM ===================================================
REM Script para iniciar o Sistema Empresas em HTTPS
REM ===================================================

echo.
echo ========================================
echo   Sistema Empresas - Inicializacao HTTPS
echo ========================================
echo.

REM Verifica se está na pasta correta
if not exist "SistemaEmpresas\SistemaEmpresas.csproj" (
    echo [ERRO] Execute este script na raiz do projeto!
    echo Pasta atual: %CD%
    pause
    exit /b 1
)

echo [1/4] Verificando certificados...
if not exist "SistemaEmpresas\certificado\localhost.pem" (
    echo [AVISO] Certificados nao encontrados!
    echo Execute: mkcert -install
    echo          cd SistemaEmpresas\certificado
    echo          mkcert -key-file localhost-key.pem -cert-file localhost.pem localhost 127.0.0.1 ::1
    pause
    exit /b 1
)
echo [OK] Certificados encontrados!

echo.
echo [2/4] Compilando backend...
cd SistemaEmpresas
dotnet build --configuration Release > nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo [ERRO] Falha na compilacao!
    dotnet build
    pause
    exit /b 1
)
echo [OK] Backend compilado com sucesso!

echo.
echo [3/4] Iniciando backend em HTTPS...
echo URL: https://localhost:5001
echo Swagger: https://localhost:5001/swagger
echo.

start "Backend - Sistema Empresas" cmd /k "dotnet run --urls https://0.0.0.0:5001"

REM Aguarda 5 segundos para o backend iniciar
timeout /t 5 /nobreak > nul

echo.
echo [4/4] Iniciando frontend em HTTPS...
echo URL: https://localhost:5173
echo.

cd ..\frontend
start "Frontend - Sistema Empresas" cmd /k "npm run dev"

echo.
echo ========================================
echo   Sistema iniciado com sucesso!
echo ========================================
echo.
echo Backend:  https://localhost:5001
echo Frontend: https://localhost:5173
echo Swagger:  https://localhost:5001/swagger
echo.
echo Pressione qualquer tecla para abrir o navegador...
pause > nul

start https://localhost:5173

echo.
echo Para parar o sistema, feche as janelas do terminal.
echo.
