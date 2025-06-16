using AutoMapper;
using EcoScale.src.Models;
using EcoScale.src.Public.DTOs;

namespace EcoScale.src.Public.Profiles
{
    public class EmpresaProfile : Profile
    {
        public EmpresaProfile()
        {
            CreateMap<CompanyCreationRequest, Empresa>();
            CreateMap<CompanyUpdateRequest, Empresa>();
            CreateMap<ResponsavelCreationRequest, ResponsavelEmpresa>();
        }
    }
}