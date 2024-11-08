using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ptm_dev_test.Dtos;
using ptm_dev_test.Services.IServices;
using System.Net;

namespace ptm_dev_test.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class ExamesController : ControllerBase
    {
        private readonly IExamesService _examesService;

        public ExamesController(IExamesService examesService)
        {
            _examesService = examesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateExame([FromBody] ExamesDto exame)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { mensagem = "Usuário não autorizado para criar um exame." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { mensagem = "Os dados fornecidos são inválidos." });
            }

            try
            {
                var createdExame = await _examesService.CreateExameAsync(exame);
                return CreatedAtAction(nameof(GetExameById), new { id = createdExame.Id }, createdExame);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetExames([FromQuery] string? nome, [FromQuery] int? idade, [FromQuery] string? genero,
                                                   [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { mensagem = "Usuário não autorizado para visualizar exames." });
            }

            try
            {
                var paginatedExames = await _examesService.GetExamesAsync(nome, idade, genero, pageNumber, pageSize);
                return Ok(paginatedExames);
            }
            catch (DbUpdateException)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExameById(long id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { mensagem = "Usuário não autorizado para visualizar o exame." });
            }

            try
            {
                var exame = await _examesService.GetExameByIdAsync(id);
                if (exame == null)
                {
                    return NotFound(new { mensagem = "Exame não encontrado." });
                }

                return Ok(exame);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (DbUpdateException)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
