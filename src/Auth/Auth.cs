using AutoMapper;
using EcoScale.src.Data;
using EcoScale.src.Middlewares.Exceptions;
using EcoScale.src.Models;
using EcoScale.src.Models.Abstract;
using EcoScale.src.Models.Notifications;
using EcoScale.src.Public.DTOs;
using EcoScale.src.Public.Enum;
using EcoScale.src.Services;
using Microsoft.EntityFrameworkCore;

namespace EcoScale.src.Auth
{
    public class Auth (AppDbContext context)
    {
        private readonly AppDbContext _context = context;
        private readonly Cryptography _cryptography = new();
        private readonly Jwt _jwt = new();

        public async Task<LoginResponse> LoginAsync(LoginRequest request){
            var user = await _context.Usuarios.SingleOrDefaultAsync(u => u.Email == request.Email)
                ?? throw new NotFoundException("Usuário não encontrado.");
            if (!_cryptography.VerifyHash(user.Senha, request.Senha)) {
                throw new UnauthorizedAccessException("Senha inválida.");
            }

            if(!user.EmailConfirmado) {
                await ProcessLoginAsync(user, user.Email, user.Id.ToString(), request.Senha, user.Senha);
                return new LoginResponse {
                    Token  = null,
                };
            }
            return new LoginResponse {
                Token  = _jwt.GenerateToken(user.Email, user.Role == Role.Moderador),
            };
        }

        /// <summary>
        /// Processa a autenticação de um usuário (empresa ou moderador), verificando a senha,
        /// gerando um código de confirmação e enviando este código por email.
        /// </summary>
        /// <typeparam name="T">Tipo da entidade a ser autenticada.</typeparam>
        /// <param name="user">A entidade do usuário que está tentando realizar o login.</param>
        /// <param name="email">O email do usuário para envio do código de confirmação.</param>
        /// <param name="refId">Identificador único da entidade (pode ser CNPJ ou Id).</param>
        /// <param name="inputPassword">Senha informada pelo usuário na tentativa de login.</param>
        /// <param name="storedPasswordHash">Hash da senha armazenada para verificação.</param>
        /// <returns>A entidade autenticada do tipo <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidPasswordException">Lançada quando a senha informada é inválida.</exception>
        private async Task<T> ProcessLoginAsync<T>(
            T user,
            string email,
            string refId,
            string inputPassword,
            string storedPasswordHash)
        {
            if (!_cryptography.VerifyHash(storedPasswordHash, inputPassword)) {
                throw new InvalidPasswordException("Senha inválida.");
            }

            var validationNumber = _cryptography.GenerateSixDigitString();
            EmailSettings emailSettings = GetEmailSettings();
            var mailer = new EmailSender(emailSettings);
            await mailer.SendEmailAsync(email, EmailTypes.CONFIRMAREMAIL, validationNumber);

            var emailConfirmacao = new EmailConfirmacao {
                RefId = refId,
                Codigo = validationNumber,
                Confirmado = false,
            };

            _context.EmailConfirmacoes.Add(emailConfirmacao);
            _context.SaveChanges();

            return user;
        }

        /// <summary>
        /// Confirma o email de um usuário através da validação do token de confirmação.
        /// </summary>
        /// <param name="request">Objeto que contém os dados da requisição, incluindo a entidade e o código de confirmação.</param>
        /// <param name="refId">Função que extrai o identificador único (refId) da entidade.</param>
        /// <returns>Retorna um token JWT gerado para a entidade confirmada.</returns>
        /// <exception cref="NotFoundException">Lançada quando não é encontrada uma confirmação de email associada ao refId.</exception>
        /// <exception cref="InvalidTokenException">Lançada quando o código de confirmação fornecido não é válido.</exception>
        private string ConfirmarEmailInternal(EmailConfirmationRequest request, string refId)
        {
            var now = DateTime.UtcNow;
            var emailConfirmacao = _context.EmailConfirmacoes
                .Where(em => !em.Excluido
                    && em.RefId == refId
                    && !em.Confirmado
                    && em.ExpiresAt > now)
                .FirstOrDefault() 
                ?? throw new NotFoundException("Confirmação de email não encontrada ou já expirou.");

            if (!Equals(emailConfirmacao.Codigo, request.Codigo)) {
                throw new InvalidTokenException("Código inválido");
            }

            var user = _context.Usuarios
                .Where(u => u.Id.ToString() == refId)
                .FirstOrDefault() 
                ?? throw new NotFoundException("Usuário não encontrado.");

            emailConfirmacao.Confirmado = true;
            emailConfirmacao.Excluido = true;
            _context.SaveChanges();
            return _jwt.GenerateToken(user.Email, user.Role == Role.Moderador);
        }

        /// <summary>
        /// Confirma o email de uma empresa utilizando o código de validação recebido.
        /// </summary>
        /// <param name="request">Objeto contendo os dados da empresa e o código de confirmação.</param>
        /// <returns>Um token JWT gerado para a empresa após confirmação do email.</returns>
        /// <exception cref="NotFoundException">Lançada quando não é encontrada a confirmação de email associada ao CNPJ da empresa.</exception>
        /// <exception cref="ExpiredTokenException">Lançada quando o código de confirmação está expirado.</exception>
        /// <exception cref="InvalidTokenException">Lançada quando o código informado não é o mesmo que foi enviado.</exception>
        public string ConfirmarEmail(EmailConfirmationRequest request)
        {
            var user = _context.Usuarios.FirstOrDefault(emp => emp.Email == request.Email)
                     ?? throw new NotFoundException("Empresa não encontrada.");
            user.EmailConfirmado = true;
            _context.SaveChanges();
            return ConfirmarEmailInternal(new EmailConfirmationRequest { Email = request.Email, Codigo = request.Codigo }, user.Id.ToString());
        }

        public EmailSettings GetEmailSettings() {
            return new() {
                SmtpServer  = "smtp.gmail.com",
                Password    = Environment.GetEnvironmentVariable("MAIL_PSS")    ?? throw new ArgumentNullException("EmailSettings:Password", "Email Password não configurado"),
                Username    = Environment.GetEnvironmentVariable("MAIL_USERNAME") ?? throw new ArgumentNullException("EmailSettings:Username", "Email Username não configurado"),
                FromAddress = Environment.GetEnvironmentVariable("MAIL_FROM_ADDRS") ?? throw new ArgumentNullException("EmailSettings:FromAddress", "Email FromAddress não configurado"),
                DisplayName = Environment.GetEnvironmentVariable("MAIL_DISPLAY_NAME") ?? throw new ArgumentNullException("EmailSettings:DisplayName", "Email DisplayName não configurado"),
                Port        = int.Parse(Environment.GetEnvironmentVariable("MAIL_PORT") ?? throw new ArgumentNullException("EmailSettings:Port", "Email Port não configurado")),
            };
        }
        
        /// <summary>
        /// Envia um e-mail de confirmação contendo um número de verificação de seis dígitos para o endereço de e-mail informado.
        /// </summary>
        /// <param name="email">O endereço de e-mail do destinatário.</param>
        /// <param name="codigo">O código para validação</param>
        /// <returns>
        /// Retorna um objeto do tipo EmailConfirmacao que contém:
        ///   - RefId: o identificador da entidade,
        ///   - Codigo: o número de verificação enviado por e-mail,
        ///   - Confirmado: um valor booleano que inicialmente é falso,
        /// indicando que a confirmação ainda não foi realizada.
        /// </returns>
        /// <remarks>
        /// Este método gera um número de verificação usando um gerador criptográfico, envia o e-mail usando um serviço de envio de e-mails e constrói o objeto EmailConfirmacao para acompanhar o estado da confirmação.
        /// </remarks>
        public async void SendConfirmationEmail(string email, string codigo) {
            EmailSettings emailSettings = GetEmailSettings();
            var mailer = new EmailSender(emailSettings);
            await mailer.SendEmailAsync(email, EmailTypes.CONFIRMAREMAIL, codigo);
        }

        public EmailConfirmacao CreateEmailConfirmacao(string entityId) {
            string validationNumber = _cryptography.GenerateSixDigitString();
            return new EmailConfirmacao {
                RefId = entityId,
                Codigo = validationNumber,
                Confirmado = false,
            };
        }
    }
}

