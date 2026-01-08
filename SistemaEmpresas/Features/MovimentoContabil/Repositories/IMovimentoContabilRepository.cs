using SistemaEmpresas.Core.Dtos;
using SistemaEmpresas.Features.MovimentoContabil.Dtos;

namespace SistemaEmpresas.Features.MovimentoContabil.Repositories;

public interface IMovimentoContabilRepository
{
    /// <summary>
    /// Obtém informações de estoque de um produto
    /// </summary>
    Task<EstoqueInfoDto?> ObterEstoqueInfoAsync(int sequenciaDoProduto);
    
    /// <summary>
    /// Busca produtos para movimento contábil
    /// </summary>
    Task<List<EstoqueInfoDto>> BuscarProdutosParaMovimentoAsync(string? busca, int limite = 50);

    /// <summary>
    /// Busca conjuntos para movimento contábil
    /// </summary>
    Task<List<EstoqueInfoDto>> BuscarConjuntosParaMovimentoAsync(string? busca, int limite = 50);
    Task<List<DespesaMvtoContabilItemDto>> BuscarDespesasParaMovimentoAsync(string? busca, int limite = 50);
    
    /// <summary>
    /// Obtém informações de estoque de um conjunto
    /// </summary>
    Task<EstoqueInfoDto?> ObterEstoqueConjuntoInfoAsync(int sequenciaDoConjunto);
    
    /// <summary>
    /// Realiza ajuste de inventário para um produto
    /// </summary>
    Task<AjusteMovimentoContabilResultDto> RealizarAjusteAsync(AjusteMovimentoContabilDto dto, string usuario);
    
    /// <summary>
    /// Realiza ajuste de inventário em lote
    /// </summary>
    Task<AjusteMovimentoContabilLoteResultDto> RealizarAjusteLoteAsync(AjusteMovimentoContabilLoteDto dto, string usuario);
    
    /// <summary>
    /// Lista movimentos de estoque de um produto
    /// </summary>
    Task<PagedResult<MovimentoEstoqueDto>> ListarMovimentosAsync(MovimentoEstoqueFiltroDto filtro);
    
    /// <summary>
    /// Recalcula o estoque contábil de um produto baseado nas baixas
    /// </summary>
    Task<decimal> RecalcularEstoqueContabilAsync(int sequenciaDoProduto);
    
    /// <summary>
    /// Atualiza o estoque contábil do produto na tabela Produtos
    /// </summary>
    Task AtualizarEstoqueProdutoAsync(int sequenciaDoProduto, decimal novoEstoque);

    // Novos métodos para MovimentoContabilNovo (MVTOCONN.FRM)
    
    /// <summary>
    /// Cria um novo movimento contábil completo
    /// </summary>
    Task<MovimentoContabilNovoDto> CriarMovimentoAsync(MovimentoContabilNovoDto dto, string usuario);
    
    /// <summary>
    /// Obtém um movimento contábil pelo ID
    /// </summary>
    Task<MovimentoContabilNovoDto?> ObterMovimentoAsync(int sequenciaDoMovimento);
    
    /// <summary>
    /// Lista movimentos contábeis com filtros
    /// </summary>
    Task<PagedResult<MovimentoContabilNovoDto>> ListarMovimentosNovosAsync(MovimentoContabilFiltroDto filtro);
    
    /// <summary>
    /// Exclui um movimento contábil e reverte as baixas de estoque
    /// </summary>
    Task<bool> ExcluirMovimentoAsync(int sequenciaDoMovimento, string usuario);

    // Métodos para Produção Inteligente

    /// <summary>
    /// Verifica se é possível produzir um item e retorna os componentes faltantes
    /// </summary>
    Task<VerificacaoProducaoResultDto> VerificarViabilidadeProducaoAsync(int sequenciaDoProdutoOuConjunto, decimal quantidade, bool ehConjunto);

    /// <summary>
    /// Executa produção em cascata, produzindo automaticamente os itens intermediários necessários
    /// </summary>
    Task<ProducaoCascataResultDto> ExecutarProducaoCascataAsync(ProducaoCascataRequestDto request, string usuario);
}
