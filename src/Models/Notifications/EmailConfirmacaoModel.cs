using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EcoScale.src.Models.Notifications
{
    [Table("email_confirmacao")]
    public class EmailConfirmacao : Abstract.AbstractNotificacao
    {
        [Column("num_codigo")]
        [NotNull]
        public required string Codigo { get; set; }

        [Column("boo_confirmado")]
        [NotNull]
        public required bool Confirmado { get; set; }

        [Column("boo_excluido")]
        public bool Excluido { get; set; } = false;
    }
}