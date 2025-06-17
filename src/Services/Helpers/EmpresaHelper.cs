using EcoScale.src.Data;
using EcoScale.src.Middlewares.Exceptions;
using EcoScale.src.Models;
using EcoScale.src.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EcoScale.src.Services.Helpers
{
    public partial class EmpresaHelper(AppDbContext context) : AbstractService(context) {
        private readonly ApiHelper _api = new();
        public async Task<bool> ValidaCnpj(string Cnpj)
        {
            if (!ValidaNumeroCnpj(Cnpj)) return false;
            return true;
            // try {
            //     var response = await _api.GetAsync($"https://open.cnpja.com/office/{Cnpj}");
            //     return Equals(response.StatusCode, System.Net.HttpStatusCode.OK);
            // }
            // catch (HttpRequestException ex) when (ex.StatusCode.Equals(System.Net.HttpStatusCode.BadRequest) 
            // || ex.StatusCode.Equals(System.Net.HttpStatusCode.NotFound)) {
            //     return false;
            // }
            // catch (Exception e) {
            //     throw new Exception($"Erro inesperado na chamada de api para validação do CNPJ: {e.Message}");
            // }
        }

        private static bool ValidaNumeroCnpj(string Cnpj) {
            if (!CnpjRegex().IsMatch(Cnpj))
            return false;

            if (Cnpj.Distinct().Count() == 1) return false;

            var digits = Cnpj.Select(c => int.Parse(c.ToString())).ToArray();

            int[] weight1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            int sum = 0;
            for (int i = 0; i < 12; i++) {
                sum += digits[i] * weight1[i];
            }
            int remainder = sum % 11;
            int firstDigit = remainder < 2 ? 0 : 11 - remainder;
            if (digits[12] != firstDigit) return false;

            int[] weight2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            sum = 0;
            for (int i = 0; i < 13; i++) {
                sum += digits[i] * weight2[i];
            }
            remainder = sum % 11;
            int secondDigit = remainder < 2 ? 0 : 11 - remainder;
            if (digits[13] != secondDigit) return false;

            return true;
        }

        public async Task<Empresa> FindByCnpj(string Cnpj) {
            return await _context.Empresas.Include(e => e.Responsavel).FirstOrDefaultAsync(e => Equals(e.Cnpj, Cnpj))
                ?? throw new NotFoundException("Empresa não encontrada.");
        }

        public async Task<Empresa> FindInClaims(HttpContext context) {
            var email = context.User.FindFirst("Email")?.Value;
            var empresa = await _context.Empresas.Include(e => e.Responsavel).Where(e => e.Email == email)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Empresa não encontrada.");
            return empresa;
        }

        public bool ExisteResponsavelComCpf(string cpf)
            => _context.ResponsavelEmpresa
                    .Any(r => r.Cpf == cpf);

        public bool ExisteEmpresComCpnjOuEmail(string cnpj, string email)
            => _context.Empresas
                    .Any(e => e.Cnpj == cnpj || e.Email == email); 
        
        public async Task<Empresa?> GetByArgs(string? Cnpj = null, string? Email = null) {
            return await _context.Empresas.FirstOrDefaultAsync(e => Equals(e.Cnpj, Cnpj) || Equals(e.Email, Email));
        }

        public async Task ValidaDocumentosAsync(string cnpj, string cpf)
        {
            cnpj = _helper.LimpaDocumento(cnpj);
            cpf  = _helper.LimpaDocumento(cpf);

            if (!await ValidaCnpj(cnpj))
                throw new BadRequestException("CNPJ inválido");

            if (!_helper.ValidaCpf(cpf))
                throw new BadRequestException("CPF do responsavel inválido");
        }

        [System.Text.RegularExpressions.GeneratedRegex(@"^\d{14}$")]
        private static partial System.Text.RegularExpressions.Regex CnpjRegex();
    }
}