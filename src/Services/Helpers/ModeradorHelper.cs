using EcoScale.src.Data;
using EcoScale.src.Middlewares.Exceptions;
using EcoScale.src.Models;
using EcoScale.src.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EcoScale.src.Services.Helpers
{
    public class ModeradorHelper(AppDbContext context) : AbstractService(context)
    {
       public async Task<Moderador> FindInClaims(HttpContext context)
        {
            string token = _jwt.GetToken(context);
            var email = _jwt.GetSpecificClaim(token, "Email");
            return await _context.Moderadores.FirstOrDefaultAsync(e => Equals(e.Email, email))
                ?? throw new NotFoundException("Empresa não encontrada.");
        }
    }
}