using ptm_dev_test.Dtos;
using ptm_dev_test.Models;

namespace ptm_dev_test.Services.IServices
{
    public interface ITokenService
    {
        string GenerateToken(UserDto usuario);
    }
}
