using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEmpresas.Models;

/// <summary>
/// Modelo de Log de Auditoria para rastrear todas as ações do usuário no sistema
/// </summary>
[Table("LogsAuditoria")]
public class LogAuditoria
{
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// Data e hora da ação (UTC)
    /// </summary>
    [Required]
    public DateTime DataHora { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Código do usuário que executou a ação
    /// </summary>
    [Required]
    public int UsuarioCodigo { get; set; }

    /// <summary>
    /// Nome do usuário
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string UsuarioNome { get; set; } = string.Empty;

    /// <summary>
    /// Grupo do usuário
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string UsuarioGrupo { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de ação: LOGIN, LOGOUT, CRIAR, ALTERAR, EXCLUIR, VISUALIZAR, EXPORTAR, IMPORTAR, etc.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string TipoAcao { get; set; } = string.Empty;

    /// <summary>
    /// Módulo do sistema: Cadastros, Fiscal, Sistema, etc.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Modulo { get; set; } = string.Empty;

    /// <summary>
    /// Tela/Entidade afetada: Usuarios, NotaFiscal, Geral, etc.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Entidade { get; set; } = string.Empty;

    /// <summary>
    /// ID do registro afetado (quando aplicável)
    /// </summary>
    public string? EntidadeId { get; set; }

    /// <summary>
    /// Descrição legível da ação
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Dados anteriores (JSON) - para alterações
    /// </summary>
    public string? DadosAnteriores { get; set; }

    /// <summary>
    /// Dados novos (JSON) - para criações e alterações
    /// </summary>
    public string? DadosNovos { get; set; }

    /// <summary>
    /// Campos que foram alterados (separados por vírgula)
    /// </summary>
    [MaxLength(1000)]
    public string? CamposAlterados { get; set; }

    /// <summary>
    /// Endereço IP do usuário
    /// </summary>
    [MaxLength(50)]
    public string? EnderecoIP { get; set; }

    /// <summary>
    /// User Agent do navegador
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Método HTTP: GET, POST, PUT, DELETE
    /// </summary>
    [MaxLength(10)]
    public string? MetodoHttp { get; set; }

    /// <summary>
    /// URL da requisição
    /// </summary>
    [MaxLength(500)]
    public string? UrlRequisicao { get; set; }

    /// <summary>
    /// Código de status HTTP da resposta
    /// </summary>
    public int? StatusCode { get; set; }

    /// <summary>
    /// Tempo de execução em milissegundos
    /// </summary>
    public long? TempoExecucaoMs { get; set; }

    /// <summary>
    /// Se houve erro na operação
    /// </summary>
    public bool Erro { get; set; } = false;

    /// <summary>
    /// Mensagem de erro (se houver)
    /// </summary>
    [MaxLength(2000)]
    public string? MensagemErro { get; set; }

    /// <summary>
    /// ID do Tenant (empresa)
    /// </summary>
    public int? TenantId { get; set; }

    /// <summary>
    /// Nome do Tenant
    /// </summary>
    [MaxLength(100)]
    public string? TenantNome { get; set; }

    /// <summary>
    /// Identificador único da sessão
    /// </summary>
    [MaxLength(100)]
    public string? SessaoId { get; set; }

    /// <summary>
    /// Correlation ID para rastrear requisições relacionadas
    /// </summary>
    [MaxLength(50)]
    public string? CorrelationId { get; set; }
}

/// <summary>
/// Tipos de ação para auditoria
/// </summary>
public static class TipoAcaoAuditoria
{
    public const string LOGIN = "LOGIN";
    public const string LOGOUT = "LOGOUT";
    public const string LOGIN_FALHA = "LOGIN_FALHA";
    public const string CRIAR = "CRIAR";
    public const string ALTERAR = "ALTERAR";
    public const string EXCLUIR = "EXCLUIR";
    public const string VISUALIZAR = "VISUALIZAR";
    public const string LISTAR = "LISTAR";
    public const string EXPORTAR = "EXPORTAR";
    public const string IMPORTAR = "IMPORTAR";
    public const string ATIVAR = "ATIVAR";
    public const string INATIVAR = "INATIVAR";
    public const string AUTORIZAR = "AUTORIZAR";
    public const string APROVAR = "APROVAR";
    public const string REJEITAR = "REJEITAR";
    public const string CANCELAR = "CANCELAR";
    public const string ENVIAR = "ENVIAR";
    public const string ALTERAR_SENHA = "ALTERAR_SENHA";
    public const string RESET_SENHA = "RESET_SENHA";
    public const string ALTERAR_PERMISSAO = "ALTERAR_PERMISSAO";
    public const string ERRO = "ERRO";
    public const string OUTRO = "OUTRO";
}
