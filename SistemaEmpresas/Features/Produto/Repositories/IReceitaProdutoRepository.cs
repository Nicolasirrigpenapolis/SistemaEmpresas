using SistemaEmpresas.Features.Produto.Dtos;

namespace SistemaEmpresas.Features.Produto.Repositories;

/// <summary>
/// Interface do repositorio de receita do produto (materias primas)
/// </summary>
public interface IReceitaProdutoRepository
{
    /// <summary>
    /// Obtem a receita completa de um produto
    /// </summary>
    /// <param name="sequenciaDoProduto">Sequencia do produto principal</param>
    /// <returns>Receita do produto com todos os itens</returns>
    Task<ReceitaProdutoDto> ObterReceitaAsync(int sequenciaDoProduto);

    /// <summary>
    /// Lista os itens da receita de um produto
    /// </summary>
    /// <param name="sequenciaDoProduto">Sequencia do produto principal</param>
    /// <returns>Lista de itens da receita</returns>
    Task<List<ReceitaProdutoListDto>> ListarItensAsync(int sequenciaDoProduto);

    /// <summary>
    /// Adiciona um item a receita do produto
    /// </summary>
    /// <param name="sequenciaDoProduto">Sequencia do produto principal</param>
    /// <param name="dto">Dados do item a adicionar</param>
    /// <returns>Item adicionado</returns>
    Task<ReceitaProdutoListDto> AdicionarItemAsync(int sequenciaDoProduto, ReceitaProdutoCreateUpdateDto dto);

    /// <summary>
    /// Atualiza a quantidade de um item da receita
    /// </summary>
    /// <param name="sequenciaDoProduto">Sequencia do produto principal</param>
    /// <param name="sequenciaDaMateriaPrima">Sequencia da materia prima</param>
    /// <param name="dto">Dados atualizados</param>
    /// <returns>Item atualizado</returns>
    Task<ReceitaProdutoListDto?> AtualizarItemAsync(int sequenciaDoProduto, int sequenciaDaMateriaPrima, ReceitaProdutoCreateUpdateDto dto);

    /// <summary>
    /// Remove um item da receita
    /// </summary>
    /// <param name="sequenciaDoProduto">Sequencia do produto principal</param>
    /// <param name="sequenciaDaMateriaPrima">Sequencia da materia prima a remover</param>
    /// <returns>True se removido, false se nao encontrado</returns>
    Task<bool> RemoverItemAsync(int sequenciaDoProduto, int sequenciaDaMateriaPrima);

    /// <summary>
    /// Remove todos os itens da receita de um produto
    /// </summary>
    /// <param name="sequenciaDoProduto">Sequencia do produto principal</param>
    /// <returns>Quantidade de itens removidos</returns>
    Task<int> LimparReceitaAsync(int sequenciaDoProduto);

    /// <summary>
    /// Verifica se um item ja existe na receita
    /// </summary>
    /// <param name="sequenciaDoProduto">Sequencia do produto principal</param>
    /// <param name="sequenciaDaMateriaPrima">Sequencia da materia prima</param>
    /// <returns>True se existe, false caso contrario</returns>
    Task<bool> ItemExisteAsync(int sequenciaDoProduto, int sequenciaDaMateriaPrima);
}
