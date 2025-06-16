using EcoScale.src.Auth;
using EcoScale.src.Data;
using EcoScale.src.Services.Helpers;

namespace EcoScale.src.Services.Abstract
{
    public abstract class AbstractService(AppDbContext context) {
        protected readonly Cryptography _cryptography = new();
        protected readonly AppDbContext _context = context;
        protected readonly Helper _helper = new(context);
        protected readonly Jwt _jwt = new();
    }
}