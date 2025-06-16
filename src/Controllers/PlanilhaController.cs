using AutoMapper;
using EcoScale.src.Data;
using EcoScale.src.Models.Abstract;
using EcoScale.src.Public.DTOs;
using EcoScale.src.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoScale.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanilhaController(AppDbContext context, IMapper mapper) : ControllerBase
    {
        private readonly PlanilhaService _planilhaService = new(context, mapper);

        /// <summary>
        /// A criação de uma nova planilha.
        /// </summary>
        /// <remarks>
        /// A criação de uma nova planilha requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <param name="request">Objeto do tipo PlanilhaCreationRequest com os dados para criação.</param>
        /// <response code="201">Indica que a planilha foi criada com sucesso.</response>
        /// <response code="400">Indica que os dados fornecidos são inválidos.</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        [HttpPost("new")]
        [ProducesResponseType(typeof(CreationResponse<Planilha>), StatusCodes.Status201Created)]
        [Authorize(Policy = "ModeradorPolicy")]
        public async Task<IActionResult> CreatePlanilha([FromBody] PlanilhaCreationRequest request)
        {
            CreationResponse<Planilha> res = await _planilhaService.New(request);
            return CreatedAtAction(nameof(CreatePlanilha), new { res });
        }

        /// <summary>
        /// Busca uma planilha pelo ID.
        /// </summary>
        /// <remarks>
        /// A criação de uma nova planilha requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <param name="Id">Id da planilha</param>
        /// <response code="200">Retorna a planilha</response>
        /// <response code="404">Nenhuma planilha encontrada com o Id fornecido</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Planilha), StatusCodes.Status200OK)]
        [Authorize(Policy = "ModeradorPolicy")]
        public async Task<IActionResult> GetPlanilha(int Id)
        {
            Planilha planilha = await _planilhaService.Get(Id);
            return Ok(planilha);
        }


        /// <summary>
        /// A criação de uma nova planilha.
        /// </summary>
        /// <remarks>
        /// A criação de uma nova planilha requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <param name="request">Objeto do tipo <see cref="PlanilhaUpdateRequest"/> com os dados atualizados da planilha</param>
        /// <response code="200">Retorna a planilha atualizada</response>
        /// <response code="404">Nenhuma planilha encontrada com o Id fornecido</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        [HttpPatch("patch")]
        [ProducesResponseType(typeof(Planilha), StatusCodes.Status200OK)]
        [Authorize(Policy = "ModeradorPolicy")]
        public async Task<IActionResult> PatchPlanilha(PlanilhaUpdateRequest request)
        {
            Planilha planilha = await _planilhaService.Patch(request);
            return Ok(planilha);
        }
    }
}