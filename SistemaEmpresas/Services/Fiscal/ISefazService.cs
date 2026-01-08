using SistemaEmpresas.DTOs.Fiscal;

namespace SistemaEmpresas.Services.Fiscal;

public interface ISefazService
{
    /// <summary>
    /// Busca o XML de uma NFe diretamente da SEFAZ usando a chave de acesso
    /// </summary>
    /// <param name="chaveAcesso">Chave de acesso da NFe (44 dígitos)</param>
    /// <returns>Stream com o conteúdo do XML ou null se não encontrado</returns>
    Task<Stream?> BuscarNFePorChaveAsync(string chaveAcesso);
}
