namespace EcoScale.src.Public.Profiles
{
    using AutoMapper;
    using EcoScale.src.Models;
    using EcoScale.src.Public.DTOs;

    public class QuestionarioProfile : Profile
    {
        public QuestionarioProfile()
        {
            CreateMap<QuestionarioCreationRequest, Questionario>();
            CreateMap<TemaCreationRequest, Tema>();
            CreateMap<CriterioCreationRequest, Criterio>();
            CreateMap<ItemAvaliadoCreationRequest, ItemAvaliado>();
            CreateMap<GetQuestionarioResponse, Questionario>();
            CreateMap<AreaCreationRequest, Area>();
        }
    }
}