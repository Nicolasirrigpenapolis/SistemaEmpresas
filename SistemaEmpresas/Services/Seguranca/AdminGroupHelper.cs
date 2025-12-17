using System.Linq;

namespace SistemaEmpresas.Services.Seguranca;

/// <summary>
/// Utilitarios para tratar o grupo administrador legado.
/// Apenas "PROGRAMADOR" eh considerado administrador.
/// </summary>
public static class AdminGroupHelper
{
    public const string GrupoProgramador = "PROGRAMADOR";

    private static readonly string[] AdminGroups = new[]
    {
        GrupoProgramador
    };

    public static bool IsAdminGroup(string? groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
        {
            return false;
        }

        var normalized = groupName.Trim();
        return AdminGroups.Any(g => g.Equals(normalized, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Retorna o nome canonical (PROGRAMADOR) para admin ou o valor original quando nao admin.
    /// </summary>
    public static string Normalize(string? groupName)
    {
        if (IsAdminGroup(groupName))
        {
            return GrupoProgramador;
        }

        return (groupName ?? string.Empty).Trim();
    }
}
