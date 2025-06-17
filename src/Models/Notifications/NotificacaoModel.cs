using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EcoScale.src.Models.Abstract;

namespace EcoScale.src.Models.Notifications
{
    [Table("notificacao")]
    public class Notificacao
    {
        [Key]
        [Column("notificacao_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("mensagem")]
        [NotNull]
        public required string Mensagem { get; set; }

        [Column("boo_lida")]
        [NotNull]
        public bool Lida { get; set; } = false;

        [Column("usuario_id"), ForeignKey("usuario_id")]
        [NotNull]
        public required Usuario Usuario { get; set; }
    }
}