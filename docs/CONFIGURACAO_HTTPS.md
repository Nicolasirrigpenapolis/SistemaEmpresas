# 🔒 Configuração HTTPS - Sistema Empresas

## ✅ Configuração Concluída

O sistema foi configurado para usar **HTTPS** tanto em **desenvolvimento** quanto em **produção**.

---

## 📋 Alterações Realizadas

### **Backend (.NET)**
✅ `appsettings.Development.json` → `https://0.0.0.0:5001`  
✅ `appsettings.example.json` → `https://0.0.0.0:5001`  
✅ `Program.cs` → HTTPS Redirection + HSTS  
✅ `CORS` → Apenas origens HTTPS permitidas  

### **Frontend (React + Vite)**
✅ `.env.development` → `https://localhost:5001/api`  
✅ `.env.example` → `https://localhost:5001/api`  
✅ `api.ts` → baseURL padrão HTTPS  
✅ `vite.config.ts` → Servidor HTTPS com certificados  

---

## 🔐 Certificados para Desenvolvimento

### **Opção 1: Usar Certificado do .NET (Recomendado)**

O .NET já possui certificados de desenvolvimento. Verifique se está instalado:

```powershell
dotnet dev-certs https --check
```

Se não estiver instalado, execute:

```powershell
# Limpar certificados antigos
dotnet dev-certs https --clean

# Gerar e confiar no novo certificado
dotnet dev-certs https --trust
```

Isso criará um certificado em:
- Windows: `%APPDATA%\ASP.NET\https\`
- Linux/Mac: `~/.aspnet/https/`

### **Opção 2: Gerar Certificados Manualmente (Para Vite)**

Para o Vite funcionar com HTTPS, você precisa de certificados `.pem`:

#### **Windows (PowerShell):**

```powershell
# Navegue até a pasta de certificados
cd C:\Projetos\SistemaEmpresas2\SistemaEmpresas\certificado

# Gerar certificado usando OpenSSL (instale se não tiver)
# Baixar OpenSSL: https://slproweb.com/products/Win32OpenSSL.html

openssl req -x509 -newkey rsa:4096 -keyout localhost-key.pem -out localhost.pem -days 365 -nodes -subj "/CN=localhost"
```

#### **Linux/Mac:**

```bash
cd /caminho/para/SistemaEmpresas2/SistemaEmpresas/certificado

openssl req -x509 -newkey rsa:4096 -keyout localhost-key.pem -out localhost.pem -days 365 -nodes -subj "/CN=localhost"
```

### **Opção 3: Usar mkcert (Mais Fácil)**

```powershell
# Instalar mkcert (Windows com Chocolatey)
choco install mkcert

# Ou com Scoop
scoop bucket add extras
scoop install mkcert

# Linux
sudo apt install mkcert  # Ubuntu/Debian
brew install mkcert      # Mac

# Instalar CA local
mkcert -install

# Gerar certificados
cd C:\Projetos\SistemaEmpresas2\SistemaEmpresas\certificado
mkcert -key-file localhost-key.pem -cert-file localhost.pem localhost 127.0.0.1 ::1
```

---

## 🚀 Como Executar

### **1. Backend (.NET)**

```powershell
cd C:\Projetos\SistemaEmpresas2\SistemaEmpresas
dotnet run
```

Acesse: `https://localhost:5001/swagger`

### **2. Frontend (Vite)**

```powershell
cd C:\Projetos\SistemaEmpresas2\frontend
npm run dev
```

Acesse: `https://localhost:5173`

---

## ⚠️ Avisos de Segurança do Navegador

Se você ver avisos sobre **certificado não confiável**:

1. **Chrome/Edge**: Clique em "Avançado" → "Continuar para localhost (não seguro)"
2. **Firefox**: "Avançado" → "Aceitar o risco e continuar"

**Ou:** Use `mkcert` (Opção 3) para certificados totalmente confiáveis.

---

## 🔧 Solução de Problemas

### **Erro: "Unable to configure HTTPS endpoint"**

```powershell
# Limpar e regenerar certificado do .NET
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

### **Erro: "Cannot find module 'fs'"**

O Vite precisa de `@types/node`:

```bash
cd frontend
npm install -D @types/node
```

### **Erro: "ERR_CERT_AUTHORITY_INVALID"**

Use `mkcert` ou aceite manualmente o certificado no navegador.

### **Erro: "CORS policy"**

Verifique se o frontend está acessando via HTTPS:
- ✅ `https://localhost:5173`
- ❌ `http://localhost:5173`

---

## 📝 Produção

Para produção, use certificados válidos:

1. **Let's Encrypt** (gratuito)
2. **Certificado comercial** (Comodo, DigiCert, etc.)
3. Configure no servidor (IIS, Nginx, Apache)

No `appsettings.Production.json`, ajuste a URL conforme seu domínio:

```json
{
  "Urls": "https://seudominio.com.br:443"
}
```

---

## ✅ Checklist Final

- [ ] Certificados gerados na pasta `certificado/`
- [ ] `dotnet dev-certs https --trust` executado
- [ ] Backend inicia sem erros em `https://localhost:5001`
- [ ] Frontend inicia sem erros em `https://localhost:5173`
- [ ] Login funciona corretamente
- [ ] CORS sem erros no console do navegador
- [ ] Certificado aceito pelo navegador

---

## 🎉 Pronto!

Seu sistema agora está **100% HTTPS** para desenvolvimento e produção! 🔒✨

**Benefícios:**
- ✅ Comunicação criptografada
- ✅ Cookies seguros (Secure flag)
- ✅ Service Workers funcionam
- ✅ APIs modernas funcionam (Geolocation, Camera, etc.)
- ✅ Preparado para produção
