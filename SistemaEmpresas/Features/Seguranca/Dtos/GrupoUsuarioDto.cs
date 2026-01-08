namespace SistemaEmpresas.Features.Seguranca.Dtos;

/// <summary>
/// DTO para listagem de grupos de usuários do sistema web
/// </summary>
public class GrupoUsuarioListDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public bool Ativo { get; set; }
    public bool GrupoSistema { get; set; }
    public int QuantidadeUsuarios { get; set; }
    public DateTime DataCriacao { get; set; }
}

/// <summary>
/// DTO para criação de grupo de usuários
/// </summary>
public class GrupoUsuarioCreateDto
{
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public bool Ativo { get; set; } = true;
}

/// <summary>
/// DTO para atualização de grupo de usuários
/// </summary>
public class GrupoUsuarioUpdateDto
{
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public bool? Ativo { get; set; }
}

/// <summary>
/// DTO para listagem de usuários do sistema web
/// </summary>
public class UsuarioSistemaListDto
{
    public int Id { get; set; }
    public string Login { get; set; } = null!;
    public string NomeCompleto { get; set; } = null!;
    public string? Email { get; set; }
    public int GrupoId { get; set; }
    public string GrupoNome { get; set; } = null!;
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? UltimoLogin { get; set; }
    public bool DeveTrocarSenha { get; set; }
}

/// <summary>
/// DTO para criação de usuário do sistema web
/// </summary>
public class UsuarioSistemaCreateDto
{
    public string Login { get; set; } = null!;
    public string NomeCompleto { get; set; } = null!;
    public string? Email { get; set; }
    public string Senha { get; set; } = null!;
    public string ConfirmarSenha { get; set; } = null!;
    public int GrupoId { get; set; }
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; } = true;
}

/// <summary>
/// DTO para atualização de usuário do sistema web
/// </summary>
public class UsuarioSistemaUpdateDto
{
    public string? NomeCompleto { get; set; }
    public string? Email { get; set; }
    public int? GrupoId { get; set; }
    public string? Observacoes { get; set; }
    public bool? Ativo { get; set; }
    
    // Alteração de senha (opcional)
    public string? NovaSenha { get; set; }
    public string? ConfirmarNovaSenha { get; set; }
}

/// <summary>
/// DTO para árvore de grupos com usuários
/// </summary>
public class GrupoComUsuariosSistemaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public bool Ativo { get; set; }
    public bool GrupoSistema { get; set; }
    public List<UsuarioSistemaListDto> Usuarios { get; set; } = new();
}

/// <summary>
/// DTO para redefinição de senha
/// </summary>
public class RedefinirSenhaDto
{
    public string NovaSenha { get; set; } = null!;
    public string ConfirmarNovaSenha { get; set; } = null!;
}
