using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EcoScale.src.Models.Abstract
{
    [Table("planilha")]
    public class Planilha
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("planilha_id")]
        public int Id { get; set; }

        [Column("boo_excluido")]
        public bool Removido { get; set; } = false;

        [Column("criterio_id"), ForeignKey("planilha_id")]
        public required ICollection<AreaPlanilha> Areas { get; set; }
    }

    [Table("area_planilha")]
    public class AreaPlanilha
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("area_planilha_id")]
        public int Id { get; set; }

        [Column("nom_area")]
        public required string Nome { get; set; }

        [Column("planilha_id"), ForeignKey("area_planilha_id")]
        public required ICollection<TemaPlanilha> Temas { get; set; }
    }

    [Table("tema_planilha")]
    public  class TemaPlanilha
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("tema_planilha_id")]
        public int Id { get; set; }

        [Column("nom_tema")]
        public required string Nome { get; set; }

        [Column("criterio"), ForeignKey("tema_planilha_id")]
        public required ICollection<CriterioPlanilha> Criterios { get; set; }
    }

    [Table("criterio_planilha")]
    public class CriterioPlanilha
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("criterio_planilha_id")]
        public int Id { get; set; }

        [Column("nom_criterio")]
        public required string Nome { get; set; }

        [Column("item_id"), ForeignKey("criterio_planilha_id")]
        public required List<ItemAvaliadoPlanilha> Itens { get; set; }
    }

    [Table("item_avaliado_planilha")]
    public class ItemAvaliadoPlanilha
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("item_avaliado_planilha_id")]
        public int Id { get; set; }

        [Column("dsc_item")]
        public required string Descricao { get; set; }
    }
}