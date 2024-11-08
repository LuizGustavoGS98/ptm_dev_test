using ptm_dev_test.Data;
using ptm_dev_test.Dtos;
using ptm_dev_test.Models;
using ptm_dev_test.Services.IServices;

namespace ptm_dev_test.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext context, ITokenService tokenService)
        {
            _dbContext = context;
            _tokenService = tokenService;
        }

        public string AuthenticateUser(UserDto user)
        {
            var userExists = _dbContext.Users.Any(u => u.UserName == user.UserName && u.Password == user.Password);
            if (!userExists) return null;

            return _tokenService.GenerateToken(user);
        }

        public async Task<UserModel> AddUser(UserDto userDto)
        {
            if (_dbContext.Users.Any(u => u.UserName == userDto.UserName))
                throw new InvalidOperationException("Usuário com o mesmo nome já existe.");

            var user = new UserModel
            {
                Id = new Random().NextInt64(),
                UserName = userDto.UserName,
                Password = userDto.Password,
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public List<UserModel> GetUsers()
        {
            return _dbContext.Users.ToList();
        }

        public bool DeleteUser(long id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return false;

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
