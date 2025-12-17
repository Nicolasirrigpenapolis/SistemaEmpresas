# Sistema Empresas - Deploy v1.0.0

**Data:** 12/12/2025

## 📦 Conteúdo

- **SistemaEmpresas/** - Aplicação completa (Backend .NET + Frontend React)
- **01_AddModuloTransporte.sql** - Migration: Módulo Transporte
- **02_AddMarcaModeloToReboques.sql** - Migration: Marca/Modelo Reboques  
- **03_AddEmailToUsuario.sql** - Migration: Email de Usuário
- **GERAR_CERTIFICADO_SSL.ps1** - Script para gerar certificado SSL
- **GUIA_INSTALACAO_SSL.md** - Guia completo de SSL/HTTPS
- **README.md** - Este arquivo

## 🚀 Instalação Rápida

### 1. Banco de Dados
Execute os scripts SQL na ordem (01, 02, 03) no SQL Server Management Studio.

```sql
-- No SSMS, executar em ordem:
USE SistemaEmpresas;
GO

-- 1. Módulo Transporte
-- Abrir e executar: 01_AddModuloTransporte.sql

-- 2. Marca/Modelo Reboques
-- Abrir e executar: 02_AddMarcaModeloToReboques.sql

-- 3. Email Usuário
-- Abrir e executar: 03_AddEmailToUsuario.sql
```

### 2. Gerar Certificado SSL

**Opção A: Desenvolvimento/Testes (Certificado Auto-Assinado)**

Execute como Administrador:
```powershell
.\GERAR_CERTIFICADO_SSL.ps1
```

Siga as instruções do script.

**Opção B: Produção (Let's Encrypt - GRÁTIS)**

Consulte `GUIA_INSTALACAO_SSL.md` para instruções completas.

**Opção C: Certificado Pago**

Coloque os arquivos `.pfx` na pasta `SistemaEmpresas/certificado/`

### 3. Configurar appsettings.json

Edite `SistemaEmpresas/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=SistemaEmpresas;User Id=sa;Password=SUA_SENHA;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "SecretKey": "GERAR_CHAVE_FORTE_AQUI_MINIMO_32_CARACTERES",
    "Issuer": "SistemaEmpresasAPI",
    "Audience": "SistemaEmpresasApp",
    "ExpiryMinutes": 480
  },
  "Kestrel": {
    "Endpoints": {
      "Https": {
        "Url": "https://*:5001",
        "Certificate": {
          "Path": "certificado/SEU_DOMINIO.pfx",
          "Password": "SENHA_DO_CERTIFICADO"
        }
      }
    }
  }
}
```

**Gerar Chave JWT Segura:**
```powershell
-join ((48..57) + (65..90) + (97..122) | Get-Random -Count 64 | % {[char]$_})
```

### 4. Executar Aplicação

**Teste Rápido:**
```powershell
cd SistemaEmpresas
.\SistemaEmpresas.exe
```

**Instalar como Serviço Windows (Recomendado):**

```powershell
# Baixar NSSM de https://nssm.cc/download
# Extrair para C:\nssm

cd C:\nssm\win64

# Instalar serviço
.\nssm.exe install SistemaEmpresasAPI "C:\Deploy\SistemaEmpresas\SistemaEmpresas.exe"
.\nssm.exe set SistemaEmpresasAPI AppDirectory "C:\Deploy\SistemaEmpresas"
.\nssm.exe set SistemaEmpresasAPI DisplayName "Sistema Empresas API"
.\nssm.exe set SistemaEmpresasAPI Start SERVICE_AUTO_START

# Iniciar
.\nssm.exe start SistemaEmpresasAPI
```

## 🌐 Acessar Sistema

- **Frontend:** `https://localhost:5001` ou `https://seu-dominio.com.br`
- **API/Swagger:** `https://localhost:5001/swagger`

## 👤 Login Padrão

- **Empresa:** IRRIGACAO PENAPOLIS
- **Usuário:** nicolas
- **Senha:** 2510

## 🔒 Segurança em Produção

⚠️ **IMPORTANTE:** Antes de colocar em produção:

1. ✅ Trocar senha do banco de dados
2. ✅ Gerar nova chave JWT forte
3. ✅ Usar certificado SSL válido (Let's Encrypt ou pago)
4. ✅ Configurar firewall
5. ✅ Trocar senha padrão do usuário
6. ✅ Habilitar HTTPS obrigatório
7. ✅ Configurar backup automático do banco

## 🐛 Troubleshooting

### Erro: "The SSL connection could not be established"
- Verifique se o certificado está na pasta correta
- Confirme a senha do certificado no appsettings.json
- Execute o script GERAR_CERTIFICADO_SSL.ps1

### Erro: "Cannot connect to SQL Server"
- Verifique se SQL Server está rodando
- Teste a connection string com SQL Server Management Studio
- Confirme que TCP/IP está habilitado no SQL Server Configuration Manager

### Frontend não carrega
- Verifique se a API está rodando (acesse /swagger)
- Abra console do navegador (F12) para ver erros
- Confirme que a porta 5001 está liberada no firewall

### Serviço não inicia
```powershell
# Ver logs do serviço
.\nssm.exe status SistemaEmpresasAPI

# Remover e reinstalar
.\nssm.exe remove SistemaEmpresasAPI confirm
# Depois reinstalar conforme instruções acima
```

## 📚 Documentação Completa

Para instalação detalhada, configuração avançada e SSL em produção, consulte:

- **GUIA_INSTALACAO_SSL.md** - Guia completo de certificados SSL/HTTPS

## ✅ Checklist de Instalação

- [ ] SQL Server instalado
- [ ] Scripts SQL executados (01, 02, 03)
- [ ] Certificado SSL gerado
- [ ] appsettings.json configurado
- [ ] Connection String testada
- [ ] JWT Secret gerada
- [ ] Aplicação executando
- [ ] Acesso ao Swagger funcionando
- [ ] Login no sistema OK
- [ ] HTTPS funcionando sem avisos

---

**Sistema Empresas v1.0.0**  
Desenvolvido por Irrigação Penápolis  
Dezembro 2025
