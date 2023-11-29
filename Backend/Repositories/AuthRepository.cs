using Backend.Data;
using Backend.Dtos;
using Backend.Dtos.requests;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models.classes;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;

namespace Backend.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;


        public AuthRepository(DataContext context, IUserRepository userRepository, ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _context = context;
        }


        public async Task<bool> ForgetPassword(string email)
        {

            var user = await _userRepository.GetUserByEmail(email);
            user.PasswordResetToken = await _tokenRepository.GenerateUniqueToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            return await Save();
        }

        public async Task<User> GetUserByResetToken(string token)
        {
            var user = await _context.Users
                .Where(u => u.PasswordResetToken == token)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                throw new UserNotFoundException("User not found");
            }

            return user;
        }

        public async Task<User> GetUserByVerificationToken(string token)
        {
            var user = await _context.Users
                .Where(u => u.VerificationToken == token)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                throw new UserNotFoundException("User not found");
            }

            return user;
        }

        public async Task<string> Login(LoginUserDto userLogged)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(userLogged.Email);

                if (user.VerifiedAt == null)
                {
                    throw new Exception("Not verified.");
                }

                if (!_tokenRepository.VerifyPasswordHash(userLogged.Password, user.PasswordHash, user.PasswordSalt))
                {
                    throw new Exception("Wrong password.");
                }

                string token = _tokenRepository.CreateToken(user);

                return token;
            }
            catch (UserNotFoundException ex)
            {
                throw ex;
            }
        }


        public async Task<bool> Register(RegisterUserDto userCreated)
        {
            try
            {
                _tokenRepository.CreatePasswordHash(userCreated.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var userEntity = new User
                {
                    LastName = userCreated.LastName,
                    FirstName = userCreated.FirstName,
                    Email = userCreated.Email,
                    Gender = userCreated.Gender,
                    DateOfBirth = userCreated.DateOfBirth,
                    Role = userCreated.Role,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    VerificationToken = await _tokenRepository.GenerateUniqueToken()
                };

                _context.Users.Add(userEntity);

                return await Save();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

    }
}