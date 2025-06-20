using EcoScale.src.Data;
using EcoScale.src.Public.DTOs;
using EcoScale.src.Public.DTOs.Responses;

using EcoScale.src.Models;
using EcoScale.src.Services;
using EcoScale.src.Services.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace EcoScale.src.Controllers
{
    [ApiController]
    [Route("[controller]")] // define a rota com base no nome da classe, sem o controller, ou seja, se a classe se chama UserController, a rota será /user
    public class EmpresaController(AppDbContext context, IMapper mapper) : ControllerBase
    {

        private readonly EmpresaService _service = new(context, mapper);
        private readonly EmpresaHelper _helper = new(context);

        /// <summary>
        /// Cria uma nova instância de Empresa.
        /// </summary>
        /// <remarks>
        /// Este endpoint adiciona uma nova empresa no sistema, retornando o objeto criado. Para acessar o token JWT, é necessário confirmar o e-mail da empresa.
        /// O código de confirmação é enviado por e-mail após a criação da empresa. Para confirmar o e-mail, utilize o endpoint /auth/empresa/email-confirmation.
        /// </remarks>
        /// <param name="e">Objeto do tipo CompanyCreationRequest com os dados para criação.</param>
        /// <response code="201">Indica que a empresa foi criada com sucesso.</response>
        /// <response code="400">Indica que os dados fornecidos são inválidos.</response>
        /// <response code="409">Indica que o CNPJ ou email utilizado para cadastrar ja existe na base de dados.</response>
        [HttpPost("new")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CreationResponse<Empresa>), StatusCodes.Status201Created)]
        public async Task<IActionResult> New([FromBody] CompanyCreationRequest e)
        {
            CreationResponse<Empresa> response = await _service.New(e);
            return CreatedAtAction(nameof(New), new { response });
        }

        /// <summary>
        /// Recupera os dados da empresa com base no JWT.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição. A empresa é encontrada atraves do JWT
        /// </remarks>
        /// <returns>
        /// Retorna um objeto <see cref="IActionResult"/> que encapsula a entidade <see cref="Empresa"/> caso encontrada.
        /// </returns>
        /// <response code="200">Retorna a empresa correspondente ao CNPJ.</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        /// <response code="404">Empresa com o CNPJ especificado não foi encontrada.</response>
        [HttpGet("get")]
        [Authorize(Policy = "EmpresaPolicy")]
        [ProducesResponseType(typeof(Empresa), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            HttpContext context = HttpContext;
            Empresa e = await _helper.FindInClaims(context);
            return Ok(e);
        }

        /// <summary>
        /// Efetua uma exclusão lógica da empresa na base de dados.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <returns>
        /// Retorna um objeto <see cref="IActionResult"/> que encapsula a entidade <see cref="Empresa"/> caso encontrada.
        /// </returns>
        /// <response code="200">Retorna a empresa removida correspondente ao CNPJ.</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        /// <response code="404">Empresa com o CNPJ especificado não foi encontrada.</response>
        [HttpDelete("del")]
        [Authorize(Policy = "EmpresaPolicy")]
        [ProducesResponseType(typeof(Empresa), StatusCodes.Status200OK)]
        public async Task<IActionResult> Del()
        {
            HttpContext context = HttpContext;
            Empresa e = await _service.Del(context);
            return Ok(e);
        }

        /// <summary>
        /// Atualiza os dados da empresa (apenas aqueles que foram atualizados).
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <param name="request">Dados da empresa que foram atualizados.</param>
        /// <returns>
        /// Retorna um objeto <see cref="IActionResult"/> que encapsula a entidade <see cref="Empresa"/> caso encontrada.
        /// </returns>
        /// <response code="200">Retorna a empresa atualizada correspondente ao CNPJ.</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        /// <response code="404">Empresa com o CNPJ especificado não foi encontrada.</response>
        [HttpPatch("patch")]
        [Authorize(Policy = "EmpresaPolicy")]
        [ProducesResponseType(typeof(Empresa), StatusCodes.Status200OK)]
        public async Task<IActionResult> Patch([FromBody] CompanyUpdateRequest request)
        {
            HttpContext context = HttpContext;
            Empresa e = await _service.Patch(request, context);
            return Ok(e);
        }

        /// <summary>
        /// Solicita a avaliação de um relatório da empresa.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <response code="204">O pedido de avaliação foi criado.</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        /// <response code="400">A empresa não tem um relatório para ser avaliado.</response>
        /// <response code="404">Empresa não encontrada</response>
        [HttpPost("avaliacao/relatorio")]
        [Authorize(Policy = "EmpresaPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AvaliacaoRelatorio(SolicitarAvaliacaoRequest request)
        {
            HttpContext context = HttpContext;
            await _service.SolicitarAvaliacaoRelatorio(context, request);
            return NoContent();
        }

        /// <summary>
        /// Recupera o relatório da empresa com base no JWT.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <response code="200">O relatorio da empresa</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        /// <response code="404">Relatorio ou empresa não encontrada</response>
        [HttpGet("get/relatorio")]
        [Authorize(Policy = "EmpresaPolicy")]
        [ProducesResponseType(typeof(RelatorioResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRelatorio()
        {
            HttpContext context = HttpContext;
            RelatorioResponse relatorio = await _service.GetRelatorio(context);
            return Ok(relatorio);
        }
        /// <summary>
        /// Recupera o relatório da empresa com base no Id da empresa.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <response code="200">O relatorio da empresa</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        /// <response code="404">Relatorio ou empresa não encontrada</response>
        [HttpGet("get/relatorio{id}")]
        [Authorize(Policy = "EmpresaPolicy")]
        [ProducesResponseType(typeof(RelatorioResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRelatorioById(int id)
        {
            RelatorioResponse relatorio = await _service.GetRelatorioByEmpresaId(id);
            return Ok(relatorio);
        }
        /// <summary>
        /// Recupera os dados da empresa com base no Id.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <returns>
        /// Retorna um objeto <see cref="IActionResult"/> que encapsula a entidade <see cref="Empresa"/> caso encontrada.
        /// </returns>
        /// <response code="200">Retorna a empresa correspondente ao CNPJ.</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        /// <response code="404">Empresa com o CNPJ especificado não foi encontrada.</response>
        [HttpGet("get/{id}")]
        [Authorize(Policy = "EmpresaPolicy")]
        [ProducesResponseType(typeof(Empresa), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            Empresa e = await _service.GetById(id);
            return Ok(e);
        }
    }
}