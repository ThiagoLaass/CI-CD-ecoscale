using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EcoScale.src.Models
{
    [Table("relatorio")]
    public class Relatorio
    {
        [Key]
        [Column("relatorio_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("empresa_id"), ForeignKey("empresa_id")]
        public required Empresa Empresa { get; set; }

        [Column("revisor_id")]
        [AllowNull]
        public Moderador? Revisor { get; set; }

        [Column("boo_revisado")]
        public bool Revisado { get; set; } = false;

        [Column("nota")]
        public required double NotaSustentabilidade { get; set; }

        [Column("dsc_diagnostico")]
        public required string Diagnostico { get; set; }

        [Column("arr_pontos_criticos")]
        public required ICollection<string> PontosCriticos { get; set; }

        [Column("arr_pontos_fortes")]
        public required ICollection<string> PontosFortes { get; set; }

        [Column("recomendacoes"), ForeignKey("recomendacoes_id")]
        public required Recomendacoes Recomendacoes { get; set; }
    }

    [Table("recomendacoes")]
    public class Recomendacoes
    {
        [Key]
        [Column("recomendacoes_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("arr_projetos_1_ano")]
        public required ICollection<string> Projetos1Ano { get; set; }

        [Column("arr_quick_wins_90d")]
        public required ICollection<string> QuickWins90d { get; set; }

        [Column("arr_transformacoes_estrategicas")]
        public required ICollection<string> TransformacoesEstrategicas { get; set; }
    }
}