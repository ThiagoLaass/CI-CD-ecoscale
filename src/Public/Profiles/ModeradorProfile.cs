using AutoMapper;
using EcoScale.src.Models;
using EcoScale.src.Public.DTOs;

namespace EcoScale.src.Public.Profiles
{
    public class ModeradorProfile : Profile
    {
        public ModeradorProfile()
        {
            CreateMap<ModeradorCreationRequest, Moderador>();
        }
    }
}