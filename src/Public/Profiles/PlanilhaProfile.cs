using AutoMapper;
using EcoScale.src.Models.Abstract;
using EcoScale.src.Public.DTOs;

namespace EcoScale.src.Public.Profiles
{
    public class PlanilhaProfile : Profile
    {
        public PlanilhaProfile()
        {
            CreateMap<PlanilhaCreationRequest, Planilha>();
            CreateMap<AreaPlanilhaCreationRequest, AreaPlanilha>();
            CreateMap<TemaPlanilhaCreationRequest, TemaPlanilha>();
            CreateMap<CriterioPlanilhaCreationRequest, CriterioPlanilha>();
            CreateMap<ItemAvaliadoPlanilhaCreationRequest, ItemAvaliadoPlanilha>();

            CreateMap<PlanilhaUpdateRequest, Planilha>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<AreaPlanilhaUpdateRequest, AreaPlanilha>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<TemaPlanilhaUpdateRequest, TemaPlanilha>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CriterioPlanilhaUpdateRequest, CriterioPlanilha>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ItemAvaliadoPlanilhaUpdateRequest, ItemAvaliadoPlanilha>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}