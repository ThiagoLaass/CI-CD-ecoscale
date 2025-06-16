using AutoMapper;
using EcoScale.src.Models;
using EcoScale.src.Public.DTOs;

namespace EcoScale.src.Public.Profiles
{
    public class ResponsavelEmpresaProfile : Profile
    {
        public ResponsavelEmpresaProfile()
        {
            CreateMap<ResponsavelCreationRequest, ResponsavelEmpresa>();
        }
    }
}