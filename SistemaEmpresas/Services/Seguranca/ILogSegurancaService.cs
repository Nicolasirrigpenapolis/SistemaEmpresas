namespace SistemaEmpresas.Services.Seguranca;

/// <summary>
/// Serviço especializado para logs de segurança (tentativas de login, alterações de senha, acessos sensíveis)
/// </summary>
public interface ILogSegurancaService
{
    /// <summary>
    /// Registra uma tentativa de login (sucesso ou falha)
    /// </summary>
    Task LogTentativaLoginAsync(
        string usuario,
        string tenant,
        bool sucesso,
        string? motivoFalha = null,
        string? ip = null);

    /// <summary>
    /// Registra uma alteração de senha
    /// </summary>
    Task LogAlteracaoSenhaAsync(
        int usuarioCodigo,
        string usuarioNome,
        string grupo,
        bool sucesso,
        string? motivoFalha = null,
        string? ip = null);

    /// <summary>
    /// Registra acesso a dados sensíveis (relatórios, exportações, etc.)
    /// </summary>
    Task LogAcessoDadosSensiveisAsync(
        string tipoAcesso,
        string descricao,
        int usuarioCodigo,
        string usuarioNome,
        string grupo,
        string? ip = null);

    /// <summary>
    /// Registra tentativa de acesso não autorizado
    /// </summary>
    Task LogAcessoNaoAutorizadoAsync(
        string recurso,
        string metodo,
        int usuarioCodigo,
        string usuarioNome,
        string grupo,
        string? ip = null);

    /// <summary>
    /// Registra logout do usuário
    /// </summary>
    Task LogLogoutAsync(
        int usuarioCodigo,
        string usuarioNome,
        string grupo,
        string? ip = null);
}
