# ============================================================================
# Script para Gerar Certificado SSL Auto-Assinado
# ============================================================================
# ATENÇÃO: Este certificado é APENAS para desenvolvimento/testes!
# Para produção, use Let's Encrypt ou certificado pago.
# ============================================================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Gerador de Certificado SSL" -ForegroundColor Cyan
Write-Host "  Sistema Empresas v1.0.0" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Solicitar informações
$dominio = Read-Host "Digite o domínio (ex: sistemaempresas.local ou localhost)"
$senha = Read-Host "Digite uma senha para o certificado" -AsSecureString
$senhaTexto = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($senha))

Write-Host ""
Write-Host "Gerando certificado para: $dominio" -ForegroundColor Yellow
Write-Host ""

# Criar certificado
try {
    $cert = New-SelfSignedCertificate `
        -DnsName $dominio, "www.$dominio", "api.$dominio" `
        -CertStoreLocation "cert:\LocalMachine\My" `
        -NotAfter (Get-Date).AddYears(2) `
        -FriendlyName "Sistema Empresas - $dominio" `
        -KeyUsage DigitalSignature, KeyEncipherment `
        -KeyAlgorithm RSA `
        -KeyLength 2048

    Write-Host "✓ Certificado gerado com sucesso!" -ForegroundColor Green
    Write-Host "  Thumbprint: $($cert.Thumbprint)" -ForegroundColor Gray
    
    # Exportar para arquivo PFX
    $pfxPath = ".\SistemaEmpresas\certificado\$dominio.pfx"
    
    # Criar pasta se não existir
    if (-not (Test-Path ".\SistemaEmpresas\certificado")) {
        New-Item -ItemType Directory -Path ".\SistemaEmpresas\certificado" -Force | Out-Null
    }
    
    $cert | Export-PfxCertificate -FilePath $pfxPath -Password $senha | Out-Null
    
    Write-Host "✓ Certificado exportado para: $pfxPath" -ForegroundColor Green
    
    # Exportar certificado público (CER) também
    $cerPath = ".\SistemaEmpresas\certificado\$dominio.cer"
    $cert | Export-Certificate -FilePath $cerPath | Out-Null
    Write-Host "✓ Certificado público exportado para: $cerPath" -ForegroundColor Green
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "  PRÓXIMOS PASSOS" -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "1. Edite o arquivo appsettings.json:" -ForegroundColor Yellow
    Write-Host '   "Certificate": {' -ForegroundColor Gray
    Write-Host "     `"Path`": `"certificado/$dominio.pfx`"," -ForegroundColor Gray
    Write-Host "     `"Password`": `"SUA_SENHA_AQUI`"" -ForegroundColor Gray
    Write-Host "   }" -ForegroundColor Gray
    Write-Host ""
    Write-Host "2. Para confiar no certificado localmente:" -ForegroundColor Yellow
    Write-Host "   - Clique duas vezes em: $cerPath" -ForegroundColor Gray
    Write-Host "   - Clique em 'Instalar Certificado...'" -ForegroundColor Gray
    Write-Host "   - Escolha 'Máquina Local'" -ForegroundColor Gray
    Write-Host "   - Selecione 'Autoridades de Certificação Raiz Confiáveis'" -ForegroundColor Gray
    Write-Host ""
    Write-Host "⚠️  IMPORTANTE: Este certificado é auto-assinado!" -ForegroundColor Red
    Write-Host "   Apenas para desenvolvimento. Use Let's Encrypt em produção." -ForegroundColor Red
    Write-Host ""
    
} catch {
    Write-Host "✗ Erro ao gerar certificado: $_" -ForegroundColor Red
    Write-Host ""
    Write-Host "Certifique-se de executar como Administrador!" -ForegroundColor Yellow
    exit 1
}

Write-Host "Pressione qualquer tecla para continuar..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
