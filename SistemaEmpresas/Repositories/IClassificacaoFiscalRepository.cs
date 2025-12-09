using SistemaEmpresas.Models;

namespace SistemaEmpresas.Repositories;

/// <summary>
/// Interface do Repositório de Classificação Fiscal
/// Define os métodos de acesso a dados para Classificação Fiscal
/// 
/// ARQUITETURA:
/// - ClassificacaoFiscal contém dados VB6 (NCM, IPI, CEST, etc.)
/// - ClassTrib contém dados SVRS (CST, percentuais IBS/CBS, etc.)
/// - Associação via FK ClassTribId + navigation property ClassTribNavigation
/// </summary>
public interface IClassificacaoFiscalRepository
{
    #region Métodos CRUD Básicos

    /// <summary>
    /// Busca uma classificação fiscal por ID (Sequência)
    /// Inclui ClassTrib associado via Include
    /// </summary>
    Task<ClassificacaoFiscal?> GetByIdAsync(short id);

    /// <summary>
    /// Busca uma classificação fiscal por NCM
    /// Inclui ClassTrib associado via Include
    /// Compatibilidade VB6
    /// </summary>
    Task<ClassificacaoFiscal?> GetByNcmAsync(string ncm);

    /// <summary>
    /// Busca classificações fiscais associadas a um ClassTribId específico
    /// </summary>
    Task<List<ClassificacaoFiscal>> GetByClassTribIdAsync(int classTribId);

    /// <summary>
    /// Lista todas as classificações fiscais
    /// Inclui ClassTrib associado via Include
    /// </summary>
    /// <param name="incluirInativos">Se true, inclui registros inativos</param>
    Task<List<ClassificacaoFiscal>> GetAllAsync(bool incluirInativos = false);

    /// <summary>
    /// Lista todas as classificações fiscais válidas para NFe
    /// (ClassTrib.ValidoParaNFe = true)
    /// MÉTODO PRINCIPAL - Foco do sistema
    /// </summary>
    Task<List<ClassificacaoFiscal>> GetClassificacoesNFeAsync();

    /// <summary>
    /// Lista classificações fiscais com paginação
    /// Inclui ClassTrib associado via Include
    /// </summary>
    /// <param name="filtroTributacao">null=todos, "vinculados"=com ClassTrib, "pendentes"=sem ClassTrib</param>
    Task<(List<ClassificacaoFiscal> items, int total)> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 50,
        string? filtroNcm = null,
        string? filtroDescricao = null,
        bool? somenteNFe = null,
        bool incluirInativos = false,
        string? filtroTributacao = null);

    /// <summary>
    /// Cria uma nova classificação fiscal
    /// </summary>
    Task<ClassificacaoFiscal> CreateAsync(ClassificacaoFiscal classificacao);

    /// <summary>
    /// Atualiza uma classificação fiscal existente
    /// </summary>
    Task<ClassificacaoFiscal> UpdateAsync(ClassificacaoFiscal classificacao);

    /// <summary>
    /// Exclui uma classificação fiscal (soft delete via Inativo)
    /// </summary>
    Task<bool> DeleteAsync(short id);

    #endregion

    #region Métodos de Associação ClassTrib

    /// <summary>
    /// Associa uma classificação fiscal a um ClassTrib
    /// </summary>
    /// <param name="classificacaoId">ID da classificação fiscal</param>
    /// <param name="classTribId">ID do ClassTrib (null para desassociar)</param>
    Task<ClassificacaoFiscal?> AssociarClassTribAsync(short classificacaoId, int? classTribId);

    /// <summary>
    /// Verifica se existe alguma classificação associada a um ClassTrib
    /// </summary>
    Task<bool> ExistsClassificacaoComClassTribAsync(int classTribId);

    #endregion

    #region Métodos de Pesquisa

    /// <summary>
    /// Pesquisa classificações por NCM parcial ou descrição
    /// Inclui ClassTrib associado via Include
    /// </summary>
    Task<List<ClassificacaoFiscal>> SearchAsync(string termo);

    /// <summary>
    /// Busca classificações fiscais por CST (via ClassTrib associado)
    /// </summary>
    Task<List<ClassificacaoFiscal>> GetByCstAsync(string cst);

    #endregion
}
