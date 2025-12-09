# Auto-Login para Desenvolvimento (Descontinuado)

## 📌 Status Atual
- ✅ Todos os ambientes (incluindo DEV) exigem autenticação manual.
- ✅ Não existe endpoint especial (`/api/auth/dev-login`) exposto pela API.
- ✅ O frontend não pré-preenche empresa/usuário/senha automaticamente.
- ❌ Variáveis como `VITE_DISABLE_DEV_AUTO_LOGIN` não têm efeito e podem ser removidas.

> **Motivação**: durante o desenvolvimento foi identificado que o fluxo automático mascarava problemas de autenticação e podia gerar acessos não intencionais. Para manter paridade com produção e evitar riscos, a funcionalidade foi removida por completo.

## 🔍 Onde verificar

| Componente | Situação atual |
|------------|----------------|
| `AuthController` | Expõe apenas `login`, `refresh`, `me`, `alterar-senha` e `logout`. |
| `authService.ts` | Possui somente chamadas padrão (`login`, `logout`, `refreshToken`, ...). |
| `AuthContext.tsx` | Carrega sessão do `localStorage` e nunca dispara login automático. |
| `LoginPage.tsx` | Mantém o passo "Selecione a empresa" e inputs vazios mesmo em `import.meta.env.DEV`. |

## 🚀 Como iniciar o ambiente DEV

```powershell
# Backend
cd C:\Projetos\SistemaEmpresas\SistemaEmpresas
dotnet run

# Frontend (novo terminal)
cd C:\Projetos\SistemaEmpresas\frontend
npm install # primeira vez
npm run dev
```

1. Abra `http://localhost:5173`.
2. Escolha a empresa desejada.
3. Informe usuário e senha válidos (ex.: credenciais de testes do time).
4. Clique em **Entrar no Sistema**.

## 🧹 Dicas de troubleshooting
- Limpe `localStorage`/`sessionStorage` se notar dados antigos.
- Se o botão estiver desabilitado, confirme se *todos* os campos estão preenchidos.
- Problemas de token expirado costumam ser resolvidos com logout e login manual.

## 🗃 Histórico
- **2024-11** – Auto-login implantado experimentalmente para agilizar QA (não chegou à produção).
- **2025-11-29** – Funcionalidade descontinuada; código e documentação atualizados para refletir o comportamento definitivo.

Caso identifique qualquer resquício de auto-login ou comportamento divergente, abra um ticket descrevendo o passo a passo para reproduzir.
