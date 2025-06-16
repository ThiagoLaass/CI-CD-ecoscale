namespace EcoScale.src.Models.Abstract
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;
    using EcoScale.src.Public.Enum;
    using Microsoft.EntityFrameworkCore;

    [Index(nameof(Email), IsUnique = true)]
    public abstract class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("usuario_id")]
        public int Id { get; set; }

        [NotNull]
        [Column("email")]
        public required string Email { get; set; }

        [NotNull]
        [Column("senha")]
        public required string Senha { get; set; }

        [Column("boo_email_confirmado")]
        [NotNull]
        public bool EmailConfirmado { get; set; } = false;

        [Column("role")]
        [NotNull]
        public Role Role { get; set; } = Role.Empresa;
    }
}