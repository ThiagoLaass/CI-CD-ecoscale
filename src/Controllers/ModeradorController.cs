using AutoMapper;
using EcoScale.src.Data;
using EcoScale.src.Models;
using EcoScale.src.Public.DTOs;
using EcoScale.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcoScale.src.Controllers
{
    [ApiController]
    //[Authorize(Policy = "ModeradorPolicy")]
    [Route("api/[controller]")]
    public class ModeradorController(AppDbContext context, IMapper mapper) : ControllerBase
    {
        private readonly ModeradorService _moderadorService = new(context, mapper);

        /// <summary>
        /// A criação de um novo moderador.
        /// </summary>
        /// <remarks>
        /// A criação de um novo moderador requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <param name="request">Objeto do tipo ModeradorCreationRequest com os dados para criação.</param>
        /// <response code="201">Indica que o moderador foi criado com sucesso.</response>
        /// <response code="400">Indica que os dados fornecidos são inválidos.</response>
        /// <response code="409">Indica que o CPF ou email utilizado para cadastrar ja existe na base de dados.</response>
        /// <response code="500">Indica que ocorreu um erro interno no servidor.</response>
        /// <response code="401">Indica que o usuário não está autorizado a acessar este recurso.</response>
        [HttpPost("new")]
        // [AllowAnonymous]
        [ProducesResponseType(typeof(CreationResponse<Moderador>), StatusCodes.Status201Created)]
        public async Task<IActionResult> New([FromBody] ModeradorCreationRequest request)
        {
            CreationResponse<Moderador> response = await _moderadorService.New(request);
            return CreatedAtAction(nameof(New), new { response });
        }

        /// <summary>
        /// Recupera os dados do moderador com base no JWT.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição. O moderador é encontrado através do JWT.
        /// </remarks>
        /// <returns>
        /// Retorna um objeto <see cref="IActionResult"/> que encapsula a entidade <see cref="Moderador"/> caso encontrada.
        /// </returns>
        /// <response code="200">Retorna o moderador.</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        /// <response code="404">Moderador não encontrado.</response>
        [HttpGet("get")]
        [ProducesResponseType(typeof(Moderador), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            HttpContext context = HttpContext;
            Moderador moderador = await _moderadorService.Get(context);
            return Ok(moderador);
        }

        /// <summary>
        /// A avaliação de um relatório. Ainda em teste
        /// </summary>
        /// <remarks>
        /// A avaliação de um relatório requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <param name="request">Objeto do tipo AvaliarRelatorio com os dados para avaliação.</param>
        /// <response code="200">Indica que o relatório foi avaliado com sucesso.</response>
        /// <response code="400">Indica que os dados fornecidos são inválidos.</response>
        /// <response code="404">Indica que o relatório não foi encontrado.</response>
        /// <response code="500">Indica que ocorreu um erro interno no servidor.</response>
        /// <response code="401">Indica que o usuário não está autorizado a acessar este recurso.</response>
        [HttpPost("avaliacao/avaliar")]
        [ProducesResponseType(typeof(Relatorio), StatusCodes.Status200OK)]
        public async Task<IActionResult> Avaliar([FromBody] AvaliarRelatorio request)
        {
            HttpContext context = HttpContext;
            Relatorio r = await _moderadorService.AvaliarRelatorio(context, request);
            return Ok(r);
        }
    }
}