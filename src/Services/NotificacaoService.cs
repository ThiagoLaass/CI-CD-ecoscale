using AutoMapper;
using EcoScale.src.Data;
using EcoScale.src.Middlewares.Exceptions;
using EcoScale.src.Models;
using EcoScale.src.Models.Notifications;
using EcoScale.src.Public.DTOs;
using EcoScale.src.Public.Enum;
using EcoScale.src.Services.Abstract;
using EcoScale.src.Services.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EcoScale.src.Services
{
    public class NotificacaoService(AppDbContext context, IMapper mapper) : AbstractService(context)
    {
        private readonly IMapper _mapper = mapper;

        public async Task<List<Notificacao>> GetAll()
        {
            var notis = await _context.Notificacoes.ToListAsync();
            return notis ?? throw new NotFoundException("Nenhuma notificacao encontrada.");
        }

        public async Task<ICollection<Notificacao>> GetAllNotificacoesByUser(HttpContext context)
        {
            var user = await _helper.GetUserFromClaims(context) ?? throw new UnauthorizedException("Usuário não autenticado.");
            var notificacoes = await _context.Notificacoes
                .Where(n => n.Id == user.Id && !n.Lida)
                .ToListAsync();

            if (notificacoes.Count == 0)
            {
                throw new NotFoundException("Nenhuma notificação encontrada para o usuário.");
            }

            return notificacoes;
        }

        public async void MarkAsRead(HttpContext context, int id)
        {
            var user = await _helper.GetUserFromClaims(context);
            var notificacao = await _context.Notificacoes
                .FirstOrDefaultAsync(n => n.Id == id && n.Id == user.Id) ?? throw new NotFoundException("Notificação não encontrada.");
            notificacao.Lida = true;
            _context.Notificacoes.Update(notificacao);
            await _context.SaveChangesAsync();
        }
    }
}