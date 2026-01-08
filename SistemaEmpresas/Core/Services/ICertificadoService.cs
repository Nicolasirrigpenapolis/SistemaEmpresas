using System.Security.Cryptography.X509Certificates;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Core.Services;

/// <summary>
/// Interface para serviços de gerenciamento de certificados digitais
/// </summary>
public interface ICertificadoService
{
    /// <summary>
    /// Carrega o certificado digital de um emitente
    /// </summary>
    /// <param name="emitente">Emitente com dados do certificado</param>
    /// <returns>Certificado X509 carregado ou null se não houver certificado</returns>
    X509Certificate2? CarregarCertificado(Emitente emitente);

    /// <summary>
    /// Carrega o certificado digital por ID do emitente
    /// </summary>
    /// <param name="emitenteId">ID do emitente</param>
    /// <returns>Certificado X509 carregado ou null se não encontrado</returns>
    Task<X509Certificate2?> CarregarCertificadoPorEmitenteIdAsync(int emitenteId);

    /// <summary>
    /// Valida se o certificado é válido e não está expirado
    /// </summary>
    /// <param name="certificado">Certificado a ser validado</param>
    /// <returns>Tupla com (isValido, mensagem)</returns>
    (bool IsValido, string Mensagem) ValidarCertificado(X509Certificate2 certificado);

    /// <summary>
    /// Obtém informações detalhadas do certificado
    /// </summary>
    /// <param name="certificado">Certificado a ser analisado</param>
    /// <returns>Objeto com informações do certificado</returns>
    CertificadoInfo ObterInformacoesCertificado(X509Certificate2 certificado);

    /// <summary>
    /// Salva um certificado no sistema de arquivos de forma segura
    /// </summary>
    /// <param name="emitenteId">ID do emitente</param>
    /// <param name="certificadoBytes">Bytes do arquivo .pfx</param>
    /// <param name="senha">Senha do certificado</param>
    /// <returns>Caminho onde o certificado foi salvo</returns>
    Task<string> SalvarCertificadoAsync(int emitenteId, byte[] certificadoBytes, string senha);

    /// <summary>
    /// Remove o certificado do sistema de arquivos
    /// </summary>
    /// <param name="caminhoCertificado">Caminho do certificado a ser removido</param>
    void RemoverCertificado(string caminhoCertificado);

    /// <summary>
    /// Valida um certificado a partir de bytes e senha
    /// </summary>
    /// <param name="certificadoBytes">Bytes do arquivo .pfx</param>
    /// <param name="senha">Senha do certificado</param>
    /// <returns>Tupla com (certificado carregado ou null, mensagem de erro)</returns>
    (X509Certificate2? Certificado, string? MensagemErro) ValidarCertificadoUpload(byte[] certificadoBytes, string senha);
}

/// <summary>
/// Informações sobre um certificado digital
/// </summary>
public class CertificadoInfo
{
    public string Subject { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public DateTime ValidoApartirDe { get; set; }
    public DateTime ValidoAte { get; set; }
    public bool EstaValido { get; set; }
    public bool EstaProximoDoVencimento { get; set; } // Menos de 30 dias
    public int DiasRestantes { get; set; }
    public string Thumbprint { get; set; } = null!;
    public string SerialNumber { get; set; } = null!;
    public bool PossuiChavePrivada { get; set; }
}
