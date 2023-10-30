using Backend.Dtos;
using Backend.Models.classes;

namespace Backend.Interfaces
{
    public interface IAuthRepository
    {
        public Task<string> Login(LoginUserDto userLogged);
        public Task<bool> Register(RegisterUserDto userCreated);
        Task<User> GetUserByVerificationToken(string token);
        Task<User> GetUserByResetToken(string token);
        public Task<bool> ForgetPassword(string email);
        public Task<bool> Save();
    }
}