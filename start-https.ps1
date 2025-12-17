# ===================================================
# Script PowerShell para iniciar o Sistema Empresas em HTTPS
# ===================================================

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Sistema Empresas - Inicializacao HTTPS" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Verifica se está na pasta correta
if (!(Test-Path "SistemaEmpresas\SistemaEmpresas.csproj")) {
    Write-Host "[ERRO] Execute este script na raiz do projeto!" -ForegroundColor Red
    Write-Host "Pasta atual: $PWD" -ForegroundColor Yellow
    Read-Host "Pressione Enter para sair"
    exit 1
}

Write-Host "[1/4] Verificando certificados..." -ForegroundColor Yellow
if (!(Test-Path "SistemaEmpresas\certificado\localhost.pem")) {
    Write-Host "[AVISO] Certificados nao encontrados!" -ForegroundColor Yellow
    Write-Host "Execute:" -ForegroundColor White
    Write-Host "  mkcert -install" -ForegroundColor Cyan
    Write-Host "  cd SistemaEmpresas\certificado" -ForegroundColor Cyan
    Write-Host "  mkcert -key-file localhost-key.pem -cert-file localhost.pem localhost 127.0.0.1 ::1" -ForegroundColor Cyan
    Read-Host "Pressione Enter para sair"
    exit 1
}
Write-Host "[OK] Certificados encontrados!" -ForegroundColor Green

Write-Host ""
Write-Host "[2/4] Compilando backend..." -ForegroundColor Yellow
Push-Location SistemaEmpresas
$buildResult = dotnet build --configuration Release 2>&1 | Out-Null
if ($LASTEXITCODE -ne 0) {
    Write-Host "[ERRO] Falha na compilacao!" -ForegroundColor Red
    dotnet build
    Pop-Location
    Read-Host "Pressione Enter para sair"
    exit 1
}
Write-Host "[OK] Backend compilado com sucesso!" -ForegroundColor Green
Pop-Location

Write-Host ""
Write-Host "[3/4] Iniciando backend em HTTPS..." -ForegroundColor Yellow
Write-Host "URL: https://localhost:5001" -ForegroundColor Cyan
Write-Host "Swagger: https://localhost:5001/swagger" -ForegroundColor Cyan
Write-Host ""

Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD\SistemaEmpresas'; dotnet run --urls https://0.0.0.0:5001"

# Aguarda 5 segundos para o backend iniciar
Start-Sleep -Seconds 5

Write-Host ""
Write-Host "[4/4] Iniciando frontend em HTTPS..." -ForegroundColor Yellow
Write-Host "URL: https://localhost:5173" -ForegroundColor Cyan
Write-Host ""

Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD\frontend'; npm run dev"

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  Sistema iniciado com sucesso!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Backend:  https://localhost:5001" -ForegroundColor Cyan
Write-Host "Frontend: https://localhost:5173" -ForegroundColor Cyan
Write-Host "Swagger:  https://localhost:5001/swagger" -ForegroundColor Cyan
Write-Host ""
Write-Host "Pressione qualquer tecla para abrir o navegador..." -ForegroundColor Yellow
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

Start-Process "https://localhost:5173"

Write-Host ""
Write-Host "Para parar o sistema, feche as janelas do PowerShell." -ForegroundColor Yellow
Write-Host ""
