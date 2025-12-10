# 🏢 SistemaEmpresas

Sistema ERP multi-tenant desenvolvido em ASP.NET Core 8 + React 19 para gestão empresarial completa.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)
![React](https://img.shields.io/badge/React-19-61DAFB?style=flat&logo=react)
![TypeScript](https://img.shields.io/badge/TypeScript-5.0-3178C6?style=flat&logo=typescript)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019+-CC2927?style=flat&logo=microsoftsqlserver)

---

## 📋 Índice

- [Visão Geral](#visão-geral)
- [Tecnologias](#tecnologias)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Requisitos](#requisitos)
- [Instalação](#instalação)
- [Configuração](#configuração)
- [Executando](#executando)
- [Documentação](#documentação)

---

## 🎯 Visão Geral

O **SistemaEmpresas** é uma solução web multi-tenant que moderniza o sistema legado VB6, oferecendo:

- ✅ **Multi-tenant por banco de dados** - Cada empresa tem seu próprio banco
- ✅ **Autenticação JWT** - Login seguro com refresh token
- ✅ **Sistema de Permissões** - Controle granular por tela/ação
- ✅ **Módulo Fiscal** - Integração ClassTrib, NF-e
- ✅ **Módulo Transporte** - Veículos, Motoristas, Viagens, Manutenções
- ✅ **Dashboard** - KPIs e métricas em tempo real
- ✅ **UI Moderna** - React + TailwindCSS responsivo

---

## 🛠️ Tecnologias

### Backend
- **ASP.NET Core 8** - Framework web
- **Entity Framework Core** - ORM
- **SQL Server** - Banco de dados
- **JWT Bearer** - Autenticação
- **Swagger/OpenAPI** - Documentação de API

### Frontend
- **React 19** - Biblioteca UI
- **TypeScript 5** - Tipagem estática
- **Vite** - Build tool
- **TailwindCSS** - Estilização
- **Axios** - Cliente HTTP
- **Lucide React** - Ícones

---

## 📁 Estrutura do Projeto

```
SistemaEmpresas/
├── 📂 docs/                    # Documentação
│   ├── README.md               # Índice de documentos
│   ├── PRD.md                  # Requisitos do produto
│   ├── GUIA_RAPIDO.md          # Primeiros passos
│   └── ...
│
├── 📂 frontend/                # Aplicação React
│   ├── src/
│   │   ├── components/         # Componentes reutilizáveis
│   │   ├── contexts/           # React Contexts (Auth, etc)
│   │   ├── hooks/              # Custom hooks
│   │   ├── pages/              # Páginas/rotas
│   │   ├── services/           # Chamadas API
│   │   ├── types/              # TypeScript types
│   │   └── utils/              # Utilitários
│   └── ...
│
├── 📂 SistemaEmpresas/         # Backend ASP.NET Core
│   ├── Controllers/            # API endpoints
│   ├── Data/                   # DbContext
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Enums/                  # Enumerações
│   ├── Middleware/             # Middlewares (Tenant, etc)
│   ├── Migrations/             # EF Core migrations
│   ├── Models/                 # Entidades do banco
│   ├── Repositories/           # Padrão Repository
│   ├── Services/               # Lógica de negócio
│   └── Program.cs              # Entry point
│
├── 📂 SistemaEmpresas.Tests/   # Testes unitários
├── 📂 scripts/                 # Scripts SQL úteis
└── SistemaEmpresas.sln         # Solution Visual Studio
```

---

## 📋 Requisitos

- **.NET SDK 8.0+**
- **Node.js 18+** e **npm 9+**
- **SQL Server 2019+** (ou SQL Server Express)
- **Visual Studio 2022** ou **VS Code**

---

## 🚀 Instalação

### 1. Clone o repositório
```bash
git clone https://github.com/Nicolasirrigpenapolis/SistemaEmpresas.git
cd SistemaEmpresas
```

### 2. Backend
```bash
cd SistemaEmpresas
dotnet restore
dotnet build
```

### 3. Frontend
```bash
cd frontend
npm install
```

---

## ⚙️ Configuração

### Backend (`appsettings.json`)

Copie o exemplo e configure:
```bash
copy appsettings.example.json appsettings.json
```

Configure a connection string:
```json
{
  "ConnectionStrings": {
    "ConexaoPadrao": "Server=SEU_SERVIDOR;Database=SEU_BANCO;User Id=usuario;Password=senha;TrustServerCertificate=True"
  },
  "Jwt": {
    "Secret": "SUA_CHAVE_SECRETA_COM_PELO_MENOS_32_CARACTERES",
    "Issuer": "SistemaEmpresas",
    "Audience": "SistemaEmpresasApp"
  }
}
```

### Frontend (`.env`)

Copie o exemplo e configure:
```bash
copy .env.example .env
```

```env
VITE_API_URL=http://localhost:5001/api
```

---

## ▶️ Executando

### Desenvolvimento

**Terminal 1 - Backend:**
```bash
cd SistemaEmpresas
dotnet run
# API disponível em: https://localhost:5001
# Swagger: https://localhost:5001/swagger
```

**Terminal 2 - Frontend:**
```bash
cd frontend
npm run dev
# App disponível em: http://localhost:5173
```

### Produção

Consulte [docs/DOCUMENTACAO_DEPLOY.md](docs/DOCUMENTACAO_DEPLOY.md)

---

## 📚 Documentação

| Documento | Descrição |
|-----------|-----------|
| [PRD.md](docs/PRD.md) | Requisitos do produto |
| [GUIA_RAPIDO.md](docs/GUIA_RAPIDO.md) | Primeiros passos |
| [GUIA_PERMISSOES.md](docs/GUIA_PERMISSOES.md) | Sistema de permissões |
| [DOCUMENTACAO_DEPLOY.md](docs/DOCUMENTACAO_DEPLOY.md) | Deploy em produção |
| [VERSIONAMENTO_SISTEMA.md](docs/VERSIONAMENTO_SISTEMA.md) | Changelog |

---

## 📄 Licença

Projeto proprietário - Todos os direitos reservados.

---

**Desenvolvido com ❤️ pela equipe SistemaEmpresas**
