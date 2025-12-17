# 🔒 Guia de Instalação SSL/HTTPS no Servidor

## 📋 Índice
1. [Pré-requisitos](#pré-requisitos)
2. [Opção 1: Certificado SSL Gratuito (Let's Encrypt)](#opção-1-certificado-ssl-gratuito-lets-encrypt)
3. [Opção 2: Certificado SSL Pago](#opção-2-certificado-ssl-pago)
4. [Opção 3: Certificado Auto-Assinado (Apenas para Testes)](#opção-3-certificado-auto-assinado-apenas-para-testes)
5. [Configuração do Backend .NET](#configuração-do-backend-net)
6. [Configuração do Nginx (Recomendado)](#configuração-do-nginx-recomendado)
7. [Renovação Automática do Certificado](#renovação-automática-do-certificado)
8. [Troubleshooting](#troubleshooting)

---

## 🎯 Pré-requisitos

- Servidor com IP público
- Domínio apontando para o servidor (ex: `sistemaempresas.com.br`)
- Portas 80 (HTTP) e 443 (HTTPS) abertas no firewall
- Windows Server com IIS ou Linux com Nginx/Apache

---

## 🆓 Opção 1: Certificado SSL Gratuito (Let's Encrypt)

### **Recomendado para produção!**

### Windows Server + IIS

1. **Instalar Win-ACME**
   ```powershell
   # Baixar Win-ACME
   Invoke-WebRequest -Uri "https://github.com/win-acme/win-acme/releases/latest/download/win-acme.v2.x.x.x.zip" -OutFile "win-acme.zip"
   
   # Extrair
   Expand-Archive -Path "win-acme.zip" -DestinationPath "C:\win-acme"
   ```

2. **Executar Win-ACME**
   ```powershell
   cd C:\win-acme
   .\wacs.exe
   ```

3. **Selecionar opções:**
   - N: Criar novo certificado
   - 2: IIS Bindings
   - Selecionar o site
   - 2: RSA key
   - Yes: Aceitar termos

4. **O certificado será automaticamente instalado no IIS**

### Linux Server (Ubuntu/Debian)

1. **Instalar Certbot**
   ```bash
   sudo apt update
   sudo apt install certbot python3-certbot-nginx
   ```

2. **Obter Certificado**
   ```bash
   sudo certbot --nginx -d sistemaempresas.com.br -d www.sistemaempresas.com.br
   ```

3. **Informações solicitadas:**
   - Email para notificações
   - Aceitar termos de serviço
   - Redirecionar HTTP para HTTPS? → SIM

---

## 💳 Opção 2: Certificado SSL Pago

### Fornecedores Recomendados
- **Comodo/Sectigo**: R$ 150-300/ano
- **DigiCert**: R$ 500-1000/ano (mais confiável)
- **GoDaddy**: R$ 200-400/ano
- **SSL.com**: R$ 180-350/ano

### Processo de Instalação

1. **Gerar CSR (Certificate Signing Request)**
   
   **No Windows:**
   - Abrir IIS Manager
   - Server Certificates → Create Certificate Request
   - Preencher informações da empresa
   - Salvar como `.txt`

   **No Linux:**
   ```bash
   openssl req -new -newkey rsa:2048 -nodes -keyout server.key -out server.csr
   ```

2. **Enviar CSR para a autoridade certificadora**
   - Copiar conteúdo do arquivo `.csr`
   - Colar no site do fornecedor
   - Validar domínio (email, DNS ou arquivo)

3. **Baixar certificados**
   - Certificado principal (`.cer` ou `.crt`)
   - Certificado intermediário (`.ca-bundle`)
   - Chave privada (já tem do passo 1)

4. **Instalar no servidor**
   
   **Windows/IIS:**
   - Complete Certificate Request no IIS
   - Bind para o site na porta 443
   
   **Linux:**
   - Converter para formato adequado
   - Configurar no Nginx/Apache

---

## 🧪 Opção 3: Certificado Auto-Assinado (Apenas para Testes)

### **⚠️ NÃO usar em produção!**

```powershell
# PowerShell (Windows)
New-SelfSignedCertificate `
    -DnsName "localhost", "sistemaempresas.local" `
    -CertStoreLocation "cert:\LocalMachine\My" `
    -NotAfter (Get-Date).AddYears(2) `
    -FriendlyName "Sistema Empresas Dev" `
    -KeyUsage DigitalSignature,KeyEncipherment
```

```bash
# Linux
openssl req -x509 -newkey rsa:4096 -keyout key.pem -out cert.pem -days 365 -nodes
```

---

## ⚙️ Configuração do Backend .NET

### appsettings.Production.json

```json
{
  "Kestrel": {
    "Endpoints": {
      "Https": {
        "Url": "https://*:5001",
        "Certificate": {
          "Path": "C:\\certificados\\sistemaempresas.pfx",
          "Password": "SUA_SENHA_AQUI"
        }
      },
      "Http": {
        "Url": "http://*:5000"
      }
    }
  },
  "AllowedHosts": "sistemaempresas.com.br;www.sistemaempresas.com.br"
}
```

### Converter certificados para .pfx (se necessário)

```bash
# Linux
openssl pkcs12 -export -out certificate.pfx -inkey private.key -in certificate.crt -certfile ca_bundle.crt

# Windows (PowerShell)
Get-ChildItem -Path cert:\LocalMachine\My\<THUMBPRINT> | Export-PfxCertificate -FilePath C:\certificados\sistemaempresas.pfx -Password $pwd
```

---

## 🔧 Configuração do Nginx (Recomendado)

### Por que usar Nginx?
- Melhor performance
- Renovação automática de certificados
- Load balancing
- Cache de arquivos estáticos

### /etc/nginx/sites-available/sistemaempresas

```nginx
# Redirecionar HTTP para HTTPS
server {
    listen 80;
    listen [::]:80;
    server_name sistemaempresas.com.br www.sistemaempresas.com.br;
    
    return 301 https://$server_name$request_uri;
}

# HTTPS - Backend API
server {
    listen 443 ssl http2;
    listen [::]:443 ssl http2;
    server_name api.sistemaempresas.com.br;

    # SSL Configuration
    ssl_certificate /etc/letsencrypt/live/sistemaempresas.com.br/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/sistemaempresas.com.br/privkey.pem;
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_ciphers HIGH:!aNULL:!MD5;
    ssl_prefer_server_ciphers on;

    # Proxy para Backend .NET
    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
    }
}

# HTTPS - Frontend
server {
    listen 443 ssl http2;
    listen [::]:443 ssl http2;
    server_name sistemaempresas.com.br www.sistemaempresas.com.br;

    # SSL Configuration
    ssl_certificate /etc/letsencrypt/live/sistemaempresas.com.br/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/sistemaempresas.com.br/privkey.pem;
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_ciphers HIGH:!aNULL:!MD5;
    ssl_prefer_server_ciphers on;

    # Security Headers
    add_header Strict-Transport-Security "max-age=31536000; includeSubDomains" always;
    add_header X-Frame-Options "SAMEORIGIN" always;
    add_header X-Content-Type-Options "nosniff" always;
    add_header X-XSS-Protection "1; mode=block" always;

    # Frontend estático
    root /var/www/sistemaempresas/frontend/dist;
    index index.html;

    location / {
        try_files $uri $uri/ /index.html;
    }

    # Cache de assets
    location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff|woff2|ttf|eot)$ {
        expires 1y;
        add_header Cache-Control "public, immutable";
    }

    # Compressão Gzip
    gzip on;
    gzip_vary on;
    gzip_min_length 1024;
    gzip_types text/plain text/css text/xml text/javascript application/x-javascript application/javascript application/xml+rss application/json;
}
```

### Ativar configuração

```bash
sudo ln -s /etc/nginx/sites-available/sistemaempresas /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx
```

---

## 🔄 Renovação Automática do Certificado

### Let's Encrypt com Certbot

```bash
# Testar renovação
sudo certbot renew --dry-run

# Renovação automática já vem configurada via systemd ou cron
# Verificar timer
sudo systemctl status certbot.timer

# Ou ver no cron
sudo crontab -l
```

### Win-ACME (Windows)

- A renovação automática é configurada automaticamente via Task Scheduler
- Verificar em: Task Scheduler → win-acme

---

## 🐛 Troubleshooting

### Erro: "NET::ERR_CERT_AUTHORITY_INVALID"
**Solução:** Certificado auto-assinado ou não confiável
- Use Let's Encrypt
- Ou instale certificado pago de CA confiável

### Erro: "Connection Refused" na porta 443
**Verificar:**
```bash
# Linux
sudo netstat -tlnp | grep :443
sudo ufw status

# Windows
netstat -ano | findstr :443
netsh advfirewall firewall show rule name=all | findstr 443
```

### Erro: "Mixed Content" no navegador
**Solução:** Certificar que todos os recursos (CSS, JS, imagens) usam HTTPS
```javascript
// No frontend, usar URLs relativas ou HTTPS
const API_URL = process.env.VITE_API_URL || 'https://api.sistemaempresas.com.br';
```

### Certificado não renova automaticamente
```bash
# Ver logs do certbot
sudo journalctl -u certbot

# Forçar renovação
sudo certbot renew --force-renewal
```

### Performance ruim após SSL
**Otimizações:**
1. Habilitar HTTP/2 (já está no nginx acima)
2. Usar cache de sessão SSL
3. Habilitar OCSP Stapling

```nginx
# Adicionar ao bloco server
ssl_session_cache shared:SSL:10m;
ssl_session_timeout 10m;
ssl_stapling on;
ssl_stapling_verify on;
```

---

## ✅ Checklist Final

- [ ] Certificado SSL instalado
- [ ] Porta 443 aberta no firewall
- [ ] Redirecionamento HTTP → HTTPS configurado
- [ ] Backend respondendo em HTTPS
- [ ] Frontend carregando via HTTPS
- [ ] Headers de segurança configurados
- [ ] Renovação automática testada
- [ ] Teste em https://www.ssllabs.com/ssltest/
- [ ] Backup dos certificados e chaves privadas

---

## 📞 Suporte

Se tiver dúvidas, consulte:
- **Let's Encrypt:** https://letsencrypt.org/docs/
- **Certbot:** https://certbot.eff.org/
- **Nginx:** https://nginx.org/en/docs/
- **ASP.NET Core HTTPS:** https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl

---

*Última atualização: Dezembro 2025*
