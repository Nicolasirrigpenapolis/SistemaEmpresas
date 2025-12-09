# 🚀 GUIA RÁPIDO - SISTEMA EMPRESAS

## 📋 Antes de Tudo (Primeira Vez)

**No Servidor**, abra PowerShell como Administrador e execute:

```powershell
# 1. Instalar como serviço Windows
cd C:\SistemaEmpresas\publish
.\install-service.ps1 -Install

# 2. Iniciar o serviço
.\install-service.ps1 -Start

# 3. Verificar status
.\install-service.ps1 -Status
```

Pronto! O serviço vai rodar automaticamente mesmo após reiniciar o servidor.

---

## 🔄 Fluxo de Desenvolvimento

### 1️⃣ Fazer Alterações no Código

Edite os arquivos no VS Code:
```
Frontend: C:\Projetos\SistemaEmpresas\frontend\src\
Backend:  C:\Projetos\SistemaEmpresas\SistemaEmpresas\
```

### 2️⃣ Testar Localmente

```powershell
# Terminal 1 - Frontend
cd C:\Projetos\SistemaEmpresas\frontend
npm run dev

# Terminal 2 - Backend
cd C:\Projetos\SistemaEmpresas\SistemaEmpresas
dotnet run
```

Acesse: http://localhost:5173 (Frontend) ou http://localhost:5196 (API)

### 3️⃣ Gerar Nova Versão

```powershell
cd C:\Projetos\SistemaEmpresas
.\build.ps1
```

**Saída esperada:**
```
✅ Frontend compilado
✅ Backend compilado
✅ Frontend copiado para wwwroot
✅ BUILD CONCLUÍDO COM SUCESSO!
```

### 4️⃣ Fazer Deploy (Atualizar Servidor)

```powershell
cd C:\Projetos\SistemaEmpresas
.\build.ps1 -Server
```

**O que acontece:**
1. Cria nova build
2. Para o serviço no servidor
3. Copia arquivos para `C:\SistemaEmpresas\publish`
4. Inicia o serviço novamente
5. Sistema fica online em poucos segundos

---

## 🛠️ Gerenciamento do Serviço (Servidor)

### Status Atual
```powershell
.\install-service.ps1 -Status
```

### Iniciar
```powershell
.\install-service.ps1 -Start
```

### Parar
```powershell
.\install-service.ps1 -Stop
```

### Reiniciar
```powershell
.\install-service.ps1 -Stop
Start-Sleep -Seconds 2
.\install-service.ps1 -Start
```

---

## 📊 Checklist - Antes de Colocar em Produção

- [ ] Todas as alterações estão no Git
- [ ] Frontend rodou local sem erros
- [ ] Backend rodou local sem erros
- [ ] Build executado com sucesso (`.\build.ps1`)
- [ ] Testou em http://localhost:5001
- [ ] Certificados estão em lugar (`certificado\Irrigacao.pfx`)
- [ ] SQL credentials estão corretos (admin/conectairrig@)
- [ ] Deploy executado (`.\build.ps1 -Server`)
- [ ] Serviço iniciou sem erros
- [ ] Acesso a http://IP:5001 funciona
- [ ] Testou funcionalidades principais (ClassTrib, etc)

---

## 🚨 Resolver Problemas

### Porta 5001 em Uso
```powershell
netstat -ano | findstr :5001
taskkill /PID <PID> /F
```

### Serviço não inicia
```powershell
# Ver logs
Get-EventLog -LogName Application -Source SistemaEmpresas -Newest 10

# Tentar iniciar manualmente
cd C:\SistemaEmpresas\publish
.\SistemaEmpresas.exe
```

### Erro de Certificado
```powershell
# Verificar se existe
ls C:\SistemaEmpresas\publish\certificado\

# Copiar manualmente se necessário
cp C:\Projetos\SistemaEmpresas\SistemaEmpresas\certificado\* `
   C:\SistemaEmpresas\publish\certificado\ -Force
```

### Erro de Conexão SQL
```powershell
# Testar conexão
sqlcmd -S SRVSQL\SQLEXPRESS -U admin -P "conectairrig@" -d IRRIGACAO -Q "SELECT 1"

# Atualizar connection strings dos tenants
cd C:\SistemaEmpresas\publish
.\fix_tenants.bat
```

---

## 📞 Informações Importantes

| Item | Valor |
|------|-------|
| **Porta** | 5001 |
| **URL Local** | http://localhost:5001 |
| **URL Remota** | http://IP_SERVIDOR:5001 |
| **Diretório** | C:\SistemaEmpresas\publish\ |
| **Serviço Windows** | SistemaEmpresas |
| **Banco SQL** | SRVSQL\SQLEXPRESS |
| **Usuário SQL** | admin |
| **Senha SQL** | conectairrig@ |
| **Certificado 1** | Irrigacao.pfx (irrig02781) |
| **Certificado 2** | CHINELLATO.pfx (ct220615) |

---

## 💡 Dicas Úteis

### Ver Logs em Tempo Real
```powershell
# PowerShell como Admin
Get-EventLog -LogName Application -Source SistemaEmpresas -Tail 20 -Wait
```

### Backup Rápido
```powershell
# Antes de deploy importante
Copy-Item -Path C:\SistemaEmpresas\publish `
          -Destination C:\SistemaEmpresas\publish.backup.$(Get-Date -f 'yyyyMMdd_HHmmss') `
          -Recurse
```

### Monitorar Saúde do Serviço
```powershell
# Loop que monitora
$service = "SistemaEmpresas"
while ($true) {
    $s = Get-Service $service
    Write-Host "$(Get-Date -f 'HH:mm:ss') - $($s.Name): $($s.Status)" -ForegroundColor $(if($s.Status -eq 'Running') { 'Green' } else { 'Red' })
    Start-Sleep -Seconds 30
}
```

---

## 📖 Documentação Completa

Para documentação detalhada, veja: `DOCUMENTACAO_DEPLOY.md`

