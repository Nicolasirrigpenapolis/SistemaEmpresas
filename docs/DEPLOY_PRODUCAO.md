# 🚀 Guia Completo de Deploy em Produção - Windows Server

## Sistema Empresas - Configuração HTTPS com Let's Encrypt (Gratuito)

---

## 📋 Pré-requisitos no Servidor

### Hardware Recomendado:
- **CPU:** 4 cores ou mais
- **RAM:** 8 GB mínimo (16 GB recomendado)
- **Disco:** 100 GB SSD
- **OS:** Windows Server 2016 ou superior

### Software Necessário:
- ✅ Windows Server 2016/2019/2022
- ✅ SQL Server 2014 ou superior
- ✅ .NET 8.0 Runtime
- ✅ IIS (Internet Information Services)
- ✅ Domínio próprio configurado

---

## 📍 FASE 1: Preparação do Servidor

### **Passo 1.1: Configurar DNS do Domínio**

Antes de tudo, configure seu domínio para apontar para o servidor:

1. Acesse o painel do seu provedor de domínio (Registro.br, GoDaddy, etc.)
2. Crie um registro tipo **A** ou **CNAME**:
   ```
   Tipo: A
   Nome: sistema (ou @, ou www)
   Valor: IP_DO_SEU_SERVIDOR
   TTL: 3600
   ```
3. Aguarde propagação (pode levar até 24h, geralmente 1-2h)

**Testar propagação:**
```powershell
nslookup sistema.suaempresa.com.br
# Deve retornar o IP do servidor
```

---

### **Passo 1.2: Instalar .NET 8.0 Runtime**

1. Baixe o instalador:
   - Acesse: https://dotnet.microsoft.com/download/dotnet/8.0
   - Baixe: **ASP.NET Core Runtime 8.0 - Windows Hosting Bundle**

2. Execute o instalador:
   ```powershell
   # Como administrador
   .\dotnet-hosting-8.0.x-win.exe
   ```

3. **Reinicie o servidor** após instalação

4. Verifique instalação:
   ```powershell
   dotnet --list-runtimes
   # Deve listar: Microsoft.AspNetCore.App 8.0.x
   ```

---

### **Passo 1.3: Instalar e Configurar IIS**

1. Abra **Server Manager** → **Manage** → **Add Roles and Features**

2. Selecione:
   - ✅ **Web Server (IIS)**
   - ✅ **Management Tools** → **IIS Management Console**
   - ✅ **ASP.NET 4.8** (ou superior)

3. Após instalação, abra **IIS Manager**:
   ```powershell
   # Ou via PowerShell
   Start-Process inetmgr
   ```

4. Verifique se IIS está funcionando:
   - Abra navegador: `http://localhost`
   - Deve aparecer a página padrão do IIS

---

### **Passo 1.4: Configurar Firewall**

Libere as portas necessárias:

```powershell
# Como Administrador

# Porta 80 (HTTP - necessária para Let's Encrypt)
New-NetFirewallRule -DisplayName "Allow HTTP" -Direction Inbound -LocalPort 80 -Protocol TCP -Action Allow

# Porta 443 (HTTPS)
New-NetFirewallRule -DisplayName "Allow HTTPS" -Direction Inbound -LocalPort 443 -Protocol TCP -Action Allow

# SQL Server (se acesso remoto)
New-NetFirewallRule -DisplayName "Allow SQL Server" -Direction Inbound -LocalPort 1433 -Protocol TCP -Action Allow
```

---

## 📦 FASE 2: Publicar Aplicação

### **Passo 2.1: Compilar na Máquina de Desenvolvimento**

Na sua máquina de desenvolvimento:

```powershell
cd C:\Projetos\SistemaEmpresas2\SistemaEmpresas

# Publicar em modo Release
dotnet publish -c Release -o publish

# Isso criará a pasta "publish" com todos os arquivos necessários
```

---

### **Passo 2.2: Copiar Arquivos para o Servidor**

Opções para transferir:

**Opção A: Pendrive/HD Externo**
1. Copie a pasta `publish` para pendrive
2. Cole no servidor em: `C:\inetpub\wwwroot\sistemaempresas`

**Opção B: Área de Trabalho Remota**
1. Conecte via RDP com "Recursos Locais" → "Mais" → "Unidades"
2. Copie da máquina local para o servidor

**Opção C: FTP/SFTP** (se disponível)
1. Use FileZilla ou WinSCP
2. Envie pasta `publish` para servidor

---

### **Passo 2.3: Criar Estrutura de Pastas no Servidor**

```powershell
# No servidor, como Administrador

# Criar pasta da aplicação
New-Item -ItemType Directory -Path "C:\inetpub\wwwroot\sistemaempresas" -Force

# Criar pasta para logs
New-Item -ItemType Directory -Path "C:\inetpub\wwwroot\sistemaempresas\Logs" -Force

# Criar pasta para certificados dos emitentes
New-Item -ItemType Directory -Path "C:\inetpub\wwwroot\sistemaempresas\certificados" -Force

# Dar permissões ao IIS
icacls "C:\inetpub\wwwroot\sistemaempresas" /grant "IIS_IUSRS:(OI)(CI)F" /T
icacls "C:\inetpub\wwwroot\sistemaempresas" /grant "IUSR:(OI)(CI)F" /T
```

---

### **Passo 2.4: Configurar appsettings.Production.json**

No servidor, edite o arquivo:

```powershell
notepad C:\inetpub\wwwroot\sistemaempresas\appsettings.Production.json
```

Conteúdo:

```json
{
  "Urls": "https://0.0.0.0:5001",
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning",
      "System.Net.Security": "Error"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "ConexaoPadrao": "Server=NOME_SERVIDOR\\INSTANCIA;Database=SistemaEmpresas;User Id=sa;Password=SUA_SENHA;TrustServerCertificate=True;Encrypt=False;"
  },
  "TenantCache": {
    "ExpiracaoMinutos": 30,
    "HabilitarCache": true
  },
  "Jwt": {
    "SecretKey": "GERE_UMA_CHAVE_SUPER_SEGURA_COM_PELO_MENOS_64_CARACTERES_AQUI",
    "Issuer": "SistemaEmpresas",
    "Audience": "SistemaEmpresasApp",
    "ExpiracaoHoras": 1,
    "RefreshTokenExpiracaoDias": 7
  }
}
```

**⚠️ IMPORTANTE:**
- Troque `NOME_SERVIDOR\INSTANCIA` pelo seu SQL Server
- Troque `SUA_SENHA` pela senha real do SQL
- Gere uma chave JWT segura (64+ caracteres aleatórios)

---

## 🔐 FASE 3: Configurar Certificado SSL (Let's Encrypt)

### **Passo 3.1: Instalar Certify The Web**

1. Baixe o instalador:
   - Site: https://certifytheweb.com/
   - Versão: Community (Gratuita)

2. Execute o instalador:
   - `CertifyTheWeb-Setup.exe`
   - Aceite os termos
   - Instale com configurações padrão

3. Abra **Certify The Web** após instalação

---

### **Passo 3.2: Criar Site no IIS (Antes do Certificado)**

No IIS Manager:

1. **Sites** → Botão direito → **Add Website**

2. Preencha:
   ```
   Site name: SistemaEmpresas
   Physical path: C:\inetpub\wwwroot\sistemaempresas
   Binding:
     Type: http
     IP address: All Unassigned
     Port: 80
     Host name: sistema.suaempresa.com.br
   ```

3. Clique **OK**

4. **Application Pools** → **SistemaEmpresas**:
   - **.NET CLR Version:** No Managed Code
   - **Managed Pipeline Mode:** Integrated
   - **Start Mode:** AlwaysRunning

5. Clique em **Advanced Settings**:
   - **Process Model** → **Identity:** ApplicationPoolIdentity
   - **Recycling** → **Regular Time Interval:** 0 (desabilita reciclagem automática)

---

### **Passo 3.3: Solicitar Certificado Let's Encrypt**

No **Certify The Web**:

1. Clique em **New Certificate**

2. **Identifier:**
   - Type: DNS
   - Domain: `sistema.suaempresa.com.br`
   - Clique **Add Domain** se tiver `www.sistema.suaempresa.com.br`

3. **Authorization:**
   - Challenge Type: **http-01** (padrão)
   - Validation: **Local IIS Server**

4. **Deployment:**
   - Deployment Mode: **Auto**
   - Select Website: **SistemaEmpresas**

5. **Advanced:**
   - Certificate Authority: **Let's Encrypt (Production)**
   - ✅ Enable Auto Renewal (renovação automática a cada 60 dias)

6. Clique **Request Certificate**

7. Aguarde validação (1-2 minutos)

8. ✅ Certificado instalado com sucesso!

---

### **Passo 3.4: Configurar HTTPS Binding no IIS**

Agora que o certificado está instalado:

1. No IIS Manager → **Sites** → **SistemaEmpresas** → **Bindings**

2. Clique **Add**:
   ```
   Type: https
   IP address: All Unassigned
   Port: 443
   Host name: sistema.suaempresa.com.br
   SSL certificate: sistema.suaempresa.com.br (Let's Encrypt)
   ✅ Require Server Name Indication
   ```

3. **Opcional:** Manter binding HTTP para redirecionamento automático

4. Clique **OK**

---

### **Passo 3.5: Configurar URL Rewrite (Redirecionar HTTP → HTTPS)**

1. Instale **URL Rewrite Module**:
   - Download: https://www.iis.net/downloads/microsoft/url-rewrite
   - Execute o instalador

2. Reinicie IIS Manager

3. Selecione site **SistemaEmpresas** → **URL Rewrite**

4. **Add Rule(s)** → **Blank Rule**

5. Preencha:
   ```
   Name: Redirect to HTTPS
   Match URL:
     Requested URL: Matches the Pattern
     Using: Regular Expressions
     Pattern: (.*)
   
   Conditions:
     Input: {HTTPS}
     Type: Matches the Pattern
     Pattern: ^OFF$
   
   Action:
     Action type: Redirect
     Redirect URL: https://{HTTP_HOST}/{R:1}
     Redirect type: Permanent (301)
   ```

6. Clique **Apply**

---

## 🗄️ FASE 4: Configurar Banco de Dados

### **Passo 4.1: Restaurar/Criar Banco de Dados**

No SQL Server Management Studio (SSMS):

```sql
-- 1. Criar banco TenantDB (controle de tenants)
CREATE DATABASE TenantDB;
GO

USE TenantDB;
GO

-- 2. Executar migration do TenantDB
-- Copie o conteúdo do arquivo: SistemaEmpresas/Migrations/TenantDbContextModelSnapshot.cs
-- Ou execute via Entity Framework:
```

No servidor (PowerShell):

```powershell
cd C:\inetpub\wwwroot\sistemaempresas

# Aplicar migrations do TenantDB
dotnet ef database update --context TenantDbContext

# Aplicar migrations do AppDB (para cada tenant)
dotnet ef database update --context AppDbContext
```

---

### **Passo 4.2: Configurar Tenants**

```sql
USE TenantDB;
GO

-- Inserir tenants
INSERT INTO Tenants (Nome, Dominio, ConnectionString, Ativo)
VALUES 
('Irrigação Penápolis', 'irrigacao', 
 'Server=SEU_SERVIDOR\INSTANCIA;Database=IRRIGACAO;User Id=sa;Password=SENHA;TrustServerCertificate=True;', 
 1),
('Chinellato Transportes', 'chinellato', 
 'Server=SEU_SERVIDOR\INSTANCIA;Database=ChinellatoTransportes;User Id=sa;Password=SENHA;TrustServerCertificate=True;', 
 1);
GO
```

---

### **Passo 4.3: Criar Bancos de Dados dos Tenants**

```sql
-- Para cada tenant, criar o banco
CREATE DATABASE IRRIGACAO;
GO

CREATE DATABASE ChinellatoTransportes;
GO

-- Executar migrations em cada banco
-- (será feito automaticamente na primeira execução da aplicação)
```

---

## 🎯 FASE 5: Testar e Validar

### **Passo 5.1: Testar Aplicação**

1. Reinicie o site no IIS:
   ```powershell
   Restart-WebAppPool -Name "SistemaEmpresas"
   ```

2. Acesse no navegador:
   ```
   https://sistema.suaempresa.com.br
   ```

3. Verifique:
   - ✅ Certificado SSL válido (cadeado verde)
   - ✅ Redirecionamento HTTP → HTTPS funciona
   - ✅ Página de login aparece
   - ✅ Console do navegador sem erros

---

### **Passo 5.2: Testar Login**

1. Tente fazer login com usuário padrão
2. Verifique logs em: `C:\inetpub\wwwroot\sistemaempresas\Logs`

---

### **Passo 5.3: Verificar Renovação Automática**

No **Certify The Web**:
- ✅ Status: **Certificate OK**
- ✅ Next Renewal: Data futura (60 dias)
- ✅ Auto Renewal: Enabled

---

## 🔧 FASE 6: Configurações Avançadas (Opcional)

### **Backup Automático**

Crie tarefa agendada para backup:

```powershell
# Script de backup (salvar como C:\Scripts\backup-sistema.ps1)
$date = Get-Date -Format "yyyyMMdd-HHmmss"
$backupPath = "D:\Backups\SistemaEmpresas\$date"

# Backup SQL
sqlcmd -S localhost\INSTANCIA -Q "BACKUP DATABASE [IRRIGACAO] TO DISK='$backupPath\IRRIGACAO.bak' WITH INIT"

# Backup arquivos
Copy-Item "C:\inetpub\wwwroot\sistemaempresas\certificados" -Destination "$backupPath\certificados" -Recurse
```

Agendar no Task Scheduler:
- Trigger: Diariamente às 2:00 AM
- Action: `powershell.exe -File C:\Scripts\backup-sistema.ps1`

---

### **Monitoramento**

1. **Application Insights** (Azure - opcional)
2. **Windows Performance Monitor**
3. **IIS Logs:** `C:\inetpub\logs\LogFiles`

---

## ✅ Checklist Final de Deploy

### Infraestrutura:
- [ ] DNS configurado e propagado
- [ ] Firewall liberado (portas 80, 443)
- [ ] .NET 8.0 Runtime instalado
- [ ] IIS instalado e configurado

### Aplicação:
- [ ] Arquivos publicados em `C:\inetpub\wwwroot\sistemaempresas`
- [ ] `appsettings.Production.json` configurado
- [ ] Permissões corretas no IIS
- [ ] Site criado no IIS

### Certificado SSL:
- [ ] Certify The Web instalado
- [ ] Certificado Let's Encrypt solicitado
- [ ] HTTPS binding configurado
- [ ] Redirecionamento HTTP → HTTPS funcionando
- [ ] Renovação automática ativada

### Banco de Dados:
- [ ] TenantDB criado e configurado
- [ ] Bancos dos tenants criados
- [ ] Migrations aplicadas
- [ ] Dados de teste inseridos

### Testes:
- [ ] Site acessível via HTTPS
- [ ] Certificado válido (cadeado verde)
- [ ] Login funciona
- [ ] API responde corretamente
- [ ] Logs sendo gerados

### Segurança:
- [ ] Senha JWT forte configurada
- [ ] Senhas de SQL Server seguras
- [ ] Backup configurado
- [ ] HSTS habilitado

---

## 🆘 Solução de Problemas

### Erro: "HTTP Error 502.5 - Process Failure"

**Causa:** .NET Runtime não instalado ou pool incorreto

**Solução:**
```powershell
# Verificar runtime
dotnet --list-runtimes

# Reconfigurar pool
Set-ItemProperty "IIS:\AppPools\SistemaEmpresas" -Name managedRuntimeVersion -Value ""
```

---

### Erro: "Unable to configure HTTPS endpoint"

**Causa:** Certificado não encontrado

**Solução:**
1. Verifique binding no IIS
2. Reemita certificado no Certify The Web

---

### Erro: "Cannot connect to SQL Server"

**Causa:** Connection string incorreta ou firewall SQL

**Solução:**
```powershell
# Testar conexão
sqlcmd -S SERVIDOR\INSTANCIA -U sa -P SENHA
```

---

### Site lento ou não responde

**Causa:** Pool reciclando ou memória insuficiente

**Solução:**
```powershell
# Aumentar memória do pool
Set-ItemProperty "IIS:\AppPools\SistemaEmpresas" -Name recycling.periodicRestart.memory -Value 0

# Desabilitar idle timeout
Set-ItemProperty "IIS:\AppPools\SistemaEmpresas" -Name processModel.idleTimeout -Value ([TimeSpan]::FromMinutes(0))
```

---

## 📞 Suporte

Se encontrar problemas:

1. Verifique logs em: `C:\inetpub\wwwroot\sistemaempresas\Logs`
2. Verifique Event Viewer: Aplicativos e Serviços → Microsoft → Windows → IIS
3. Teste acesso direto: `https://localhost` no servidor

---

## 🎉 Pronto!

Seu **Sistema Empresas** está agora em produção com:
- ✅ **HTTPS válido** (Let's Encrypt)
- ✅ **Renovação automática** de certificado
- ✅ **Multi-tenant** funcionando
- ✅ **Seguro** e **escalável**

**Parabéns pelo deploy!** 🚀🎊
