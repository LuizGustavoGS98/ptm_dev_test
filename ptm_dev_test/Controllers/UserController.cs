using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ptm_dev_test.Dtos;
using ptm_dev_test.Services.IServices;
using System.Net;

namespace ptm_dev_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
        {
            try
            {
                var user = await _userService.AddUser(userDto);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { mensagem = "Erro ao tentar salvar o usuário no banco de dados.", detalhes = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { mensagem = "Ocorreu um erro inesperado ao adicionar o usuário.", detalhes = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _userService.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { mensagem = "Erro ao obter a lista de usuários.", detalhes = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(long id)
        {
            try
            {
                bool deleted = _userService.DeleteUser(id);
                return deleted ? NoContent() : NotFound(new { mensagem = "Usuário não encontrado." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { mensagem = "Erro ao tentar remover o usuário do banco de dados.", detalhes = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { mensagem = "Ocorreu um erro inesperado ao deletar o usuário.", detalhes = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(long id)
        {
            try
            {
                var user = _userService.GetUsers().Find(u => u.Id == id);
                if (user == null)
                    return NotFound(new { mensagem = "Usuário não encontrado." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { mensagem = "Erro ao obter o usuário.", detalhes = ex.Message });
            }
        }
    }
}
