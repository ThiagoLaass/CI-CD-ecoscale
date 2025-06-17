using AutoMapper;
using EcoScale.src.Data;
using EcoScale.src.Middlewares.Exceptions;
using EcoScale.src.Public.DTOs.Responses;
using EcoScale.src.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EcoScale.src.Services
{
    public class RelatorioService(AppDbContext context, IMapper mapper) : AbstractService(context)
    {
        private readonly IMapper _mapper = mapper;

        public async Task<RelatorioResponse> GetRelatorioById(int Id)
        {
            var relatorio = await _context.Relatorios
                .Include(r => r.Recomendacoes)
                .FirstOrDefaultAsync(r => r.Empresa.Id == Id)
                ?? throw new NotFoundException("Relatório não encontrado.");
            return _mapper.Map<RelatorioResponse>(relatorio);
        }

        public async Task<ICollection<RelatorioResponse>> GetAllRelatorios()
        {
            var relatorios = await _context.Relatorios
                .Include(r => r.Recomendacoes)
                .ToListAsync();
            if (relatorios.Count == 0) throw new NotFoundException("Nenhum relatório encontrado.");
            return [.. relatorios.Select(r => _mapper.Map<RelatorioResponse>(r))];
        }
    }
}