using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EcoScale.src.Models.Abstract;


namespace EcoScale.src.Models
{
    [Table("moderador")]
    public class Moderador : Usuario {
        [Column("nome")]
        [NotNull]
        public required string Nome { get; set; }

        [NotMapped]
        public bool IsModerador { get; set; } = true; // Atributo utilizado apenas para validação da autenticação
    }
}