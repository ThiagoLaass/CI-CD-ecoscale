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
    public class ModeradorService(AppDbContext context, IMapper mapper) : AbstractService(context)
    {
        private readonly IMapper _mapper = mapper;
        private readonly ModeradorHelper _moderadorHelper = new(context);

        public async Task<Moderador> Get(HttpContext context)
        {
            return await _moderadorHelper.FindInClaims(context);
        }

        public async Task<List<Moderador>> GetAll()
        {
            var moderadores = await _context.Moderadores.ToListAsync();
            return moderadores ?? throw new NotFoundException("Nenhum moderador encontrado.");
        }

        public async Task<CreationResponse<Moderador>> New(ModeradorCreationRequest request)
        {
            var moderador = _mapper.Map<Moderador>(request);
            var senha = _cryptography.CreateHash(request.Senha);
            moderador.Senha = senha;
            moderador.Role = Role.Moderador;
            _context.Moderadores.Add(moderador);
            await _context.SaveChangesAsync();
            return new CreationResponse<Moderador>
            {
                Entity = moderador,
                Token = _jwt.GenerateToken(moderador.Email, true)
            };
        }
        public async Task<Relatorio> AvaliarRelatorio(HttpContext context, AvaliarRelatorio request)
        {
            var moderador = await _moderadorHelper.FindInClaims(context);
            var reqAvaliacao = await _context.ReqAvaliacoes
            .Include(r => r.Relatorio)
                .ThenInclude(r => r.Empresa)
                .ThenInclude(e => e.Responsavel)
            .FirstOrDefaultAsync(r => r.Id == request.ReqAvaliacao.Id)
            ?? throw new NotFoundException("Requisição de avaliação não encontrada.");

            var relatorio = reqAvaliacao.Relatorio
            ?? throw new NotFoundException("Relatório não encontrado.");

            reqAvaliacao.Avaliado = true;
            reqAvaliacao.UpdatedAt = DateTime.UtcNow;
            reqAvaliacao.EmAberto = false;
            _mapper.Map(request.ReqAvaliacao, reqAvaliacao);

            relatorio.Revisado = true;
            relatorio.Revisor = moderador;
            _mapper.Map(request.Relatorio, relatorio);
            var DataFormatada = reqAvaliacao.UpdatedAt.ToString("dd/MM/yyyy");
            var notificacao = new Notificacao
            {
                Mensagem = $"O seu relatório {relatorio.Id} foi avaliado por {moderador.Nome}. Na data {DataFormatada}.",
                Usuario = relatorio.Empresa,
            };

            _context.Notificacoes.Add(notificacao);

            await _context.SaveChangesAsync();
            return relatorio;
        }
    }
}