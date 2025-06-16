using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Org.BouncyCastle.Bcpg;

namespace EcoScale.src.Models
{
    [Table("responsavel_empresa")]
    public class ResponsavelEmpresa
    {
        [Key]
        [Column("responsavel_empresa_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("nome"), NotNull]
        public required string Nome { get; set; }

        [Column("telefone"), NotNull]
        public required string Telefone { get; set; }

        [Column("cpf"), NotNull]
        public required string Cpf { get; set; }
    }
}