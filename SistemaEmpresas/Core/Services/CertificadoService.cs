using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Data;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Core.Services;

/// <summary>
/// Serviço para gerenciamento de certificados digitais
/// </summary>
public class CertificadoService : ICertificadoService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CertificadoService> _logger;
    private readonly IWebHostEnvironment _environment;
    private const string PASTA_CERTIFICADOS = "certificados";

    public CertificadoService(
        AppDbContext context,
        ILogger<CertificadoService> logger,
        IWebHostEnvironment environment)
    {
        _context = context;
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Carrega o certificado digital de um emitente
    /// </summary>
    public X509Certificate2? CarregarCertificado(Emitente emitente)
    {
        if (string.IsNullOrWhiteSpace(emitente.CaminhoCertificado))
        {
            _logger.LogWarning("Emitente {EmitenteId} não possui caminho de certificado configurado", emitente.Id);
            return null;
        }

        if (!File.Exists(emitente.CaminhoCertificado))
        {
            _logger.LogError("Certificado não encontrado no caminho: {Caminho}", emitente.CaminhoCertificado);
            return null;
        }

        try
        {
            var senha = DescriptografarSenha(emitente.SenhaCertificado);
            var certificado = new X509Certificate2(
                emitente.CaminhoCertificado,
                senha,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);

            _logger.LogInformation(
                "Certificado carregado com sucesso para emitente {EmitenteId}. Subject: {Subject}, Validade: {Validade}",
                emitente.Id,
                certificado.Subject,
                certificado.NotAfter);

            return certificado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar certificado do emitente {EmitenteId}", emitente.Id);
            return null;
        }
    }

    /// <summary>
    /// Carrega o certificado digital por ID do emitente
    /// </summary>
    public async Task<X509Certificate2?> CarregarCertificadoPorEmitenteIdAsync(int emitenteId)
    {
        var emitente = await _context.Emitentes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == emitenteId);

        if (emitente == null)
        {
            _logger.LogWarning("Emitente {EmitenteId} não encontrado", emitenteId);
            return null;
        }

        return CarregarCertificado(emitente);
    }

    /// <summary>
    /// Valida se o certificado é válido e não está expirado
    /// </summary>
    public (bool IsValido, string Mensagem) ValidarCertificado(X509Certificate2 certificado)
    {
        var agora = DateTime.Now;

        if (agora < certificado.NotBefore)
        {
            return (false, $"Certificado ainda não é válido. Válido a partir de {certificado.NotBefore:dd/MM/yyyy}");
        }

        if (agora > certificado.NotAfter)
        {
            return (false, $"Certificado expirado em {certificado.NotAfter:dd/MM/yyyy}");
        }

        if (!certificado.HasPrivateKey)
        {
            return (false, "Certificado não possui chave privada");
        }

        var diasRestantes = (certificado.NotAfter - agora).Days;
        if (diasRestantes <= 30)
        {
            return (true, $"⚠️ Certificado válido, mas expira em {diasRestantes} dias ({certificado.NotAfter:dd/MM/yyyy})");
        }

        return (true, $"Certificado válido até {certificado.NotAfter:dd/MM/yyyy}");
    }

    /// <summary>
    /// Obtém informações detalhadas do certificado
    /// </summary>
    public CertificadoInfo ObterInformacoesCertificado(X509Certificate2 certificado)
    {
        var agora = DateTime.Now;
        var diasRestantes = (certificado.NotAfter - agora).Days;
        var estaValido = agora >= certificado.NotBefore && agora <= certificado.NotAfter;

        return new CertificadoInfo
        {
            Subject = certificado.Subject,
            Issuer = certificado.Issuer,
            ValidoApartirDe = certificado.NotBefore,
            ValidoAte = certificado.NotAfter,
            EstaValido = estaValido && certificado.HasPrivateKey,
            EstaProximoDoVencimento = diasRestantes <= 30 && diasRestantes >= 0,
            DiasRestantes = diasRestantes,
            Thumbprint = certificado.Thumbprint,
            SerialNumber = certificado.SerialNumber,
            PossuiChavePrivada = certificado.HasPrivateKey
        };
    }

    /// <summary>
    /// Salva um certificado no sistema de arquivos de forma segura
    /// </summary>
    public async Task<string> SalvarCertificadoAsync(int emitenteId, byte[] certificadoBytes, string senha)
    {
        // Cria a pasta de certificados se não existir
        var pastaCertificados = Path.Combine(_environment.ContentRootPath, PASTA_CERTIFICADOS);
        if (!Directory.Exists(pastaCertificados))
        {
            Directory.CreateDirectory(pastaCertificados);
            _logger.LogInformation("Pasta de certificados criada: {Pasta}", pastaCertificados);
        }

        // Nome do arquivo: emitente_{id}_{timestamp}.pfx
        var nomeArquivo = $"emitente_{emitenteId}_{DateTime.Now:yyyyMMddHHmmss}.pfx";
        var caminhoCompleto = Path.Combine(pastaCertificados, nomeArquivo);

        // Salva o arquivo
        await File.WriteAllBytesAsync(caminhoCompleto, certificadoBytes);

        _logger.LogInformation(
            "Certificado salvo com sucesso para emitente {EmitenteId} em: {Caminho}",
            emitenteId,
            caminhoCompleto);

        return caminhoCompleto;
    }

    /// <summary>
    /// Remove o certificado do sistema de arquivos
    /// </summary>
    public void RemoverCertificado(string caminhoCertificado)
    {
        if (File.Exists(caminhoCertificado))
        {
            try
            {
                File.Delete(caminhoCertificado);
                _logger.LogInformation("Certificado removido: {Caminho}", caminhoCertificado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover certificado: {Caminho}", caminhoCertificado);
            }
        }
    }

    /// <summary>
    /// Valida um certificado a partir de bytes e senha
    /// </summary>
    public (X509Certificate2? Certificado, string? MensagemErro) ValidarCertificadoUpload(byte[] certificadoBytes, string senha)
    {
        try
        {
            var certificado = new X509Certificate2(
                certificadoBytes,
                senha,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);

            // Valida o certificado
            var (isValido, mensagem) = ValidarCertificado(certificado);

            if (!isValido)
            {
                return (null, mensagem);
            }

            return (certificado, null);
        }
        catch (CryptographicException ex)
        {
            _logger.LogWarning(ex, "Erro ao validar certificado durante upload");
            return (null, "Senha do certificado incorreta ou arquivo inválido");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao validar certificado");
            return (null, "Erro ao processar o certificado");
        }
    }

    /// <summary>
    /// Criptografa a senha do certificado para armazenamento seguro
    /// </summary>
    public static string CriptografarSenha(string? senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
        {
            return string.Empty;
        }

        try
        {
            // Usa uma chave fixa para simplificar (em produção, use uma chave do appsettings ou Key Vault)
            var chave = "SistemaEmpresas2024CertKey!@#"; // 32 bytes
            var chaveBytes = Encoding.UTF8.GetBytes(chave.PadRight(32).Substring(0, 32));

            using var aes = Aes.Create();
            aes.Key = chaveBytes;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var msEncrypt = new MemoryStream();

            // Escreve o IV no início do stream
            msEncrypt.Write(aes.IV, 0, aes.IV.Length);

            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(senha);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }
        catch
        {
            return senha; // Em caso de erro, retorna a senha sem criptografia
        }
    }

    /// <summary>
    /// Descriptografa a senha do certificado
    /// </summary>
    private static string? DescriptografarSenha(string? senhaCriptografada)
    {
        if (string.IsNullOrWhiteSpace(senhaCriptografada))
        {
            return string.Empty;
        }

        try
        {
            var chave = "SistemaEmpresas2024CertKey!@#"; // Mesma chave usada na criptografia
            var chaveBytes = Encoding.UTF8.GetBytes(chave.PadRight(32).Substring(0, 32));
            var cipherTextBytes = Convert.FromBase64String(senhaCriptografada);

            using var aes = Aes.Create();
            aes.Key = chaveBytes;

            // Lê o IV do início do array
            var iv = new byte[aes.IV.Length];
            Array.Copy(cipherTextBytes, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var msDecrypt = new MemoryStream(cipherTextBytes, iv.Length, cipherTextBytes.Length - iv.Length);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }
        catch
        {
            return senhaCriptografada; // Em caso de erro, retorna a senha como está
        }
    }
}
