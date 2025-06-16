using AutoMapper;
using EcoScale.src.Data;
using EcoScale.src.Middlewares.Exceptions;
using EcoScale.src.Models.Abstract;
using EcoScale.src.Public.DTOs;
using EcoScale.src.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EcoScale.src.Services
{
    public class PlanilhaService(AppDbContext context, IMapper mapper) : AbstractService(context)
    {
        private readonly IMapper _mapper = mapper;
        public async Task<Planilha> Get(int id)
        {
            return await _context.Planilhas
                .Include(p => p.Areas)
                    .ThenInclude(a => a.Temas)
                        .ThenInclude(t => t.Criterios)
                            .ThenInclude(c => c.Itens)
                .FirstOrDefaultAsync(p => p.Id == id && !p.Removido)
                ?? throw new NotFoundException("Planilha não encontrada.");
        }

        public async Task<CreationResponse<Planilha>> New(PlanilhaCreationRequest request)
        {
            var planilha = _mapper.Map<Planilha>(request);
            _context.Planilhas.Add(planilha);
            await _context.SaveChangesAsync();
            return new CreationResponse<Planilha>
            {
                Entity = planilha,
            };
        }

        public async Task<Planilha> Patch(PlanilhaUpdateRequest request)
        {
            var planilha = await _context.Planilhas
                .Include(p => p.Areas)
                    .ThenInclude(a => a.Temas)
                        .ThenInclude(t => t.Criterios)
                            .ThenInclude(c => c.Itens)
                .FirstOrDefaultAsync(p => p.Id == request.Id && !p.Removido)
                ?? throw new NotFoundException("Planilha não encontrada.");
            _mapper.Map(request, planilha);
            _context.Planilhas.Update(planilha);
            await _context.SaveChangesAsync();
            return planilha;
        }
    }
}