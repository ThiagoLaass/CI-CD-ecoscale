using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using EcoScale.src.Models;
using EcoScale.src.Public.Enum;

namespace EcoScale.src.Public.DTOs
{
    public class CompanyCreationRequest
    {
        [NotNull]
        public required string Cnpj { get; set; }

        [NotNull]
        public required string Senha { get; set; }

        [NotNull]
        public required string RazaoSocial { get; set; }

        [NotNull]
        public required string NumTelefone { get; set; }

        [NotNull]
        public required string EnderecoSede { get; set; }

        [NotNull]
        public required string Email { get; set; }

        [NotNull]
        public required ResponsavelCreationRequest Responsavel { get; set; }

        [JsonIgnore]
        public byte[]? FotoPerfil { get; set; }

        [JsonIgnore]
        public string? FotoMimeType { get; set; }

        [JsonPropertyName("fotoPerfil")]
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
                    var parts = value.Split(',', 2);
                    if (parts.Length == 2)
                    {
                        var mimePart = parts[0];
                        var mimeStart = "data:".Length;
                        var mimeEnd = mimePart.IndexOf(';', mimeStart);
                        if (mimeEnd > mimeStart)
                            FotoMimeType = mimePart[mimeStart..mimeEnd];
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

        public string? Descricao { get; set; }

        public string? SetorAtuacao { get; set; }
    }

    public class ResponsavelCreationRequest
    {

        [NotNull]
        public required string Nome { get; set; }

        [NotNull]
        public required string Telefone { get; set; }

        [NotNull]
        public required string Cpf { get; set; }
    }

    public class CompanyLoginRequest
    {
        [NotNull]
        public required string Email { get; set; }
        [NotNull]
        public required string Senha { get; set; }
    }

    public class CompanyUpdateRequest
    {
        public string? RazaoSocial { get; set; }
        public string? NumTelefone { get; set; }
        public string? EnderecoSede { get; set; }

        [JsonIgnore]
        public byte[]? FotoPerfil { get; set; }

        [JsonIgnore]
        public string? FotoMimeType { get; set; }

        [JsonPropertyName("fotoPerfil")]
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
                    var parts = value.Split(',', 2);
                    if (parts.Length == 2)
                    {
                        var mimePart = parts[0];
                        var mimeStart = "data:".Length;
                        var mimeEnd = mimePart.IndexOf(';', mimeStart);
                        if (mimeEnd > mimeStart)
                            FotoMimeType = mimePart[mimeStart..mimeEnd];
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
        public string? Descricao { get; set; }
        public string? SetorAtuacao { get; set; }
        public string? Email { get; set; }
        // public ResponsavelEmpresa? Responsavel { get; set; }
        public ICollection<int>? Questionarios { get; set; }
    }

    public class SolicitarAvaliacaoRequest
    {
        public required string Motivo { get; set; }
    }
}