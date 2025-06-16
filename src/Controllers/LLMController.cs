using AutoMapper;
using EcoScale.src.Data;
using EcoScale.src.Models;
using EcoScale.src.Public.DTOs;
using EcoScale.src.Public.DTOs.Responses;
using EcoScale.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcoScale.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LLMController(AppDbContext context, IMapper mapper) : ControllerBase
    {
        private readonly LLMService _service = new(context, mapper);

        /// <summary>
        /// Busca os critérios de avaliação que a empresa irá responder.
        /// </summary>
        /// <remarks>
        /// Os critérios estão hardcoded no LLMService. Basta trocar para a resposta da LLM.
        /// </remarks>
        [HttpPost("criterios")]
        [ProducesResponseType(typeof(ICollection<Criterio>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCriterios([FromBody] GetCriteriosRequest req)
        {
            var criterios = await _service.GetCriterios(req);
            return Ok(criterios);
        }

        /// <summary>
        /// Gera o relatório com base nas respostas da empresa.
        /// </summary>
        [HttpPost("resposta")]
        [ProducesResponseType(typeof(LLMResponse), 200)]
        public async Task<IActionResult> GetRespostaLLM(GetLLMResponseRequest req) {
            RelatorioResponse response = await _service.GetRespostaLLM(req);
            return Ok(response);
        }
    }
}