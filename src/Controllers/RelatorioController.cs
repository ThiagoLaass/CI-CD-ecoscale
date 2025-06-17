using AutoMapper;
using EcoScale.src.Data;
using EcoScale.src.Public.DTOs.Responses;
using EcoScale.src.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoScale.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController(AppDbContext context, IMapper mapper) : ControllerBase
    {
        private readonly RelatorioService _service = new(context, mapper);

        /// <summary>
        /// Recupera o relatório da empresa com base no Id da empresa.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <response code="200">O relatorio da empresa</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        /// <response code="404">Relatorio ou empresa não encontrada</response>
        [HttpGet("get/relatorio/{empresaId}")]
        [Authorize(Policy = "ModeradorPolicy")]
        [ProducesResponseType(typeof(RelatorioResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRelatorioById(int empresaId)
        {
            RelatorioResponse relatorio = await _service.GetRelatorioById(empresaId);
            return Ok(relatorio);
        }

        /// <summary>
        /// Recupera todos os relatórios de todas as empresas.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <response code="200">Todos os relatórios</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        /// <response code="404">Relatorio ou empresa não encontrada</response>
        [HttpGet("get/relatorio/all")]
        [Authorize(Policy = "ModeradorPolicy")]
        [ProducesResponseType(typeof(ICollection<RelatorioResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            ICollection<RelatorioResponse> relatorio = await _service.GetAllRelatorios();
            return Ok(relatorio);
        }
    }
}