namespace SistemaEmpresas.DTOs;

// ============================================
// DTOs para Sistema de Permissões por Tela
// Gerenciamento granular de permissões CRUD
// ============================================

#region Templates de Permissões

/// <summary>
/// DTO para listagem de templates
/// </summary>
public class PermissoesTemplateListDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool IsPadrao { get; set; }
    public DateTime DataCriacao { get; set; }
    public int QuantidadeTelas { get; set; }
}

/// <summary>
/// DTO para criação/edição de template
/// </summary>
public class PermissoesTemplateCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public List<PermissoesTemplateDetalheDto> Detalhes { get; set; } = new();
}

/// <summary>
/// DTO para atualização de template
/// </summary>
public class PermissoesTemplateUpdateDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public List<PermissoesTemplateDetalheDto> Detalhes { get; set; } = new();
}

/// <summary>
/// DTO para detalhes de permissão do template
/// </summary>
public class PermissoesTemplateDetalheDto
{
    public int? Id { get; set; }
    public string Modulo { get; set; } = string.Empty;
    public string Tela { get; set; } = string.Empty;
    public bool Consultar { get; set; }
    public bool Incluir { get; set; }
    public bool Alterar { get; set; }
    public bool Excluir { get; set; }
}

/// <summary>
/// DTO completo do template com detalhes
/// </summary>
public class PermissoesTemplateComDetalhesDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool IsPadrao { get; set; }
    public DateTime DataCriacao { get; set; }
    public List<PermissoesTemplateDetalheDto> Detalhes { get; set; } = new();
}

#endregion

#region Permissões por Tela

/// <summary>
/// DTO para listagem de permissões de tela
/// </summary>
public class PermissoesTelaListDto
{
    public int Id { get; set; }
    public string Grupo { get; set; } = string.Empty;
    public string Modulo { get; set; } = string.Empty;
    public string Tela { get; set; } = string.Empty;
    public string NomeTela { get; set; } = string.Empty;
    public string Rota { get; set; } = string.Empty;
    public bool Consultar { get; set; }
    public bool Incluir { get; set; }
    public bool Alterar { get; set; }
    public bool Excluir { get; set; }
    public int Ordem { get; set; }
}

/// <summary>
/// DTO para criação/atualização de permissão de tela
/// </summary>
public class PermissoesTelaCreateUpdateDto
{
    public string Grupo { get; set; } = string.Empty;
    public string Modulo { get; set; } = string.Empty;
    public string Tela { get; set; } = string.Empty;
    public string NomeTela { get; set; } = string.Empty;
    public string Rota { get; set; } = string.Empty;
    public bool Consultar { get; set; }
    public bool Incluir { get; set; }
    public bool Alterar { get; set; }
    public bool Excluir { get; set; }
    public int Ordem { get; set; }
}

/// <summary>
/// DTO para atualização em lote de permissões
/// </summary>
public class PermissoesTelasBatchUpdateDto
{
    public string Grupo { get; set; } = string.Empty;
    public List<PermissoesTelaCreateUpdateDto> Permissoes { get; set; } = new();
}

/// <summary>
/// DTO para aplicar template a um grupo
/// </summary>
public class AplicarTemplateDto
{
    public int TemplateId { get; set; }
    public string Grupo { get; set; } = string.Empty;
    /// <summary>
    /// Se true, substitui todas as permissões existentes. Se false, apenas adiciona/atualiza.
    /// </summary>
    public bool SubstituirExistentes { get; set; } = true;
}

#endregion

#region Agrupamentos e Visualização

/// <summary>
/// DTO para módulo com suas telas e permissões
/// </summary>
public class ModuloPermissoesDto
{
    public string Nome { get; set; } = string.Empty;
    public string Icone { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public List<PermissoesTelaListDto> Telas { get; set; } = new();
}

/// <summary>
/// DTO para permissões completas de um grupo (agrupado por módulo)
/// </summary>
public class PermissoesCompletasGrupoDto
{
    public string Grupo { get; set; } = string.Empty;
    public string NomeGrupo { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public List<ModuloPermissoesDto> Modulos { get; set; } = new();
}

/// <summary>
/// DTO para definição de tela disponível no sistema
/// </summary>
public class TelaDisponivelDto
{
    public string Modulo { get; set; } = string.Empty;
    public string Tela { get; set; } = string.Empty;
    public string NomeTela { get; set; } = string.Empty;
    public string Rota { get; set; } = string.Empty;
    public string Icone { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public bool RequirePermissao { get; set; } = true;
}

/// <summary>
/// DTO para módulo com telas disponíveis
/// </summary>
public class ModuloComTelasDto
{
    public string Nome { get; set; } = string.Empty;
    public string Icone { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public List<TelaDisponivelDto> Telas { get; set; } = new();
}

#endregion

#region Consultas de Permissão

/// <summary>
/// DTO para verificar permissão de uma tela específica
/// </summary>
public class VerificarPermissaoDto
{
    public string Tela { get; set; } = string.Empty;
    public string Acao { get; set; } = string.Empty; // "consultar", "incluir", "alterar", "excluir"
}

/// <summary>
/// DTO de resposta para verificação de permissão
/// </summary>
public class PermissaoResultDto
{
    public bool Permitido { get; set; }
    public string Tela { get; set; } = string.Empty;
    public string Acao { get; set; } = string.Empty;
    public string? Mensagem { get; set; }
}

/// <summary>
/// DTO com todas as permissões do usuário logado
/// </summary>
public class PermissoesUsuarioLogadoDto
{
    public string Usuario { get; set; } = string.Empty;
    public string Grupo { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public Dictionary<string, PermissaoTelaResumoDto> Telas { get; set; } = new();
}

/// <summary>
/// DTO resumido de permissão (para carregamento rápido)
/// </summary>
public class PermissaoTelaResumoDto
{
    public bool C { get; set; } // Consultar
    public bool I { get; set; } // Incluir
    public bool A { get; set; } // Alterar
    public bool E { get; set; } // Excluir
}

#endregion

#region Estatísticas

/// <summary>
/// DTO para estatísticas de permissões
/// </summary>
public class PermissoesEstatisticasDto
{
    public int TotalGrupos { get; set; }
    public int TotalUsuarios { get; set; }
    public int TotalUsuariosAtivos { get; set; }
    public int TotalUsuariosInativos { get; set; }
    public int TotalTemplates { get; set; }
    public int TotalTelasConfiguradas { get; set; }
}

#endregion
