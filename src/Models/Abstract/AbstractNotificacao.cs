using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EcoScale.src.Models.Abstract
{
    public abstract class AbstractNotificacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotNull]
        [Column("ref_id")] // referencia o Id da entidade que recebeu o email
        public required string RefId { get; set; }

        [Column("expires_at")]
        [NotNull]
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(15);

        [Column("created_at")]
        [NotNull]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}