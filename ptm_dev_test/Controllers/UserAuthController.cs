using Microsoft.AspNetCore.Mvc;
using ptm_dev_test.Dtos;
using ptm_dev_test.Services.IServices;

namespace ptm_dev_test.Controllers
{
    [ApiController]
    [Route("api/authenticate")]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserAuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] UserDto userLogin)
        {
            try
            {
                if (userLogin == null || string.IsNullOrWhiteSpace(userLogin.UserName) || string.IsNullOrWhiteSpace(userLogin.Password))
                {
                    return BadRequest(new { mensagem = "Nome de usuário e senha são obrigatórios." });
                }

                var token = _userService.AuthenticateUser(userLogin);

                if (token == null)
                    return Unauthorized(new { mensagem = "Credenciais inválidas." });

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Ocorreu um erro ao processar sua solicitação.", detalhes = ex.Message });
            }
        }
    }
}
