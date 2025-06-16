using System.Diagnostics.CodeAnalysis;
using EcoScale.src.Models;

namespace EcoScale.src.Public.DTOs
{
    public class ModeradorCreationRequest
    {
        [NotNull]
        public required string Nome { get; set; }

        [NotNull]
        public required string Telefone { get; set; }

        [NotNull]
        public required string Cpf { get; set; }

        [NotNull]
        public required string Email { get; set; }

        [NotNull]
        public required string Senha { get; set; }
    }

    public class AvaliarRelatorio
    {
        public required ReqAvaliacaoRequest ReqAvaliacao { get; set; }
        public required RelatorioUpdateRequest Relatorio { get; set; }
    }

    public class RelatorioUpdateRequest
    {
        public required int Id { get; set; }
        public required Empresa Empresa { get; set; }
        public string? DescricaoSustentabilidade { get; set; }
        public string? DescricaoMelhorias { get; set; }
        public bool Revisado { get; set; } = true;
    }

    public class ReqAvaliacaoRequest
    {
        public required int Id { get; set; }
        public Relatorio? Relatorio { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool EmAberto { get; set; } = true;
        public bool Avaliado { get; set; } = false;
        public string? Motivo { get; set; }
    }
}