using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[Keyless]
[Table("Parâmetros")]
public partial class Parametro
{
    [Column("Caminho Atualização")]
    [StringLength(255)]
    [Unicode(false)]
    public string CaminhoAtualizacao { get; set; } = null!;

    [Column("Caminho Atualização 2")]
    [StringLength(255)]
    [Unicode(false)]
    public string CaminhoAtualizacao2 { get; set; } = null!;

    [Column("Nome do Servidor")]
    [StringLength(30)]
    [Unicode(false)]
    public string NomeDoServidor { get; set; } = null!;

    [Column("Diretorio das Fotos")]
    [StringLength(255)]
    [Unicode(false)]
    public string DiretorioDasFotos { get; set; } = null!;

    [Column("Diretorio Fotos Conjuntos")]
    [StringLength(255)]
    [Unicode(false)]
    public string DiretorioFotosConjuntos { get; set; } = null!;

    [Column("Diretorio Fotos Produtos")]
    [StringLength(255)]
    [Unicode(false)]
    public string DiretorioFotosProdutos { get; set; } = null!;

    [Column("Diretorio Desenho Tec")]
    [StringLength(255)]
    [Unicode(false)]
    public string DiretorioDesenhoTec { get; set; } = null!;
}
