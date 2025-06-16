using AutoMapper;
using EcoScale.src.Data;
using EcoScale.src.Middlewares.Exceptions;
using EcoScale.src.Models;
using EcoScale.src.Models.Abstract;
using EcoScale.src.Public.DTOs;
using EcoScale.src.Public.DTOs.Responses;
using EcoScale.src.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoScale.src.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionarioController(AppDbContext context, IMapper mapper) : ControllerBase
    {
        private readonly QuestionarioService _service = new(context, mapper);

        /// <summary>
        /// Cria um novo questionário.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// O campo 'Contexto' da empresa é preenchido automaticamente assim que o questionário é criado.
        /// Valores possíveis para os enums, e seus respectivos significados:
        ///
        /// Avaliacao:
        /// 
        ///   0 - NAO_FEITO  
        ///   1 - MAL_FEITO  
        ///   2 - FEITO  
        ///   3 - BEM_FEITO  
        ///   4 - NAO_APLICA  
        ///   5 - PENDENTE
        /// 
        /// Relevancia:
        /// 
        ///   0 - NAO_RELEVANTE  
        ///   1 - RELEVANTE  
        ///   2 - MUITO_RELEVANTE
        /// 
        /// </remarks>
        /// <param name="request">
        /// Objeto do tipo <see cref="QuestionarioCreationRequest"/> com os dados para criação.
        /// </param>
        /// <returns>
        /// Retorna um objeto <see cref="IActionResult"/> que encapsula a entidade <see cref="Questionario"/> criada.
        /// </returns>
        /// <response code="200">Retorna o questionário criado.</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        [ProducesResponseType(typeof(CreationResponse<Questionario>), StatusCodes.Status201Created)]
        [Authorize(Policy = "EmpresaPolicy")]
        [HttpPost("new")]
        public async Task<IActionResult> New([FromBody] GetCriteriosRequest request)
        {
            HttpContext context = HttpContext;
            QuestionarioResponse questionario = await _service.New(request, context);
            return CreatedAtAction(nameof(New), new { questionario });
        }

        /// <summary>
        /// Pesquisa por um questionario pelo id.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <param name="id">Id do questionario a ser pesquisado, recebido via query string.</param>
        /// <returns>
        /// Retorna um objeto <see cref="IActionResult"/> que encapsula a entidade <see cref="Questionario"/> caso encontrado.
        /// </returns>
        /// <response code="200">Retorna o empresa criado.</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        /// <response code="404">Questionario não encontrado.</response>
        [HttpGet("get/{id}")]
        [Authorize(Policy = "EmpresaPolicy")]
        [ProducesResponseType(typeof(Questionario), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            Questionario q = await _service.Get(id);
            return Ok(q);
        }

        /// <summary>
        /// Pesquisa por todos os questionarios.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <returns>
        /// Retorna um objeto <see cref="IActionResult"/> que encapsula uma coleção de entidades <see cref="Questionario"/>.
        /// </returns>
        /// <response code="200">Retorna todos os questionarios.</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        /// <response code="404">Nenhum questionario encontrado.</response>
        [HttpGet("get/all")]
        [Authorize(Policy = "EmpresaPolicy")]
        [ProducesResponseType(typeof(GetQuestionarioResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            GetQuestionarioResponse questionarios = await _service.GetAll();
            return Ok(questionarios);
        }

        /// <summary>
        /// Faz a remocão lógica de um questionario.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <param name="id">Id do questionario a ser removido, recebido via query string.</param>
        /// <returns>
        /// Retorna um objeto <see cref="IActionResult"/> que encapsula a entidade <see cref="Questionario"/> removido.
        /// </returns>
        /// <response code="200">O questionario removido.</response>
        /// <response code="401">Usuário não autorizado a acessar esta operação.</response>
        /// <response code="404">Questionario não encontrado.</response>
        [ProducesResponseType(typeof(Questionario), StatusCodes.Status200OK)]
        [Authorize(Policy = "ModeradorPolicy")]
        [HttpDelete("del/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Questionario q = await _service.Del(id);
            return Ok(q);
        }

        /// <summary>
        /// Busca criterios da planilha (o questionario é criado a partir dos dados da planilha) por um array de ids. A requisição é um post por questões de facilidade.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// O endpoint irá retornar todo o JSON das areas e temas em que o criterio está inserido.
        /// </remarks>
        /// <param name="request">Um campo contento o array de ids de criterios</param>
        /// <returns>
        /// Retorna um objeto <see cref="IActionResult"/> que encapsula as entidades <see cref="CriterioPlanilha"/> buscados.
        /// </returns>
        /// <response code="200">Os criterios</response>
        /// <response code="404">Nenhum criterio encontrado</response>
        [HttpPost("get/criterios")]
        [ProducesResponseType(typeof(ICollection<CriterioPlanilha>), StatusCodes.Status200OK)]
        [Authorize(Policy = "EmpresaPolicy")]
        public IActionResult GetCriterio(GetCriteriosByIdsRequest request)
        {
            ICollection<CriterioPlanilha> c = _service.GetCriterioByIds(request);
            return Ok(c);
        }

        /// <summary>
        /// Resposta em blocos de um questionario.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// Apenas os os objetos de <see cref="ItemAvaliadoUpdateRequest"/>, além do 'QuestionarioId' devem ser enviados. 
        /// </remarks>
        /// <param name="request">O objeto contendo o id do questionario e a lista de itens. <see cref="RespostaEmBlocosRequest"/></param>
        /// <returns>
        /// Este endpoint não retorna nada, apenas atualiza o questionario com as respostas enviadas.
        /// </returns>
        /// <response code="200">Questionario atualizado com sucesso</response>
        /// <response code="404">Questionario não encontrado</response>
        [HttpPatch("resposta-em-blocos")]
        [ProducesResponseType(typeof(Questionario), StatusCodes.Status200OK)]
        [Authorize(Policy = "EmpresaPolicy")]
        public async Task<IActionResult> RespostaEmBlocos([FromBody] RespostaEmBlocosRequest request)
        {
            await _service.RespostaEmBlocos(request);
            return Ok();
        }

        /// <summary>
        /// Finaliza um questionario.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// As respostas do questionario são enviadas para a LLM, que então gera o relatorio de sustentabilidade.
        /// </remarks>
        /// <param name="questionarioId">O id do questionario que foi finalizado.</param>
        /// <returns>
        /// O relatorio gerado pela LLM.
        /// </returns>
        /// <response code="200">Questionario finalizado, agora considerado como removido.</response>
        /// <response code="404">Questionario não encontrado</response>
        /// <response code="400">Nem todos os itens foram respondidos</response>
        /// <response code="500">Erro ao gerar o relatorio</response>
        [HttpGet("finalizar/{questionarioId}")]
        [ProducesResponseType(typeof(RelatorioResponse), StatusCodes.Status200OK)]
        [Authorize(Policy = "EmpresaPolicy")]
        public async Task<IActionResult> FinalizarQuestionario(int questionarioId)
        {
            HttpContext context = HttpContext;
            var relatorio = await _service.FinalizarQuestionario(questionarioId, context);
            return Ok(relatorio);
        }

        /// <summary>
        /// Busca um questionario pelo token do usuário
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <returns>
        /// O Questionario <see cref="Questionario"/>
        /// </returns>
        /// <response code="200">Questionario</response>
        /// <response code="404">Questionario não encontrado</response>
        [HttpGet("get/token")]
        [ProducesResponseType(typeof(RelatorioResponse), StatusCodes.Status200OK)]
        [Authorize(Policy = "EmpresaPolicy")]
        public async Task<IActionResult> GetByToken()
        {
            HttpContext context = HttpContext;
            var questionario = await _service.GetQuestionarioByToken(context);
            return Ok(questionario);
        }

        /// <summary>
        /// Busca um questionario pelo token do usuário, retornando possuiQuestionario = true ou false.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <returns>
        /// Json contendo possuiQuestionario, como false ou true
        /// </returns>
        /// <response code="200"></response>
        [HttpGet("get/token/bool")]
        [ProducesResponseType(typeof(RelatorioResponse), StatusCodes.Status200OK)]
        [Authorize(Policy = "EmpresaPolicy")]
        public async Task<IActionResult> GetByTokenBool()
        {
            HttpContext context = HttpContext;
            try {
                var questionario = await _service.GetQuestionarioByToken(context);
            } catch (NotFoundException){
                return Ok( new { possuiQuestionario = false } );
            }
            return Ok( new { possuiQuestionario = true } );
        }

        /// <summary>
        /// Retorna o questionario com os criterios que ainda não foram respondidos.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// Criterios que possuem pelo menos um item com resposta nula são retornados.
        /// </remarks>
        /// <returns>
        /// Questionario com os criterios que ainda não foram respondidos.
        /// </returns>
        /// <response code="200">Questionario com os criterios não respondidos</response>
        /// <response code="404">Questionario não encontrado</response>
        [HttpGet("get/criterios/faltantes")]
        [ProducesResponseType(typeof(ICollection<Criterio>), StatusCodes.Status200OK)]
        [Authorize(Policy = "EmpresaPolicy")]
        public async Task<IActionResult> GetCriteriosNaoRespondidos()
        {
            HttpContext context = HttpContext;
            var questionario = await _service.GetCriteriosNaoRespondidos(context);
            return Ok(questionario);
        }
        
        /// <summary>
        /// Retorna respondido = true ou false, indicando se o questionario foi respondido completamente.
        /// </summary>
        /// <remarks>
        /// Este endpoint requer autenticação por meio do envio do JWT válido no cabeçalho da requisição.
        /// </remarks>
        /// <returns>
        /// respondeu = true ou false, indicando se o questionario foi respondido completamente.
        /// </returns>
        /// <response code="200">respondido true|false</response>
        /// <response code="404">Questionario não encontrado</response>
        [HttpGet("respondeu-tudo")]
        [Authorize(Policy = "EmpresaPolicy")]
        public async Task<IActionResult> RespondeuTudo()
        {
            HttpContext context = HttpContext;
            var res = await _service.RespondeuTudo(context);
            return Ok( new { respondido = res } );
        }
    }
}
