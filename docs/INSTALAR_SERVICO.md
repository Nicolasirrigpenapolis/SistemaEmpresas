# Instalação do Sistema Empresas como Serviço Windows# 🚀 INSTALAR COMO SERVIÇO WINDOWS



## Pré-requisitos## ⚡ Jeito Mais Fácil

- .NET 8.0 Runtime instalado

- Acesso de Administrador no servidor### No Servidor, na pasta `C:\SistemaEmpresas\publish\`:



## 1. Publicar a Aplicação1. **Clique com botão direito** em `run-as-admin.bat`

2. Selecione **"Executar como administrador"**

```powershell3. Escolha **opção 1** para instalar

cd c:\Projetos\SistemaEmpresas\SistemaEmpresas4. Depois escolha **opção 2** para iniciar

dotnet publish -c Release -o C:\SistemaEmpresas

```Pronto! O serviço vai ficar rodando automaticamente.



## 2. Instalar o Serviço---



Abra o **Prompt de Comando como Administrador** e execute:## 🎯 Opções do Menu



```cmd```

sc create SistemaEmpresas binPath="C:\SistemaEmpresas\SistemaEmpresas.exe" start=auto DisplayName="Sistema de Empresas"1 - Instalar como Serviço Windows

sc description SistemaEmpresas "API de Gestão de Empresas com Classificação Fiscal"2 - Iniciar Serviço

```3 - Parar Serviço

4 - Ver Status

## 3. Configurar Reinício Automático em Falhas5 - Desinstalar Serviço

0 - Sair

```cmd```

sc failure SistemaEmpresas reset=60 actions=restart/60000/restart/60000/restart/60000

```---



## 4. Iniciar o Serviço## 📌 Se Preferir Executar Manualmente



```cmdAbra **PowerShell como Administrador** e execute:

sc start SistemaEmpresas

``````powershell

# Ir para a pasta

## Comandos Úteiscd C:\SistemaEmpresas\publish



| Ação | Comando |# Instalar

|------|---------|.\install-service.ps1 -Install

| Iniciar | `sc start SistemaEmpresas` |

| Parar | `sc stop SistemaEmpresas` |# Iniciar

| Status | `sc query SistemaEmpresas` |.\install-service.ps1 -Start

| Remover | `sc delete SistemaEmpresas` |

# Ver status

## Verificar Logs.\install-service.ps1 -Status

```

Os logs ficam no **Event Viewer** do Windows:

- Abra `eventvwr.msc`---

- Navegue até: **Windows Logs > Application**

- Filtre por **Source: SistemaEmpresas**## ✅ Verificar Se Está Funcionando



## Configuração de Porta### Via Menu

```

Edite o arquivo `C:\SistemaEmpresas\appsettings.json` e configure a URL:Escolha opção 4 (Ver Status)

```

```json

{### Via PowerShell

  "Kestrel": {```powershell

    "Endpoints": {Get-Service -Name "SistemaEmpresas"

      "Http": {```

        "Url": "http://0.0.0.0:5001"

      }### Acessar a Aplicação

    }```

  }http://localhost:5001

}```

```

---

Após alterar, reinicie o serviço:

```cmd## 🛠️ Comandos Úteis

sc stop SistemaEmpresas

sc start SistemaEmpresas### Ver todos os serviços

``````powershell

Get-Service | Where-Object {$_.Name -like "*Sistema*"}
```

### Reiniciar o serviço
```powershell
Restart-Service -Name "SistemaEmpresas"
```

### Ver logs de erro
```powershell
Get-EventLog -LogName Application -Source "SistemaEmpresas" -Newest 20
```

### Parar definitivamente
```powershell
Stop-Service -Name "SistemaEmpresas" -Force
```

---

## 💡 O que Significa Cada Status?

| Status | Significado | O que fazer |
|--------|-------------|-----------|
| 🟢 Running | Serviço está ativo | Nada, está normal |
| 🔴 Stopped | Serviço parou | Execute opção 2 do menu |
| ⚠️ Error | Erro ao iniciar | Verifique logs ou SQL connection |

---

## 🚨 Se Não Conseguir Instalar

1. **Verifique se executou como Admin**
   - Clique direito > Executar como administrador

2. **Verifique se `SistemaEmpresas.exe` existe**
   ```powershell
   Test-Path C:\SistemaEmpresas\publish\SistemaEmpresas.exe
   ```

3. **Verifique se SQL está acessível**
   ```powershell
   sqlcmd -S SRVSQL\SQLEXPRESS -U admin -P "conectairrig@" -d IRRIGACAO -Q "SELECT 1"
   ```

4. **Ver erro detalhado**
   ```powershell
   cd C:\SistemaEmpresas\publish
   .\install-service.ps1 -Install
   ```

---

## 📞 Depois de Instalar

✅ Servidor vai rodar **automaticamente** mesmo após reiniciar  
✅ Aplicação fica disponível em **http://localhost:5001**  
✅ Sem necessidade de deixar `.bat` aberto  
✅ Use o menu para iniciar/parar conforme necessário
