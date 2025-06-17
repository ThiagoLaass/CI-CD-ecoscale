using EcoScale.src.Public.DTOs;
using EcoScale.src.Services.Interfaces;
using EcoScale.src.Models;
using EcoScale.src.Services.Abstract;
using EcoScale.src.Data;
using EcoScale.src.Services.Helpers;
using EcoScale.src.Middlewares.Exceptions;
using Microsoft.EntityFrameworkCore;

using AutoMapper;
using EcoScale.src.Models.Notifications;
using EcoScale.src.Public.DTOs.Responses;

namespace EcoScale.src.Services
{
    public class EmpresaService(AppDbContext context, IMapper mapper) : AbstractService(context), IEmpresa
    {
        private readonly EmpresaHelper _empresaHelper = new(context);
        private readonly Auth.Auth _auth = new(context);
        private readonly IMapper _mapper = mapper;

        public async Task<CreationResponse<Empresa>> New(CompanyCreationRequest request)
        {
            var cleanedCnpj = _helper.LimpaDocumento(request.Cnpj);
            var cleanedCpf = _helper.LimpaDocumento(request.Responsavel.Cpf);

            await _empresaHelper.ValidaDocumentosAsync(cleanedCnpj, cleanedCpf);

            if (_empresaHelper.ExisteResponsavelComCpf(cleanedCpf))
            {
                throw new ConflictException("Não foi possível concluir o processamento da solicitação devido a um conflito de dados.");
            }

            if (_empresaHelper.ExisteEmpresComCpnjOuEmail(cleanedCnpj, request.Email))
            {
                throw new ConflictException("Já existe uma empresa cadastrada com esse CNPJ ou email.");
            }

            var responsavel = _mapper.Map<ResponsavelEmpresa>(request.Responsavel);
            var empresa = _mapper.Map<Empresa>(request);
            empresa.NumTelefone = _helper.LimpaDocumento(empresa.NumTelefone);
            responsavel.Telefone = _helper.LimpaDocumento(responsavel.Telefone);
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                responsavel.Cpf = cleanedCpf;
                empresa.Cnpj = cleanedCnpj;
                empresa.Senha = _cryptography.CreateHash(request.Senha);

                _context.Empresas.Add(empresa);
                await _context.SaveChangesAsync();

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao criar empresa: {ex.Message}", ex);
            }
            return new CreationResponse<Empresa> { Token = _jwt.GenerateToken(empresa.Email, false) };
        }

        public async Task<List<Empresa>> GetAll()
        {
            return await _context.Empresas.Include(e => e.Responsavel).ToListAsync()
                ?? throw new NotFoundException("Nenhuma empresa encontrada.");
        }

        public async Task<Empresa> Patch(CompanyUpdateRequest empresaUpdate, HttpContext context)
        {
            var empresa = await _empresaHelper.FindInClaims(context) ?? throw new NotFoundException("Empresa não encontrada.");
            if (empresaUpdate.FotoPerfil is not null)
            {
                empresa.FotoPerfil = empresaUpdate.FotoPerfil;
                empresa.FotoMimeType = empresaUpdate.FotoMimeType;
                _context.Empresas.Update(empresa);
            }

            var properties = empresaUpdate.GetType().GetProperties();
            var updatedAttributes = new Dictionary<string, object>();
            foreach (var prop in properties)
            {
                if (prop.Name == nameof(empresaUpdate.Email)
                    || prop.Name == nameof(empresaUpdate.FotoPerfil)
                    || prop.Name == nameof(empresaUpdate.FotoPerfilBase64))
                {
                    continue;
                }

                var value = prop.GetValue(empresaUpdate);
                if (value is not null)
                {
                    updatedAttributes.Add(prop.Name, value);
                }
            }

            if (empresaUpdate.Email is not null)
            {
                var emailConfirmation = _auth.CreateEmailConfirmacao(empresa.Id.ToString());
                _context.EmailConfirmacoes.Add(emailConfirmation);
                _auth.SendConfirmationEmail(empresaUpdate.Email, emailConfirmation.Codigo);
            }

            return await _helper.AtualizarAtributosAsync<Empresa>(empresa.Id, updatedAttributes);
        }

        public async Task<Empresa> Del(HttpContext context)
        {
            string token = _jwt.GetToken(context);
            var cnpj = _jwt.GetSpecificClaim(token, "Cnpj");
            Empresa empresa = await _empresaHelper.FindByCnpj(cnpj)
                ?? throw new NotFoundException("Empresa não encontrada.");
            empresa.IsRemovida = true;
            _context.Empresas.Update(empresa);
            await _context.SaveChangesAsync();
            return empresa;
        }

        public async Task SolicitarAvaliacaoRelatorio(HttpContext context, SolicitarAvaliacaoRequest request)
        {
            var empresa = await _empresaHelper.FindInClaims(context);

            var relatorio = await _context.Relatorios
                .Include(r => r.Empresa)
                .ThenInclude(e => e.Responsavel)
                .FirstOrDefaultAsync(r => r.Empresa.Id == empresa.Id)
                ?? throw new NotFoundException("Relatório não encontrado.");

            ReqAvaliacaoModel reqAvaliacao = new()
            {
                Relatorio = relatorio,
                Motivo = request.Motivo
            };

            Notificacao notificacao = new()
            {
                Mensagem = $"Uma nova avaliação da empresa {empresa.RazaoSocial} foi solicitada.",
                Usuario = empresa,
            };

            _context.Notificacoes.Add(notificacao);
            _context.ReqAvaliacoes.Add(reqAvaliacao);
            await _context.SaveChangesAsync();
        }

        public async Task<RelatorioResponse> GetRelatorio(HttpContext context)
        {
            var empresa = await _empresaHelper.FindInClaims(context);
            var relatorio = await _context.Relatorios
                .Include(r => r.Recomendacoes)
                .FirstOrDefaultAsync(r => r.Empresa.Id == empresa.Id)
                ?? throw new NotFoundException("Relatório não encontrado.");
            return _mapper.Map<RelatorioResponse>(relatorio);
        }
    }
}