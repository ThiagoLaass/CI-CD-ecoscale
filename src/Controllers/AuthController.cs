using EcoScale.src.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcoScale.src.Public.DTOs;

namespace EcoScale.src.Controllers
{
    [ApiController]

    [Route("[controller]")] // define a rota com base no nome da classe, sem o controller, ou seja, se a classe se chama UserController, a rota será /user
    public class AuthController(AppDbContext context) : ControllerBase{

        private readonly Auth.Auth _service = new(context);

        /// <summary>
        /// Login de um usuário (empresa ou moderador).
        /// </summary>
        /// <remarks>
        /// Caso seja o primeiro login da empresa, o token jwt na resposta estará nulo. É necessário informar o código enviado pelo e-mail (o código é valido por 30 minutos).
        /// A confirmarção do email é feita na rota email-confirmation, a rota retorna um token jwt.
        /// </remarks>
        /// <param name="request">Objeto contendo o email e a senha do usuário</param>
        /// <response code="200">Indica que o usuário foi encontrado e retornado com sucesso.</response>
        /// <response code="400">Indica que os dados fornecidos são inválidos.</response>
        /// <response code="404">Indica que o usuário não foi encontrado.</response>
        /// <response code="401">Indica que os dados fornecidos são incorretos.</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            LoginResponse response = await _service.LoginAsync(request);
            return Ok(new { response });
        }


        /// <summary>
        /// Confirma o e-mail de um usuário e retorna um token jwt.
        /// </summary>
        /// <param name="request">Objeto contendo os dados necessários para a confirmação do e-mail (email e código)</param>
        /// <returns>Um objeto IActionResult contendo um token encapsulado com o token jwt.</returns>
        /// <response code="200">Confirmação realizada com sucesso e token gerado.</response>
        /// <response code="400">Código incorreto.</response>
        /// <response code="400">Código expirado.</response>
        /// <response code="404">O email de confirmação não foi encontrado.</response>
        [AllowAnonymous]
        [HttpPost("email-confirmation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CompanyEmailConfirmation([FromBody] EmailConfirmationRequest request)
        {
            string token = _service.ConfirmarEmail(request);
            return Ok(new { token });
        }
    }
}