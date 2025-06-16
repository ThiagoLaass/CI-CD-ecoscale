using EcoScale.src.Models;

namespace EcoScale.src.Public.DTOs.Responses
{
    public class LLMResponse
    {
        public required float nota_sustentabilidade { get; set; }
        public required RelatorioResponseLLM relatorio { get; set; }
    }

    public class RelatorioResponseLLM
    {
        public string? diagnostico { get; set; }
        public ICollection<string>? pontos_criticos { get; set; }
        public ICollection<string>? pontos_fortes { get; set; } 
        public RecomendacoesResponseLLM? recomendacoes { get; set; }
    }

    public class RecomendacoesResponseLLM
    {
        public ICollection<string>? projetos_1_ano { get; set; }
        public ICollection<string>? quick_wins_90d { get; set; }
        public ICollection<string>? transformacoes_estrategicas { get; set; }
    }

    public class RelatorioResponse
    {
        public required string Diagnostico { get; set; }
        public required float NotaSustentabilidade { get; set; }
        public required ICollection<string> PontosCriticos { get; set; }
        public required ICollection<string> PontosFortes { get; set; }
        public required RecomendacoesResponse Recomendacoes { get; set; }
    }

    public class RecomendacoesResponse
    {
        public required ICollection<string> Projetos1Ano { get; set; }
        public required ICollection<string> QuickWins90d { get; set; }
        public required ICollection<string> TransformacoesEstrategicas { get; set; }
    }

    public class GetCriteriosResponse
    {
        public required ICollection<int> criterios_ids { get; set; }
    }
}