using AutoMapper;
using EcoScale.src.Data;
using EcoScale.src.Middlewares.Exceptions;
using EcoScale.src.Models;
using EcoScale.src.Models.Abstract;
using EcoScale.src.Public.DTOs;
using EcoScale.src.Public.DTOs.Responses;
using EcoScale.src.Services.Abstract;
using EcoScale.src.Services.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EcoScale.src.Services
{
    public class QuestionarioService(AppDbContext context, IMapper mapper) : AbstractService(context)
    {
        private readonly IMapper _mapper = mapper;
        private readonly LLMService _llm = new(context, mapper);
        private readonly EmpresaHelper _empresaHelper = new(context);
        public async Task<Questionario> Del(int id)
        {
            Questionario q = await _context.Questionarios.FindAsync(id)
                ?? throw new NotFoundException("Questionario não encontrado.");
            q.Removido = true;
            _context.Questionarios.Update(q);
            await _context.SaveChangesAsync();
            return q;
        }

        public async Task<Questionario> Get(int id)
        {
            return await _context.Questionarios.Include(q => q.Areas)
                .ThenInclude(t => t.Temas)
                .ThenInclude(c => c.Criterios)
                .ThenInclude(i => i.Itens)
                .Include(q => q.Empresa)
                .ThenInclude(e => e.Responsavel)
                .FirstOrDefaultAsync(q => q.Id == id && !q.Removido)
                ?? throw new NotFoundException("Questionario não encontrado.");
        }

        public async Task<QuestionarioResponse> New(GetCriteriosRequest request, HttpContext context)
        {
            var empresa = await _empresaHelper.FindInClaims(context);
            var criteriosResponse = await _llm.GetCriterios(request);
            var criteriosIds = criteriosResponse.Select(c => c.Id).ToHashSet();

            var planilha = await _context.Planilhas
                .Include(p => p.Areas)!
                    .ThenInclude(a => a.Temas)!
                        .ThenInclude(t => t.Criterios)!
                            .ThenInclude(c => c.Itens)
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException("Planilha não encontrada.");

            var areasFiltradas = planilha.Areas
                .Select(a =>
                {
                    var temasFiltrados = a.Temas
                        .Select(t =>
                        {
                            var criteriosFiltrados = t.Criterios
                                .Where(c => criteriosIds.Contains(c.Id))
                                .Select(c => new Criterio
                                {
                                    Nome = c.Nome,
                                    Itens = [.. c.Itens.Select(i => new ItemAvaliado { Descricao = i.Descricao })]
                                })
                                .ToList();
                            if (criteriosFiltrados.Count == 0) return null;

                            var temaFiltrado = new Tema
                            {
                                Nome = t.Nome,
                                Criterios = criteriosFiltrados
                            };
                            return temaFiltrado;
                        })
                        .OfType<Tema>()
                        .ToList();
                    if (temasFiltrados.Count == 0) return null;

                    var areaFiltrada = new Area
                    {
                        Nome = a.Nome,
                        Temas = temasFiltrados
                    };
                    return areaFiltrada;
                })
                .OfType<Area>()
                .ToList();

            var questionario = new Questionario
            {
                Empresa = empresa,
                Areas = areasFiltradas
            };

            empresa.Contexto = request.contexto;
            _context.Questionarios.Add(questionario);
            await _context.SaveChangesAsync();

            return new QuestionarioResponse
            {
                Areas = areasFiltradas
            };
        }

        public async Task<ICollection<Questionario>> GetByIds(ICollection<int> ids)
        {
            return await _context.Questionarios.Where(q => ids.Contains(q.Id)).ToListAsync()
                ?? throw new NotFoundException("Nenhum questionário encontrado.");
        }

        public async Task<GetQuestionarioResponse> GetAll()
        {
            var questionarios = await _context.Questionarios.
            Include(q => q.Empresa).
            Include(q => q.Areas).
            ThenInclude(t => t.Temas).
            ThenInclude(c => c.Criterios).
            ThenInclude(i => i.Itens).ToListAsync()
                ?? throw new NotFoundException("Nenhum questionário encontrado.");
            return new GetQuestionarioResponse { Questionarios = questionarios };
        }

        public async Task<Questionario> RespostaEmBlocos(RespostaEmBlocosRequest request)
        {
            Questionario questionario = await _context.Questionarios.Include(q => q.Areas)
                .ThenInclude(t => t.Temas)
                .ThenInclude(c => c.Criterios)
                .ThenInclude(i => i.Itens)
                .FirstOrDefaultAsync(q => q.Id == request.QuestionarioId && !q.Removido)
                ?? throw new NotFoundException("Questionario não encontrado.");
            var itemRequestMap = (request.Itens ?? Enumerable.Empty<ItemAvaliadoUpdateRequest>())
                .ToDictionary(i => i.Id, i => i);

            foreach (var item in questionario.Areas.SelectMany(a => a.Temas)
                .SelectMany(t => t.Criterios)
                .SelectMany(c => c.Itens))
            {
                if (itemRequestMap.TryGetValue(item.Id, out var itemRequest))
                {
                    item.Resposta = itemRequest.Resposta;
                }
            }

            _context.Questionarios.Update(questionario);
            await _context.SaveChangesAsync();
            return questionario;
        }

        public ICollection<CriterioPlanilha> GetCriterioByIds(GetCriteriosByIdsRequest request)
        {
            return _context.CriteriosPlanilha.Include(c => c.Itens).Where(c => request.CriteriosIds.Contains(c.Id)).ToListAsync().Result
                ?? throw new NotFoundException("Critério não encontrado.");
        }

        public async Task<Questionario> GetQuestionarioByToken(HttpContext context)
        {
            var empresa = await _empresaHelper.FindInClaims(context);
            var questionario = await _context.Questionarios
                .Include(q => q.Areas)
                    .ThenInclude(a => a.Temas)
                        .ThenInclude(t => t.Criterios)
                            .ThenInclude(c => c.Itens)
                .FirstOrDefaultAsync(q => q.Id == empresa.Id && !q.Removido)
                ?? throw new NotFoundException("Questionário não encontrado.");
            return questionario;
        }

        public async Task<RelatorioResponse> FinalizarQuestionario(int questionarioId, HttpContext httpContext)
        {
            var questionario = await _context.Questionarios
                .Include(q => q.Areas)
                    .ThenInclude(a => a.Temas)
                        .ThenInclude(t => t.Criterios)
                            .ThenInclude(c => c.Itens)
                .Include(q => q.Empresa)
                .FirstOrDefaultAsync(q => q.Id == questionarioId && !q.Removido)
                ?? throw new NotFoundException("Questionário não encontrado.");

            if (questionario.Areas
                .SelectMany(a => a.Temas)
                .SelectMany(t => t.Criterios)
                .SelectMany(c => c.Itens)
                .Any(i => i.Resposta == null))
            {
                throw new BadRequestException("Nem todos os itens foram respondidos.");
            }

            var llmRequest = new GetLLMResponseRequest
            {
                questionario = questionario
            };

            var relatorioResp = await _llm.GetRespostaLLM(llmRequest);
            var empresa = await _empresaHelper.FindInClaims(httpContext);
            var relatorio = _mapper.Map<Relatorio>(relatorioResp);
            relatorio.Empresa = empresa;

            _context.Relatorios.Add(relatorio);
            await _context.SaveChangesAsync();

            return relatorioResp;
        }

        public async Task<QuestionarioResponse> GetCriteriosNaoRespondidos(HttpContext context)
        {
            var questionario = await GetQuestionarioByToken(context);
            var areasFiltradas = questionario.Areas
                .Select(a =>
                {
                    var temasFiltrados = a.Temas
                        .Select(t =>
                        {
                            var criteriosFiltrados = t.Criterios
                                .Where(c => c.Itens.Any(i => i.Resposta == null))
                                .Select(c => new Criterio
                                {
                                    Id = c.Id,
                                    Nome = c.Nome,
                                    Itens = [.. c.Itens
                                        .Where(i => i.Resposta == null)
                                        .Select(i => new ItemAvaliado { Descricao = i.Descricao, Id = i.Id })]
                                })
                                .ToList();
                            if (criteriosFiltrados.Count == 0)
                                return null;

                            return new Tema
                            {
                                Id = t.Id,
                                Nome = t.Nome,
                                Criterios = criteriosFiltrados
                            };
                        })
                        .OfType<Tema>()
                        .ToList();

                    if (temasFiltrados.Count == 0)
                        return null;

                    return new Area
                    {
                        Id = a.Id,
                        Nome = a.Nome,
                        Temas = temasFiltrados
                    };
                })
                .OfType<Area>()
                .ToList();

            return new QuestionarioResponse
            {
                QuestionarioId = questionario.Id,
                Areas = areasFiltradas
            };
        }

        public async Task<bool> RespondeuTudo(HttpContext context)
        {
            var questionario = await GetQuestionarioByToken(context);
            return questionario.Areas
                .SelectMany(a => a.Temas)
                .SelectMany(t => t.Criterios)
                .SelectMany(c => c.Itens)
                .All(i => i.Resposta != null);
        }
    }
}