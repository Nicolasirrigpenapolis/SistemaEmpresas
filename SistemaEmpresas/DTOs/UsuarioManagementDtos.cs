namespace SistemaEmpresas.DTOs;

// ============================================
// DTOs para Gerenciamento de Usuários
// Compatível com banco legado VB6
// ============================================

#region Grupos

/// <summary>
/// DTO para listagem de grupos
/// </summary>
public class GrupoListDto
{
    public string Nome { get; set; } = string.Empty;
    public int QuantidadeUsuarios { get; set; }
    public bool IsAdmin { get; set; }
}

/// <summary>
/// DTO para criação de grupo
/// </summary>
public class GrupoCreateDto
{
    public string Nome { get; set; } = string.Empty;
}

#endregion

#region Usuários

/// <summary>
/// DTO para listagem de usuários
/// </summary>
public class UsuarioListDto
{
    public string Nome { get; set; } = string.Empty;
    public string Grupo { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
    public bool IsAdmin { get; set; }
    public bool Ativo { get; set; } = true;
    public int? GrupoUsuarioId { get; set; }
}

/// <summary>
/// DTO para criação de usuário
/// </summary>
public class UsuarioCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string ConfirmarSenha { get; set; } = string.Empty;
    public string Grupo { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; } = true;
}

/// <summary>
/// DTO para edição de usuário
/// </summary>
public class UsuarioUpdateDto
{
    public string? NovaSenha { get; set; }
    public string? ConfirmarNovaSenha { get; set; }
    public string Grupo { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; } = true;
}

/// <summary>
/// DTO para mover usuário entre grupos
/// </summary>
public class MoverUsuarioDto
{
    public string NomeUsuario { get; set; } = string.Empty;
    public string GrupoOrigem { get; set; } = string.Empty;
    public string GrupoDestino { get; set; } = string.Empty;
}

#endregion

#region Permissões

/// <summary>
/// DTO para permissão de tabela/menu
/// </summary>
public class PermissaoTabelaDto
{
    public string Projeto { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string NomeExibicao { get; set; } = string.Empty;
    public bool Visualiza { get; set; } = true;
    public bool Inclui { get; set; } = true;
    public bool Modifica { get; set; } = true;
    public bool Exclui { get; set; } = true;
    
    /// <summary>
    /// Tipo: "TABELA" ou "MENU"
    /// </summary>
    public string Tipo { get; set; } = "TABELA";
    
    /// <summary>
    /// Módulo para agrupamento (ex: Comercial, Financeiro, etc.)
    /// </summary>
    public string? Modulo { get; set; }
}

/// <summary>
/// DTO para atualização de permissões em lote
/// </summary>
public class AtualizarPermissoesDto
{
    public string Grupo { get; set; } = string.Empty;
    public List<PermissaoTabelaDto> Permissoes { get; set; } = new();
}

/// <summary>
/// DTO para listagem completa de permissões de um grupo
/// </summary>
public class PermissoesGrupoDto
{
    public string Grupo { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public List<PermissaoTabelaDto> Tabelas { get; set; } = new();
    public List<PermissaoTabelaDto> Menus { get; set; } = new();
}

/// <summary>
/// DTO para copiar permissões entre grupos
/// </summary>
public class CopiarPermissoesDto
{
    public string GrupoOrigem { get; set; } = string.Empty;
    public string GrupoDestino { get; set; } = string.Empty;
}

#endregion

#region Estrutura Hierárquica

/// <summary>
/// DTO para árvore de grupos e usuários (TreeView)
/// </summary>
public class GrupoComUsuariosDto
{
    public string Nome { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool Expandido { get; set; } = true;
    public List<UsuarioListDto> Usuarios { get; set; } = new();
}

/// <summary>
/// DTO para módulo de tabelas (agrupamento)
/// </summary>
public class ModuloTabelasDto
{
    public string Nome { get; set; } = string.Empty;
    public string Icone { get; set; } = string.Empty;
    public List<PermissaoTabelaDto> Tabelas { get; set; } = new();
}

#endregion

#region Respostas

/// <summary>
/// DTO de resposta padrão para operações
/// </summary>
public class OperacaoResultDto
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public List<string>? Erros { get; set; }
    
    /// <summary>
    /// ID do registro criado/alterado (quando aplicável)
    /// </summary>
    public int? Id { get; set; }
}

#endregion
