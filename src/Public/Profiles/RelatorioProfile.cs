using AutoMapper;
using EcoScale.src.Models;
using EcoScale.src.Public.DTOs;
using EcoScale.src.Public.DTOs.Responses;

namespace EcoScale.src.Public.Profiles
{
    public class RelatorioProfile : Profile
    {
        public RelatorioProfile()
        {
            CreateMap<RelatorioUpdateRequest, Relatorio>();
            CreateMap<ReqAvaliacaoRequest, ReqAvaliacaoModel>();
            CreateMap<LLMResponse, RelatorioResponse>()
                .ForMember(dest => dest.NotaSustentabilidade, opt => opt.MapFrom(src => src.nota_sustentabilidade))
                .ForMember(dest => dest.Diagnostico, opt => opt.MapFrom(src => src.relatorio.diagnostico))
                .ForMember(dest => dest.PontosCriticos, opt => opt.MapFrom(src => src.relatorio.pontos_criticos))
                .ForMember(dest => dest.PontosFortes, opt => opt.MapFrom(src => src.relatorio.pontos_fortes))
                .ForMember(dest => dest.Recomendacoes, opt => opt.MapFrom(src => src.relatorio.recomendacoes));
            CreateMap<RecomendacoesResponseLLM, RecomendacoesResponse>()
                .ForMember(dest => dest.Projetos1Ano, opt => opt.MapFrom(src => src.projetos_1_ano))
                .ForMember(dest => dest.QuickWins90d, opt => opt.MapFrom(src => src.quick_wins_90d))
                .ForMember(dest => dest.TransformacoesEstrategicas, opt => opt.MapFrom(src => src.transformacoes_estrategicas));
            CreateMap<RelatorioResponse, Relatorio>();
            CreateMap<RecomendacoesResponse, Recomendacoes>();

            CreateMap<Recomendacoes, RecomendacoesResponse>();
            CreateMap<Relatorio, RelatorioResponse>();
        }
    }
}