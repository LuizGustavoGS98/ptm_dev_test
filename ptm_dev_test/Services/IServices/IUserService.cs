using ptm_dev_test.Dtos;
using ptm_dev_test.Models;

namespace ptm_dev_test.Services.IServices
{
    public interface IUserService
    {
        Task<UserModel> AddUser(UserDto user);
        bool DeleteUser(long id);
        string AuthenticateUser(UserDto user);
        List<UserModel> GetUsers();
    }
}
