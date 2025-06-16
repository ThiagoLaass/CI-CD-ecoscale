using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EcoScale.src.Models.Abstract;

namespace EcoScale.src.Models
{
    [Table("empresa")]
    public class Empresa : Usuario
    {
        [NotNull]
        [Column("cnpj")]
        public required string Cnpj { get; set; }

        [Column("razao_social")]
        [NotNull]
        public required string RazaoSocial { get; set; }

        [Column("num_telefone")]
        [NotNull]
        public required string NumTelefone { get; set; }

        [Column("endereco_sede")]
        [NotNull]
        public required string EnderecoSede { get; set; }

        [Column("foto_perfil", TypeName = "bytea")]
        public byte[]? FotoPerfil { get; set; }

        [Column("foto_perfil_mime")]
        public string? FotoMimeType { get; set; }

        [NotMapped]
        public string? FotoPerfilBase64
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    FotoPerfil = null;
                    FotoMimeType = null;
                }
                else if (value.StartsWith("data:"))
                {
                    var parts = value.Split([','], 2);
                    if (parts.Length == 2)
                    {
                        var mimePart = parts[0];
                        var mimeStart = "data:".Length;
                        var mimeEnd = mimePart.IndexOf(';', mimeStart);
                        if (mimeEnd > mimeStart)
                        {
                            FotoMimeType = mimePart[mimeStart..mimeEnd];
                        }
                        FotoPerfil = Convert.FromBase64String(parts[1]);
                    }
                    else
                    {
                        FotoPerfil = null;
                        FotoMimeType = null;
                    }
                }
                else
                {
                    FotoPerfil = Convert.FromBase64String(value);
                    FotoMimeType = null;
                }
            }
        }

        [Column("dsc_empresa")]
        public string? Descricao { get; set; }

        [Column("dsc_contexto")]
        public string? Contexto { get; set; }

        [Column("setor_atuacao")]
        public string? SetorAtuacao { get; set; }

        [Column("boo_removida")]
        public bool IsRemovida { get; set; } = false;

        [Column("responsavel_id"), ForeignKey("responsavel_id")]
        [NotNull]
        public required ResponsavelEmpresa Responsavel { get; set; }


    }
}