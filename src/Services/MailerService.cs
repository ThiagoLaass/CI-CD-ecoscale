using MailKit.Net.Smtp;
using MailKit.Security;
using System.Net.Sockets;
using EcoScale.src.Middlewares.Exceptions;
using MimeKit;
namespace EcoScale.src.Services
{
    public class EmailSender(EmailSettings settings)
    {
        private readonly EmailSettings _settings = settings;

        /// <summary>
        /// Envia um e-mail de forma assíncrona utilizando as configurações especificadas no serviço.
        /// </summary>
        /// <param name="to">Endereço de e-mail do destinatário.</param>
        /// <param name="type">Assunto da mensagem, que também é utilizado para gerar o conteúdo HTML do corpo do e-mail.</param>
        /// <param name="validationNumer">Número de validação, quando aplicavel</param>
        /// <remarks>
        /// O método cria uma mensagem de e-mail em formato HTML e utiliza um cliente SMTP para conectar, autenticar, enviar o e-mail e desconectar.
        /// Caso ocorra uma exceção de soquete durante o envio, uma nova exceção é lançada com uma mensagem detalhada.
        /// </remarks>
        /// <exception cref="MailerException">É lançada quando ocorre uma falha na conexão ou envio do e-mail.</exception>
        /// <returns>Uma tarefa que representa a operação assíncrona de envio do e-mail.</returns>
        public async Task SendEmailAsync(string to, EmailTypes type, string? validationNumer) {
            var message = new MimeMessage();
            string htmlMessage;
            if(type.Equals(EmailTypes.CONFIRMAREMAIL)) {
                htmlMessage = GetHtmlBody(type, validationNumer);
            } else {
                htmlMessage = GetHtmlBody(type);
            }

            message.From.Add(new MailboxAddress(_settings.DisplayName, _settings.FromAddress));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = type switch {
                EmailTypes.CONFIRMAREMAIL => "Confirmação de E-mail - EcoScale",
                EmailTypes.RELATORIO      => "Solicitação de Verificação - EcoScale",
                _                         => "Bem-vindo à EcoScale",
            };

            message.Headers.Add("X-Mailer", "EcoScale Mailer");
            message.Headers.Add("X-Priority", "3");
            message.Body = new TextPart("html") { Text = htmlMessage };

            using var client = new SmtpClient();
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            
            try {
                await client.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_settings.Username, _settings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (SocketException ex) {
                throw new MailerException($"Houve um erro ao enviar o email: {ex.Message}");
            } catch (System.Net.Mail.SmtpException ex) {
                throw new MailerException($"Houve um erro ao enviar o email: {ex.Message}");
            } catch (Exception ex) {
                throw new MailerException($"Houve um erro ao enviar o email: {ex.Message}");
            }
        }

        private static string GetHtmlBody(EmailTypes type, string? verificationCode = null) {
            var modernStyle = @"
            <style type=""text/css"">
                @import url('https://fonts.googleapis.com/css?family=Poppins:400,600&display=swap');

                body {
                    margin: 0;
                    padding: 0;
                    background-color: #f7f7f7;
                    font-family: 'Poppins', Arial, sans-serif;
                    color: #333333;
                }

                .container {
                    width: 100%;
                    max-width: 600px;
                    margin: 40px auto;
                    background-color: #ffffff;
                    border-radius: 8px;
                    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.05);
                    overflow: hidden;
                }

                .header {
                    background-color: #4CAF50; /* cor de destaque para o topo */
                    text-align: center;
                    padding: 30px 20px;
                    color: #ffffff;
                }

                .header img {
                    max-width: 120px;
                    height: auto;
                    margin-bottom: 10px;
                }

                .header h1 {
                    margin: 0;
                    font-size: 24px;
                    font-weight: 600;
                }

                .content {
                    padding: 20px 30px;
                    line-height: 1.6;
                }

                .content p {
                    margin: 0 0 16px 0;
                }

                .btn {
                    display: inline-block;
                    background-color: #4CAF50;
                    color: #ffffff;
                    padding: 12px 24px;
                    text-decoration: none;
                    border-radius: 5px;
                    margin-top: 20px;
                    font-weight: 600;
                    transition: background-color 0.3s ease;
                }

                .btn:hover {
                    background-color: #43a047;
                }

                .footer {
                    background-color: #f5f5f5;
                    text-align: center;
                    font-size: 12px;
                    color: #888888;
                    padding: 20px;
                }

                @media (max-width: 600px) {
                    .container {
                        margin: 20px auto;
                        border-radius: 0;
                    }
                    .content {
                        padding: 20px;
                    }
                }
            </style>
            ";

            var avaliacaoEmail = $@"<!DOCTYPE html>
            <html>
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Verificação de Avaliação - EcoScale</title>
                {modernStyle}
            </head>
            <body>
                <div class=""container"">
                    <div class=""header"">
                        <img src=""cid:logo"" alt=""EcoScale Logo"">
                        <h1>Solicitação de Verificação</h1>
                    </div>
                    <div class=""content"">
                        <p>Olá,</p>
                        <p>Recebemos sua solicitação para a verificação da avaliação que nossa LLM forneceu para o resultado do formulário preenchido por sua empresa.</p>
                        <p>Nossa equipe já está analisando os dados e em breve entraremos em contato com mais detalhes sobre o processo de verificação.</p>
                        <p>Se precisar de mais informações ou tiver dúvidas, não hesite em nos contatar.</p>
                        <p>Obrigado por confiar na EcoScale!</p>
                    </div>
                    <div class=""footer"">
                        <p>EcoScale © 2025 - Todos os direitos reservados.</p>
                        <p>Este é um e-mail automático, por favor, não responda.</p>
                    </div>
                </div>
            </body>
            </html>";


            var defaultEmail = $@"<!DOCTYPE html>
            <html>
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>EcoScale - Formulário de Sustentabilidade</title>
                {modernStyle}
            </head>
            <body>
                <div class=""container"">
                    <div class=""header"">
                        <img src=""cid:logo"" alt=""EcoScale Logo"">
                        <h1>Bem-vindo à EcoScale</h1>
                    </div>
                    <div class=""content"">
                        <p>Olá,</p>
                        <p>Obrigado por se cadastrar em nosso formulário. Na <strong>EcoScale</strong>, nossa missão é ajudar empresas a mensurar e aprimorar sua sustentabilidade por meio de uma régua prática e personalizada.</p>
                        <p>Você está a um passo de transformar a gestão ambiental da sua empresa e promover ações que geram resultados positivos tanto para o meio ambiente quanto para o seu negócio.</p>
                        <p>Para continuar, clique no botão abaixo e preencha o formulário com as informações solicitadas:</p>
                        <p style=""text-align: center;"">
                            <a href=""https://ecoscale.example.com/formulario"" class=""btn"">Acessar Formulário</a>
                        </p>
                    </div>
                    <div class=""footer"">
                        <p>EcoScale © 2025 - Todos os direitos reservados.</p>
                        <p>Este é um e-mail automático, por favor, não responda.</p>
                    </div>
                </div>
            </body>
            </html>";

            var verificationEmail = $@"<!DOCTYPE html>
            <html>
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Código de Verificação - EcoScale</title>
                {modernStyle}
            </head>
            <body>
                <div class=""container"">
                    <div class=""header"">
                        <img src=""cid:logo"" alt=""EcoScale Logo"">
                        <h1>Código de Verificação</h1>
                    </div>
                    <div class=""content"">
                        <p>Olá,</p>
                        <p>Utilize o código abaixo para confirmar sua identidade e continuar com o processo de verificação:</p>
                        <span class=""code"">{verificationCode}</span>
                        <p>Se você não solicitou essa verificação, por favor, desconsidere este e-mail.</p>
                        <p>Obrigado,</p>
                        <p>Equipe EcoScale</p>
                    </div>
                    <div class=""footer"">
                        <p>EcoScale © 2025 - Todos os direitos reservados.</p>
                        <p>Este é um e-mail automático, por favor, não responda.</p>
                    </div>
                </div>
            </body>
            </html>";

            return type switch
            {
                EmailTypes.CONFIRMAREMAIL  => verificationEmail,
                EmailTypes.RELATORIO       => avaliacaoEmail,
                _                          => defaultEmail,
            };
        }
    }

    public class EmailSettings
    {
        public required string SmtpServer { get; set; }
        public required int Port { get; set; }
        public required string FromAddress { get; set; }
        public required string DisplayName { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public enum EmailTypes
    {
        CONFIRMAREMAIL,
        RELATORIO,
        RELATORIOAVALIADO
    }
}