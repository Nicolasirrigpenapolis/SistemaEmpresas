using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaEmpresas.Models;

[PrimaryKey("SequenciaDaProducao", "SequenciaDoItem")]
[Table("Itens da Produção")]
public partial class ItenDaProducao
{
    [Key]
    [Column("Sequencia da Produção")]
    public int SequenciaDaProducao { get; set; }

    [Key]
    [Column("Sequencia do Item")]
    public int SequenciaDoItem { get; set; }

    [Column("Seqüência do Produto")]
    public int SequenciaDoProduto { get; set; }

    [Column("Seqüência da Matéria Prima")]
    public int SequenciaDaMateriaPrima { get; set; }

    [Column("Seqüência do Conjunto")]
    public int SequenciaDoConjunto { get; set; }

    [Column(TypeName = "decimal(11, 4)")]
    public decimal Quantidade { get; set; }

    [Column("Data da Produção", TypeName = "datetime")]
    public DateTime? DataDaProducao { get; set; }

    [Column("Nao calcula")]
    public bool NaoCalcula { get; set; }

    [Column("Ja produziu")]
    public bool JaProduziu { get; set; }

    [Column("Dt final", TypeName = "datetime")]
    public DateTime? DtFinal { get; set; }

    [Column("Ini serra", TypeName = "datetime")]
    public DateTime? IniSerra { get; set; }

    [Column("Fim serra", TypeName = "datetime")]
    public DateTime? FimSerra { get; set; }

    [Column("Hora ini serra", TypeName = "datetime")]
    public DateTime? HoraIniSerra { get; set; }

    [Column("Hora fim serra", TypeName = "datetime")]
    public DateTime? HoraFimSerra { get; set; }

    [Column("Data inicial oxicorte", TypeName = "datetime")]
    public DateTime? DataInicialOxicorte { get; set; }

    [Column("Hora ini oxi", TypeName = "datetime")]
    public DateTime? HoraIniOxi { get; set; }

    [Column("Data fim oxicorte", TypeName = "datetime")]
    public DateTime? DataFimOxicorte { get; set; }

    [Column("Hora fim oxi", TypeName = "datetime")]
    public DateTime? HoraFimOxi { get; set; }

    [Column("Dt ini guilhotina", TypeName = "datetime")]
    public DateTime? DtIniGuilhotina { get; set; }

    [Column("Hora ini gui", TypeName = "datetime")]
    public DateTime? HoraIniGui { get; set; }

    [Column("Hora fim gui", TypeName = "datetime")]
    public DateTime? HoraFimGui { get; set; }

    [Column("Dt fim gui", TypeName = "datetime")]
    public DateTime? DtFimGui { get; set; }

    [Column("Operador serra")]
    [StringLength(20)]
    [Unicode(false)]
    public string OperadorSerra { get; set; } = null!;

    [Column("Operador oxi")]
    [StringLength(20)]
    [Unicode(false)]
    public string OperadorOxi { get; set; } = null!;

    [Column("Operador gui")]
    [StringLength(20)]
    [Unicode(false)]
    public string OperadorGui { get; set; } = null!;

    [Column("Operador dobra")]
    [StringLength(20)]
    [Unicode(false)]
    public string OperadorDobra { get; set; } = null!;

    [Column("Operador calandra")]
    [StringLength(20)]
    [Unicode(false)]
    public string OperadorCalandra { get; set; } = null!;

    [Column("Operador perfiladeira")]
    [StringLength(20)]
    [Unicode(false)]
    public string OperadorPerfiladeira { get; set; } = null!;

    [Column("Opeardor torno")]
    [StringLength(20)]
    [Unicode(false)]
    public string OpeardorTorno { get; set; } = null!;
}
