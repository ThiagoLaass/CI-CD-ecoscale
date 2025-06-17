using AutoMapper;
using EcoScale.src.Data;
using EcoScale.src.Models.Notifications;
using EcoScale.src.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoScale.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacaoController(AppDbContext context, IMapper mapper) : ControllerBase
    {
        private readonly NotificacaoService _service = new(context, mapper);

        /// <summary>
        /// Retorna todas as notificações de todos os usuários.
        /// </summary>
        /// <returns>Mensagem de teste</returns>
        [HttpGet("all")]
        [ProducesResponseType(typeof(ICollection<Notificacao>), StatusCodes.Status200OK)]
        public IActionResult All()
        {
            ICollection<Notificacao> notificacoes = _service.GetAll().Result;
            return Ok(notificacoes);
        }

        /// <summary>
        /// Retorna todas as notificações do usuário autenticado.
        /// </summary>
        /// <returns>Objetos de <see cref="Notificacao"/> do usuário de acordo com o token utilizado</returns>
        [HttpGet("all/user")]
        [Authorize(Policy = "CompanyPolicy")]
        [ProducesResponseType(typeof(ICollection<Notificacao>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AllByUser()
        {
            HttpContext context = HttpContext;
            ICollection<Notificacao> notificacoes = await _service.GetAllNotificacoesByUser(context);
            return Ok(notificacoes);
        }

        /// <summary>
        /// Marca uma notificação como lida pelo ID.
        /// </summary>
        /// <returns>Este endpoint não retorna nada</returns>
        [HttpPatch("read/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult MarkAsRead(int id)
        {
            HttpContext context = HttpContext;
            _service.MarkAsRead(context, id);
            return NoContent();
        }
    }
}