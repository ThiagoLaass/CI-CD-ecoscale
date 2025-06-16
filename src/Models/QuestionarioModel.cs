using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoScale.src.Models
{
    [Table("questionario")]
    public class Questionario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("questionario_id")]
        public int Id { get; set; }

        [Column("boo_excluido")]
        public bool Removido { get; set; } = false;

        [Column("criterio_id"), ForeignKey("questionario_id")]
        public required ICollection<Area> Areas { get; set; }

        [Column("empresa_id"), ForeignKey("empresa_id")]
        public required Empresa Empresa { get; set; }
    }

    [Table("area")]
    public class Area
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("area_id")]
        public int Id { get; set; }

        [Column("nom_area")]
        public required string Nome { get; set; }

        [Column("area_id"), ForeignKey("area_id")]
        public required ICollection<Tema> Temas { get; set; }
    }

    [Table("tema")]
    public class Tema
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("tema_id")]
        public int Id { get; set; }

        [Column("nom_tema")]
        public required string Nome { get; set; }

        [Column("criterio_id"), ForeignKey("tema_id")]
        public required ICollection<Criterio> Criterios { get; set; }
    }

    [Table("criterio")]
    public class Criterio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("criterio_id")]
        public int Id { get; set; }

        [Column("nom_criterio")]
        public required string Nome { get; set; }

        [Column("item_id"), ForeignKey("criterio_id")]
        public required List<ItemAvaliado> Itens { get; set; }
    }

    [Table("item_avaliado")]
    public class ItemAvaliado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("item_avaliado_id")]
        public int Id { get; set; }

        [Column("dsc_item")]
        public required string Descricao { get; set; }

        [Column("resposta")]
        public string? Resposta { get; set; }
    }
}