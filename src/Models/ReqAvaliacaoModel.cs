using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoScale.src.Models
{
    [Table("req_avaliacao")]
    public class ReqAvaliacaoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("req_avaliacao_id")]
        public int Id { get; set; }

        [Column("relatorio_id"), ForeignKey("relatorio_id")]
        public required Relatorio Relatorio { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("em_aberto")]
        public bool EmAberto { get; set; } = true;

        [Column("avaliado")]
        public bool Avaliado { get; set; } = false;

        [Column("motivo")]
        public required string Motivo { get; set; }
    }
}