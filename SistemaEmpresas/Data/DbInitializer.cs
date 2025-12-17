using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Models;
using SistemaEmpresas.Services;
using SistemaEmpresas.Services.Auth;

namespace SistemaEmpresas.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var tenantDb = scope.ServiceProvider.GetRequiredService<TenantDbContext>();
        
        var tenant = await tenantDb.Tenants.FirstOrDefaultAsync(t => t.Dominio == "irrigacao");
        if (tenant != null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(tenant.ConnectionString);
            
            using var appDb = new AppDbContext(optionsBuilder.Options);
            
            // Ensure PW~SenhaHash column exists (Self-healing for legacy DBs)
            try 
            {
                await appDb.Database.ExecuteSqlRawAsync(@"
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[PW~Usuarios]') AND name = N'PW~SenhaHash')
                    BEGIN
                        ALTER TABLE [PW~Usuarios] ADD [PW~SenhaHash] varchar(255) NULL;
                    END
                ");

                // Indexes for Dashboard Optimization
                // NOTE: Index creation causes timeout on large tables during startup
                // Create these manually via SQL Server Management Studio or direct query:
                // CREATE INDEX IX_Orcamento_DataEmissao ON [Orçamento] ([Data de Emissão]);
                // CREATE INDEX IX_Orcamento_Status ON [Orçamento] ([Venda Fechada], [Cancelado]) INCLUDE ([Data de Emissão]);
                // CREATE INDEX IX_Orcamento_DataFechamento ON [Orçamento] ([Data do Fechamento]) INCLUDE ([Venda Fechada]);
            }
            catch (Exception ex)
            {
                // Log or ignore if it fails, but it's critical for the model
                Console.WriteLine($"Erro ao verificar/criar coluna PW~SenhaHash: {ex.Message}");
            }

            // Seed Group
            var groupName = "Administradores";
            // Check if group exists (encrypted or plain? Model says PwNome is key. Usually groups are plain text in legacy systems? 
            // Let's assume plain text for group name based on AuthService logs showing group name clearly)
            if (!await appDb.PwGrupos.AnyAsync(g => g.PwNome == groupName))
            {
                appDb.PwGrupos.Add(new PwGrupo { PwNome = groupName });
                await appDb.SaveChangesAsync();
            }
            
            // Seed User
            var userName = "nicolas";
            var password = "2510";
            
            var encryptedName = VB6CryptoService.Encripta(userName);
            var encryptedPassword = VB6CryptoService.Encripta(password);
            
            // Find any user with this name (encrypted)
            var existingUsers = await appDb.PwUsuarios
                .Where(u => u.PwNome == encryptedName)
                .ToListAsync();
            
            if (existingUsers.Any())
            {
                // Remove existing users with this name to avoid conflict/ambiguity
                appDb.PwUsuarios.RemoveRange(existingUsers);
                await appDb.SaveChangesAsync();
            }

            // Create new user
            var newUser = new PwUsuario
            {
                PwNome = encryptedName,
                PwSenha = encryptedPassword,
                PwGrupo = groupName,
                PwObs = "Usuário criado automaticamente"
            };
            
            appDb.PwUsuarios.Add(newUser);
            await appDb.SaveChangesAsync();

            // ==========================================
            // Seed Permissões para Telas Novas do React
            // ==========================================
            // Estas tabelas não existem no VB6, são exclusivas do React
            // O VB6 simplesmente ignora tabelas que não conhece
            await SeedPermissoesReactAsync(appDb, groupName);
        }
    }

    /// <summary>
    /// Adiciona as permissões das telas novas do React para um grupo
    /// Usa a tabela nova PermissoesTela (não mais PwTabelas)
    /// </summary>
    private static async Task SeedPermissoesReactAsync(AppDbContext appDb, string grupo)
    {
        // Telas do sistema React (usar mesmos nomes definidos em PermissoesTelaRepository.GetTelasDisponiveis())
        var telasReact = new[]
        {
            new { Modulo = "Dashboard", Tela = "Dashboard", NomeTela = "Dashboard", Rota = "/" },
            new { Modulo = "Cadastros", Tela = "Geral", NomeTela = "Cadastro Geral", Rota = "/cadastros/geral" },
            new { Modulo = "Cadastros", Tela = "Produtos", NomeTela = "Produtos", Rota = "/cadastros/produtos" },
            new { Modulo = "Cadastros", Tela = "Emitentes", NomeTela = "Emitentes", Rota = "/emitentes" },
            new { Modulo = "Fiscal", Tela = "NotaFiscal", NomeTela = "Notas Fiscais", Rota = "/notas-fiscais" },
            new { Modulo = "Fiscal", Tela = "ClassificacaoFiscal", NomeTela = "Classificação Fiscal", Rota = "/classificacao-fiscal" },
            new { Modulo = "Fiscal", Tela = "ClassTrib", NomeTela = "Classificação Tributária (IBS/CBS)", Rota = "/classtrib" },
            new { Modulo = "Sistema", Tela = "DadosEmitente", NomeTela = "Dados do Emitente", Rota = "/emitente" },
            new { Modulo = "Sistema", Tela = "Usuarios", NomeTela = "Usuários e Permissões", Rota = "/usuarios" },
            new { Modulo = "Sistema", Tela = "Logs", NomeTela = "Logs de Auditoria", Rota = "/logs" },
        };

        foreach (var tela in telasReact)
        {
            // Verifica se já existe permissão para este grupo/tela
            var existe = await appDb.PermissoesTelas
                .AnyAsync(p => p.Grupo == grupo && p.Tela == tela.Tela);

            if (!existe)
            {
                // Cria com permissão total (Consultar, Incluir, Alterar, Excluir)
                appDb.PermissoesTelas.Add(new PermissoesTela
                {
                    Grupo = grupo,
                    Modulo = tela.Modulo,
                    Tela = tela.Tela,
                    NomeTela = tela.NomeTela,
                    Rota = tela.Rota,
                    Consultar = true,
                    Incluir = true,
                    Alterar = true,
                    Excluir = true,
                    Ordem = 0
                });
            }
        }

        await appDb.SaveChangesAsync();
    }
}
