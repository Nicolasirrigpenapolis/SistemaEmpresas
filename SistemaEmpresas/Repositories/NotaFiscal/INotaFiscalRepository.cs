using SistemaEmpresas.DTOs;
using NotaFiscalModel = SistemaEmpresas.Models.NotaFiscal;

namespace SistemaEmpresas.Repositories.NotaFiscal;

public interface INotaFiscalRepository
{
    /// <summary>
    /// Lista notas fiscais com paginação e filtros
    /// </summary>
    Task<PagedResult<NotaFiscalListDto>> ListarAsync(NotaFiscalFiltroDto filtro);
    
    /// <summary>
    /// Obtém uma nota fiscal por ID com todos os detalhes
    /// </summary>
    Task<NotaFiscalDto?> ObterPorIdAsync(int id);
    
    /// <summary>
    /// Cria uma nova nota fiscal
    /// </summary>
    Task<NotaFiscalModel> CriarAsync(NotaFiscalCreateUpdateDto dto, string usuario);
    
    /// <summary>
    /// Atualiza uma nota fiscal existente
    /// </summary>
    Task<NotaFiscalModel?> AtualizarAsync(int id, NotaFiscalCreateUpdateDto dto, string usuario);
    
    /// <summary>
    /// Cancela uma nota fiscal
    /// </summary>
    Task<bool> CancelarAsync(int id, string justificativa, string usuario);
    
    /// <summary>
    /// Obtém o próximo número da nota fiscal
    /// </summary>
    Task<int> ObterProximoNumeroAsync(short propriedade);
    
    /// <summary>
    /// Lista clientes para combo
    /// </summary>
    Task<List<ClienteComboDto>> ListarClientesComboAsync(string? busca);
    
    /// <summary>
    /// Lista transportadoras para combo
    /// </summary>
    Task<List<TransportadoraComboDto>> ListarTransportadorasComboAsync(string? busca);
    
    /// <summary>
    /// Lista naturezas de operação para combo
    /// </summary>
    Task<List<NaturezaOperacaoComboDto>> ListarNaturezasComboAsync(bool? entradaSaida = null);
    
    /// <summary>
    /// Lista propriedades/filiais para combo
    /// </summary>
    Task<List<PropriedadeComboDto>> ListarPropriedadesComboAsync();

    /// <summary>
    /// Lista propriedades vinculadas a um cliente específico
    /// </summary>
    Task<List<PropriedadeComboDto>> ListarPropriedadesPorClienteAsync(int sequenciaDoGeral);
    
    /// <summary>
    /// Lista tipos de cobrança para combo
    /// </summary>
    Task<List<TipoCobrancaComboDto>> ListarTiposCobrancaComboAsync();
    
    /// <summary>
    /// Lista vendedores para combo
    /// </summary>
    Task<List<VendedorComboDto>> ListarVendedoresComboAsync(string? busca);

    /// <summary>
    /// Lista produtos para combo/autocomplete
    /// </summary>
    Task<List<ProdutoComboDto>> ListarProdutosComboAsync(string? busca);
    
    /// <summary>
    /// Verifica se nota fiscal pode ser editada
    /// </summary>
    Task<bool> PodeEditarAsync(int id);
    
    /// <summary>
    /// Obtém dados do cliente por ID
    /// </summary>
    Task<ClienteComboDto?> ObterClienteAsync(int id);
    
    /// <summary>
    /// Obtém dados da transportadora por ID
    /// </summary>
    Task<TransportadoraComboDto?> ObterTransportadoraAsync(int id);
    
    #region Itens da Nota Fiscal
    
    /// <summary>
    /// Lista produtos da nota fiscal
    /// </summary>
    Task<List<ProdutoDaNotaFiscalDto>> ListarProdutosAsync(int notaFiscalId);
    
    /// <summary>
    /// Lista conjuntos da nota fiscal
    /// </summary>
    Task<List<ConjuntoDaNotaFiscalDto>> ListarConjuntosAsync(int notaFiscalId);
    
    /// <summary>
    /// Lista peças da nota fiscal
    /// </summary>
    Task<List<PecaDaNotaFiscalDto>> ListarPecasAsync(int notaFiscalId);
    
    /// <summary>
    /// Lista parcelas da nota fiscal
    /// </summary>
    Task<List<ParcelaNotaFiscalDto>> ListarParcelasAsync(int notaFiscalId);
    
    /// <summary>
    /// Adiciona produto à nota fiscal
    /// </summary>
    Task<ProdutoDaNotaFiscalDto> AdicionarProdutoAsync(int notaFiscalId, ProdutoDaNotaFiscalCreateDto dto);
    
    /// <summary>
    /// Atualiza produto da nota fiscal
    /// </summary>
    Task<ProdutoDaNotaFiscalDto?> AtualizarProdutoAsync(int notaFiscalId, int sequenciaProduto, ProdutoDaNotaFiscalCreateDto dto);
    
    /// <summary>
    /// Remove produto da nota fiscal
    /// </summary>
    Task<bool> RemoverProdutoAsync(int notaFiscalId, int sequenciaProduto);
    
    /// <summary>
    /// Adiciona conjunto à nota fiscal
    /// </summary>
    Task<ConjuntoDaNotaFiscalDto> AdicionarConjuntoAsync(int notaFiscalId, ConjuntoDaNotaFiscalCreateDto dto);
    
    /// <summary>
    /// Atualiza conjunto da nota fiscal
    /// </summary>
    Task<ConjuntoDaNotaFiscalDto?> AtualizarConjuntoAsync(int notaFiscalId, int sequenciaConjunto, ConjuntoDaNotaFiscalCreateDto dto);
    
    /// <summary>
    /// Remove conjunto da nota fiscal
    /// </summary>
    Task<bool> RemoverConjuntoAsync(int notaFiscalId, int sequenciaConjunto);
    
    /// <summary>
    /// Adiciona peça à nota fiscal
    /// </summary>
    Task<PecaDaNotaFiscalDto> AdicionarPecaAsync(int notaFiscalId, PecaDaNotaFiscalCreateDto dto);
    
    /// <summary>
    /// Atualiza peça da nota fiscal
    /// </summary>
    Task<PecaDaNotaFiscalDto?> AtualizarPecaAsync(int notaFiscalId, int sequenciaPeca, PecaDaNotaFiscalCreateDto dto);
    
    /// <summary>
    /// Remove peça da nota fiscal
    /// </summary>
    Task<bool> RemoverPecaAsync(int notaFiscalId, int sequenciaPeca);
    
    /// <summary>
    /// Adiciona parcela à nota fiscal
    /// </summary>
    Task<ParcelaNotaFiscalDto> AdicionarParcelaAsync(int notaFiscalId, ParcelaNotaFiscalCreateDto dto);
    
    /// <summary>
    /// Atualiza parcela da nota fiscal
    /// </summary>
    Task<ParcelaNotaFiscalDto?> AtualizarParcelaAsync(int notaFiscalId, short numeroParcela, ParcelaNotaFiscalCreateDto dto);
    
    /// <summary>
    /// Remove parcela da nota fiscal
    /// </summary>
    Task<bool> RemoverParcelaAsync(int notaFiscalId, short numeroParcela);
    
    /// <summary>
    /// Recalcula totais da nota fiscal
    /// </summary>
    Task RecalcularTotaisAsync(int notaFiscalId);

    /// <summary>
    /// Duplica uma nota fiscal existente com novo número
    /// Copia todos os dados exceto: autorização, transmissão, chaves NFe e XMLs
    /// </summary>
    Task<NotaFiscalModel> DuplicarAsync(int notaFiscalId, string usuario);
    
    #endregion
}
