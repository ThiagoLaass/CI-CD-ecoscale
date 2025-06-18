using EcoScale.src.Auth;
using EcoScale.src.Data;
using EcoScale.src.Middlewares.Exceptions;
using EcoScale.src.Models;
using EcoScale.src.Models.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EcoScale.src.Services.Helpers
{
    public class Helper(AppDbContext context)
    {
//hg
        protected readonly AppDbContext _context = context;
        protected readonly Auth.Auth _auth = new(context);
        protected readonly Cryptography _cryptography = new();

        public bool SenhaValida(string senha, string senhaHashed)
        {
            return _cryptography.VerifyHash(senhaHashed, senha);
        }

        /// <summary>
        /// Atualiza os atributos de uma entidade do tipo especificado usando um dicionário de atributos.
        /// </summary>
        /// <typeparam name="T">O tipo da entidade a ser atualizada.</typeparam>
        /// <param name="chave">A chave utilizada para localizar a entidade no contexto.</param>
        /// <param name="atributos">Um dicionário contendo os pares nome/valor dos atributos a serem atualizados.</param>
        /// <remarks>
        /// Se a entidade não for encontrada, será lançada uma <see cref="NotFoundException"/>.
        /// <para>
        /// Caso o atributo "Senha" seja informado, seu valor passará por um processo de hash utilizando o objeto <c>_cryptography</c>.
        /// Se o valor informado para "Senha" for nulo, será lançada uma <see cref="BadRequestException"/>.
        /// </para>
        /// Após a atualização dos atributos modificados, as alterações são persistidas no banco de dados e o contexto é descartado.
        /// </remarks>
        /// <returns>A entidade atualizada do tipo <typeparamref name="T"/>.</returns>
        public async Task<T> AtualizarAtributosAsync<T>(object chave, Dictionary<string, object> atributos) where T : class
        {
            T entidade = await _context.Set<T>().FindAsync(chave)
            ?? throw new NotFoundException("Entidade não encontrada.");

            foreach (var kvp in atributos)
            {
                var propriedade = entidade.GetType().GetProperty(kvp.Key);
                if (propriedade == null || kvp.Value is null)
                    continue;

                switch (kvp.Key.ToLowerInvariant())
                {
                    case "senha":
                    case "cpf":
                    case "cnpj":
                        string valor = kvp.Value.ToString()
                            ?? throw new BadRequestException($"Se o campo {kvp.Key} for informado, ele não pode ser nulo.");
                        propriedade.SetValue(entidade, _cryptography.CreateHash(valor));
                        break;
                    default:
                        propriedade.SetValue(entidade, kvp.Value);
                        break;
                }

                _context.Entry(entidade).Property(kvp.Key).IsModified = true;
            }

            await _context.SaveChangesAsync();
            return entidade;
        }

        public async Task<Questionario> UpdateQuestionario(Questionario questionario)
        {
            _context.Questionarios.Update(questionario);
            await _context.SaveChangesAsync();
            return questionario;
        }

        public bool ValidaCpf(string cpf)
        {
            if (cpf.Length != 11)
            {
                return false;
            }

            int[] multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

            string tempCpf = cpf[..9];
            int soma = 0;
            for (int i = 0; i < multiplicador1.Length; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            }
            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            string digito1 = resto.ToString();

            tempCpf += digito1;
            soma = 0;
            for (int i = 0; i < multiplicador2.Length; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            }
            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            string digito2 = resto.ToString();

            return cpf.EndsWith(digito1 + digito2);
        }
        public string LimpaDocumento(string documento)
        {
            return new string([.. documento.Where(char.IsDigit)]);
        }

        public async Task<ICollection<CriterioPlanilha>> GetCriterios(ICollection<int> ids)
        {
            var criterios = await _context.CriteriosPlanilha.Where(c => ids.Contains(c.Id)).Include(c => c.Itens).ToListAsync()
                ?? await _context.CriteriosPlanilha.Include(c => c.Itens).ToListAsync();
            return criterios;
        }

        public async Task<Usuario> GetUsuarioByEmail(string email)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email)
                ?? throw new NotFoundException("Usuário não encontrado.");
            return usuario;
        }

        public async Task<Usuario> GetUserFromClaims(HttpContext context)
        {
            var email = (context.User.FindFirst("Email")?.Value) ?? throw new UnauthorizedException("Usuário não autenticado.");
            return await GetUsuarioByEmail(email);
        }
    }
}