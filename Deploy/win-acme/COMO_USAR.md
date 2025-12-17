# 🔒 Win-ACME - Gerador de Certificado SSL GRÁTIS (Let's Encrypt)

## ✅ JÁ ESTÁ BAIXADO E PRONTO!

Localização: `Deploy\win-acme\`

## 🚀 Como Usar (MUITO SIMPLES):

### 1. Executar como Administrador

```powershell
cd Deploy\win-acme
.\wacs.exe
```

### 2. Seguir o Menu Interativo

Quando abrir, você verá um menu. Escolha:

```
N - Criar novo certificado
```

### 3. Escolher o Tipo

```
1 - Manual input (se não usar IIS)
2 - IIS bindings (se usar IIS - RECOMENDADO)
```

### 4. Informar Dados

- **Email:** seu-email@exemplo.com (para notificações de renovação)
- **Domínio:** sistemaempresas.com.br
- **Aceitar termos:** Yes

### 5. Pronto! 🎉

O certificado será:
- ✅ Gerado automaticamente
- ✅ Instalado no Windows
- ✅ Configurado para renovação automática (a cada 60 dias)
- ✅ Salvo em: `C:\ProgramData\win-acme\`

## 📋 Requisitos IMPORTANTES:

⚠️ **ANTES de executar, certifique-se:**

1. ✅ Você tem um **domínio registrado** (ex: sistemaempresas.com.br)
2. ✅ O domínio está **apontando para o IP do seu servidor**
3. ✅ A **porta 80 está aberta** no firewall (Let's Encrypt precisa validar)
4. ✅ Você está executando como **Administrador**

## 🔍 Verificar se Domínio Está Apontando:

```powershell
# Verificar DNS
nslookup sistemaempresas.com.br

# Deve retornar o IP do seu servidor
```

## 🎯 Após Gerar o Certificado:

O Win-ACME vai perguntar onde instalar. Escolha:

- **IIS:** Automático (se usar IIS)
- **Manual:** Ele vai te dar o caminho do arquivo `.pfx`

### Usar no Sistema Empresas:

1. Copie o arquivo `.pfx` para `SistemaEmpresas\certificado\`
2. Edite `appsettings.json`:

```json
"Certificate": {
  "Path": "certificado/SEU_DOMINIO.pfx",
  "Password": "SENHA_GERADA_PELO_WINACME"
}
```

## 🔄 Renovação Automática

O Win-ACME cria uma tarefa agendada no Windows que renova automaticamente!

Verificar: **Task Scheduler** → **win-acme**

## ❓ Troubleshooting

### Erro: "Could not validate domain"
- Verifique se o domínio aponta para o servidor
- Confirme que porta 80 está aberta
- Teste: `http://seu-dominio.com.br` (deve responder)

### Erro: "Access denied"
- Execute como Administrador
- Clique direito em `wacs.exe` → "Executar como administrador"

### Precisa de ajuda?
Execute: `.\wacs.exe --help`

---

## 🎁 BÔNUS: Certificado para Múltiplos Domínios

O Win-ACME pode gerar um certificado para:
- sistemaempresas.com.br
- www.sistemaempresas.com.br
- api.sistemaempresas.com.br

Tudo de uma vez! Basta informar todos os domínios quando solicitado.

---

**Let's Encrypt = SSL Grátis, Confiável e Automático! 🔒✨**
