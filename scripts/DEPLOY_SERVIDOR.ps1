# ============================================
# SCRIPT DE DEPLOY - SistemaEmpresas
# Execute este script NO SERVIDOR como Administrador
# ============================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "    DEPLOY - Sistema Empresas" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Configurações
$serviceName = "SistemaEmpresas"
$installPath = "C:\SistemaEmpresas"
$backupPath = "C:\SistemaEmpresas_Backup_$(Get-Date -Format 'yyyyMMdd_HHmmss')"

# 1. PARAR O SERVIÇO
Write-Host "[1/5] Parando o servico $serviceName..." -ForegroundColor Yellow
$service = Get-Service -Name $serviceName -ErrorAction SilentlyContinue
if ($service) {
    if ($service.Status -eq 'Running') {
        Stop-Service -Name $serviceName -Force
        Start-Sleep -Seconds 3
        Write-Host "      Servico parado com sucesso!" -ForegroundColor Green
    } else {
        Write-Host "      Servico ja estava parado." -ForegroundColor Gray
    }
} else {
    Write-Host "      Servico nao encontrado (primeira instalacao?)." -ForegroundColor Gray
}

# 2. BACKUP DA VERSÃO ATUAL
Write-Host "[2/5] Fazendo backup da versao atual..." -ForegroundColor Yellow
if (Test-Path $installPath) {
    Copy-Item -Path $installPath -Destination $backupPath -Recurse -Force
    Write-Host "      Backup salvo em: $backupPath" -ForegroundColor Green
} else {
    Write-Host "      Nenhuma versao anterior encontrada." -ForegroundColor Gray
}

# 3. COPIAR NOVOS ARQUIVOS
Write-Host "[3/5] Copiando novos arquivos..." -ForegroundColor Yellow
Write-Host "      ATENCAO: Copie manualmente os arquivos da pasta 'publish' para '$installPath'" -ForegroundColor Magenta
Write-Host "      Pressione ENTER apos copiar os arquivos..." -ForegroundColor Magenta
Read-Host

# 4. APLICAR MIGRATIONS
Write-Host "[4/5] Aplicando migrations do banco de dados..." -ForegroundColor Yellow
Write-Host "      Deseja aplicar as migrations agora? (S/N)" -ForegroundColor Magenta
$resposta = Read-Host
if ($resposta -eq 'S' -or $resposta -eq 's') {
    Set-Location $installPath
    
    # Verificar se dotnet ef está instalado
    $efInstalled = dotnet tool list -g | Select-String "dotnet-ef"
    if (-not $efInstalled) {
        Write-Host "      Instalando dotnet-ef..." -ForegroundColor Yellow
        dotnet tool install --global dotnet-ef
    }
    
    Write-Host "      Executando: dotnet ef database update --context AppDbContext" -ForegroundColor Gray
    dotnet ef database update --context AppDbContext
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "      Migrations aplicadas com sucesso!" -ForegroundColor Green
    } else {
        Write-Host "      ERRO ao aplicar migrations! Verifique o log acima." -ForegroundColor Red
    }
} else {
    Write-Host "      Migrations ignoradas. Lembre-se de aplicar manualmente!" -ForegroundColor Yellow
}

# 5. INICIAR O SERVIÇO
Write-Host "[5/5] Iniciando o servico $serviceName..." -ForegroundColor Yellow
$service = Get-Service -Name $serviceName -ErrorAction SilentlyContinue
if ($service) {
    Start-Service -Name $serviceName
    Start-Sleep -Seconds 3
    $service = Get-Service -Name $serviceName
    if ($service.Status -eq 'Running') {
        Write-Host "      Servico iniciado com sucesso!" -ForegroundColor Green
    } else {
        Write-Host "      ERRO: Servico nao iniciou. Status: $($service.Status)" -ForegroundColor Red
    }
} else {
    Write-Host "      Servico nao encontrado. Instale o servico primeiro." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "    DEPLOY CONCLUIDO!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Backup anterior em: $backupPath" -ForegroundColor Gray
