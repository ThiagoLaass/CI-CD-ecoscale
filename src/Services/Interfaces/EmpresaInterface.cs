using EcoScale.src.Public.DTOs;
using EcoScale.src.Models;

namespace EcoScale.src.Services.Interfaces
{
    public interface IEmpresa {
        Task<CreationResponse<Empresa>> New(CompanyCreationRequest e);
        Task<List<Empresa>> GetAll();
        Task<Empresa> Patch(CompanyUpdateRequest empresaUpdate, HttpContext context);
        Task<Empresa> Del(HttpContext context);
    }
}