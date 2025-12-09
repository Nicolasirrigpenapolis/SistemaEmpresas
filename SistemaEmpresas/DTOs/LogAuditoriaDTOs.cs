namespace SistemaEmpresas.DTOs;

/// <summary>
/// DTO para listagem de logs
/// </summary>
public class LogAuditoriaListDto
{
    public long Id { get; set; }
    public DateTime DataHora { get; set; }
    public int UsuarioCodigo { get; set; }
    public string UsuarioNome { get; set; } = string.Empty;
    public string UsuarioGrupo { get; set; } = string.Empty;
    public string TipoAcao { get; set; } = string.Empty;
    public string Modulo { get; set; } = string.Empty;
    public string Entidade { get; set; } = string.Empty;
    public string? EntidadeId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string? EnderecoIP { get; set; }
    public bool Erro { get; set; }
    public string? TenantNome { get; set; }
    
    // Propriedades computadas para UI
    public string TipoAcaoLabel => GetTipoAcaoLabel();
    public string TipoAcaoCor => GetTipoAcaoCor();
    public string TipoAcaoIcone => GetTipoAcaoIcone();

    private string GetTipoAcaoLabel() => TipoAcao switch
    {
        "LOGIN" => "Login",
        "LOGOUT" => "Logout",
        "LOGIN_FALHA" => "Login Falhou",
        "CRIAR" => "Criação",
        "ALTERAR" => "Alteração",
        "EXCLUIR" => "Exclusão",
        "VISUALIZAR" => "Visualização",
        "LISTAR" => "Listagem",
        "EXPORTAR" => "Exportação",
        "IMPORTAR" => "Importação",
        "ATIVAR" => "Ativação",
        "INATIVAR" => "Inativação",
        "AUTORIZAR" => "Autorização",
        "APROVAR" => "Aprovação",
        "REJEITAR" => "Rejeição",
        "CANCELAR" => "Cancelamento",
        "ENVIAR" => "Envio",
        "ALTERAR_SENHA" => "Alteração de Senha",
        "RESET_SENHA" => "Reset de Senha",
        "ALTERAR_PERMISSAO" => "Alteração de Permissão",
        "ERRO" => "Erro",
        "OUTRO" => "Outro",
        _ => TipoAcao
    };

    private string GetTipoAcaoCor() => TipoAcao switch
    {
        "LOGIN" => "emerald",
        "LOGOUT" => "gray",
        "LOGIN_FALHA" => "red",
        "CRIAR" => "blue",
        "ALTERAR" => "amber",
        "EXCLUIR" => "red",
        "VISUALIZAR" => "slate",
        "LISTAR" => "slate",
        "EXPORTAR" => "violet",
        "IMPORTAR" => "indigo",
        "ATIVAR" => "emerald",
        "INATIVAR" => "orange",
        "AUTORIZAR" => "green",
        "APROVAR" => "green",
        "REJEITAR" => "red",
        "CANCELAR" => "red",
        "ENVIAR" => "cyan",
        "ALTERAR_SENHA" => "purple",
        "RESET_SENHA" => "pink",
        "ALTERAR_PERMISSAO" => "yellow",
        "ERRO" => "red",
        "OUTRO" => "gray",
        _ => "gray"
    };

    private string GetTipoAcaoIcone() => TipoAcao switch
    {
        "LOGIN" => "LogIn",
        "LOGOUT" => "LogOut",
        "LOGIN_FALHA" => "XCircle",
        "CRIAR" => "Plus",
        "ALTERAR" => "Edit",
        "EXCLUIR" => "Trash2",
        "VISUALIZAR" => "Eye",
        "LISTAR" => "List",
        "EXPORTAR" => "Download",
        "IMPORTAR" => "Upload",
        "ATIVAR" => "CheckCircle",
        "INATIVAR" => "XCircle",
        "AUTORIZAR" => "Shield",
        "APROVAR" => "CheckCircle",
        "REJEITAR" => "XCircle",
        "CANCELAR" => "Ban",
        "ENVIAR" => "Send",
        "ALTERAR_SENHA" => "Key",
        "RESET_SENHA" => "RefreshCw",
        "ALTERAR_PERMISSAO" => "Lock",
        "ERRO" => "AlertTriangle",
        "OUTRO" => "Activity",
        _ => "Activity"
    };
}

/// <summary>
/// DTO com detalhes completos do log
/// </summary>
public class LogAuditoriaDetalheDto : LogAuditoriaListDto
{
    public string? DadosAnteriores { get; set; }
    public string? DadosNovos { get; set; }
    public string? CamposAlterados { get; set; }
    public string? UserAgent { get; set; }
    public string? MetodoHttp { get; set; }
    public string? UrlRequisicao { get; set; }
    public int? StatusCode { get; set; }
    public long? TempoExecucaoMs { get; set; }
    public string? MensagemErro { get; set; }
    public int? TenantId { get; set; }
    public string? SessaoId { get; set; }
    public string? CorrelationId { get; set; }
}

/// <summary>
/// DTO para filtros de busca de logs
/// </summary>
public class LogAuditoriaFiltroDto
{
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public int? UsuarioCodigo { get; set; }
    public string? UsuarioNome { get; set; }
    public string? TipoAcao { get; set; }
    public List<string>? TiposAcao { get; set; }
    public string? Modulo { get; set; }
    public string? Entidade { get; set; }
    public string? EntidadeId { get; set; }
    public bool? ApenasErros { get; set; }
    public string? Busca { get; set; }
    public int Pagina { get; set; } = 1;
    public int ItensPorPagina { get; set; } = 50;
    public string? OrdenarPor { get; set; }
    public bool OrdemDescrescente { get; set; } = true;
}

/// <summary>
/// DTO para criar um log de auditoria
/// </summary>
public class LogAuditoriaCreateDto
{
    public int UsuarioCodigo { get; set; }
    public string UsuarioNome { get; set; } = string.Empty;
    public string UsuarioGrupo { get; set; } = string.Empty;
    public string TipoAcao { get; set; } = string.Empty;
    public string Modulo { get; set; } = string.Empty;
    public string Entidade { get; set; } = string.Empty;
    public string? EntidadeId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string? DadosAnteriores { get; set; }
    public string? DadosNovos { get; set; }
    public string? CamposAlterados { get; set; }
    public string? EnderecoIP { get; set; }
    public string? UserAgent { get; set; }
    public string? MetodoHttp { get; set; }
    public string? UrlRequisicao { get; set; }
    public int? StatusCode { get; set; }
    public long? TempoExecucaoMs { get; set; }
    public bool Erro { get; set; }
    public string? MensagemErro { get; set; }
    public int? TenantId { get; set; }
    public string? TenantNome { get; set; }
    public string? SessaoId { get; set; }
    public string? CorrelationId { get; set; }
}

/// <summary>
/// DTO para estatísticas de logs
/// </summary>
public class LogAuditoriaEstatisticasDto
{
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int TotalAcoes { get; set; }
    public int UsuariosAtivos { get; set; }
    public int TotalErros { get; set; }
    public List<LogPorTipoDto> AcoesPorTipo { get; set; } = new();
    public List<LogPorModuloDto> AcoesPorModulo { get; set; } = new();
    public List<LogPorUsuarioDto> TopUsuarios { get; set; } = new();
    public List<LogPorDiaDto> AcoesPorDia { get; set; } = new();
}

public class LogPorTipoDto
{
    public string TipoAcao { get; set; } = string.Empty;
    public int Quantidade { get; set; }
}

public class LogPorModuloDto
{
    public string Modulo { get; set; } = string.Empty;
    public int Quantidade { get; set; }
}

public class LogPorUsuarioDto
{
    public string UsuarioNome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
}

public class LogPorDiaDto
{
    public DateTime Data { get; set; }
    public int Quantidade { get; set; }
}

/// <summary>
/// Resultado paginado de logs
/// </summary>
public class LogAuditoriaPagedResult
{
    public List<LogAuditoriaListDto> Items { get; set; } = new();
    public int TotalItems { get; set; }
    public int PaginaAtual { get; set; }
    public int ItensPorPagina { get; set; }
    public int TotalPaginas { get; set; }
}
