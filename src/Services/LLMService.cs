using AutoMapper;
using EcoScale.src.Data;
using EcoScale.src.Models;
using EcoScale.src.Models.Abstract;
using EcoScale.src.Public.DTOs;
using EcoScale.src.Public.DTOs.Responses;
using EcoScale.src.Services.Helpers;
namespace EcoScale.src.Services
{
    public class LLMService(AppDbContext context, IMapper mapper)
    {
        private readonly IMapper _mapper = mapper;
        private readonly Api _api = new();
        private readonly Helper _helper = new(context);
        private readonly string _uri = "https://ecoscale-llm.onrender.com/";

        public async Task<RelatorioResponse> GetRespostaLLM(GetLLMResponseRequest request)
        {
            
            var response = await _api.PostAsync(
                $"{_uri}avaliar-empresa",
                request
            );

            var llmResp = await response.Content.ReadFromJsonAsync<LLMResponse>()
                ?? throw new Exception("Erro ao interpretar a resposta da LLM.");

            // ********** MOCK DE TESTE **********
            // var recomendacoes = new RecomendacoesResponseLLM
            // {
            //     projetos_1_ano = new List<string> { "Projeto A", "Projeto B" },
            //     quick_wins_90d = new List<string> { "Quick Win A", "Quick Win B" },
            //     transformacoes_estrategicas = new List<string> { "Transformação Estratégica A", "Transformação Estratégica B", "Transformação Estratégica C" }
            // };

            // var relatorio = new RelatorioResponseLLM
            // {
            //     diagnostico = "Diagnóstico Exemplo",
            //     pontos_criticos = new List<string> { "Ponto Crítico A", "Ponto Crítico B" },
            //     pontos_fortes = new List<string> { "Ponto Forte A", "Ponto Forte B" },
            //     recomendacoes = recomendacoes
            // };

            // var llmResp = new LLMResponse
            // {
            //     nota_sustentabilidade = 4.5f,
            //     relatorio = relatorio
            // };

            return _mapper.Map<RelatorioResponse>(llmResp);
        }

        public async Task<ICollection<CriterioPlanilha>> GetCriterios(GetCriteriosRequest request)
        {

            var response = await _api.PostAsync(
                $"{_uri}avaliar-criterios", 
                request
            );

            var ids = await response.Content.ReadFromJsonAsync<GetCriteriosResponse>()
                ?? throw new Exception("Erro ao interpretar a resposta da LLM.");

            return await _helper.GetCriterios(ids.criterios_ids)
                ?? throw new Exception("Erro ao obter critérios.");
        }
    }
}
