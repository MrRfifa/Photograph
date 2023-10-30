using Backend.Data;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models.classes;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly ITokenRepository _tokenRepository;

        public UserRepository(DataContext context, ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
            _context = context;
        }

        public async Task<bool> ChangeEmail(int userId, string newEmail, string currentPassword)
        {
            var userExists = await UserExists(userId);
            if (!userExists)
            {
                throw new UserNotFoundException($"User with ID {userId} not found");
            }

            var user = await GetUserById(userId);

            if (user is null)
            {
                throw new UserNotFoundException($"User with ID {userId} not found");
            }
            
            // Verify the user's password
            if (!_tokenRepository.VerifyPasswordHash(currentPassword, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Incorrect password. Email change request denied.");
            }

            user.NewEmail = newEmail;
            user.EmailChangeToken = await _tokenRepository.GenerateUniqueToken();
            user.EmailChangeTokenExpires = DateTime.Now.AddMinutes(15);

            return await Save();
        }


        public async Task<bool> ChangeNames(int userId, string newFirstname, string newLastname, string currentPassword)
        {
            var userExists = await UserExists(userId);
            if (!userExists)
            {
                throw new UserNotFoundException($"User with ID {userId} not found");
            }

            var user = await GetUserById(userId);

            if (user is null)
            {
                throw new UserNotFoundException($"User with ID {userId} not found");
            }

            // Verify the user's password
            if (!_tokenRepository.VerifyPasswordHash(currentPassword, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Incorrect password. Email change request denied.");
            }

            user.FirstName = newFirstname;
            user.LastName = newLastname;

            return await Save();
        }

        public async Task<bool> ChangePassword(int userId, string currentPassword, string newPassword, string confirmPassword)
        {
            var userExists = await UserExists(userId);
            if (!userExists)
            {
                throw new UserNotFoundException($"User with ID {userId} not found");
            }

            var user = await GetUserById(userId);

            if (user is null)
            {
                throw new UserNotFoundException($"User with ID {userId} not found");
            }
            if (!newPassword.Equals(confirmPassword))
            {
                throw new Exception("Incorrect passwords. Passwords do not match.");
            }
            // Verify the user's password
            if (!_tokenRepository.VerifyPasswordHash(currentPassword, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Incorrect password. Password change request denied.");
            }

            _tokenRepository.CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return await Save();
        }

        public async Task<bool> DeleteAccount(int userId, string currentPassword)
        {
            var userExists = await UserExists(userId);
            if (!userExists)
            {
                throw new UserNotFoundException($"User with ID {userId} not found");
            }
            var user = await GetUserById(userId);

            // Verify the user's password
            if (!_tokenRepository.VerifyPasswordHash(currentPassword, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Incorrect password. Account deletion denied.");
            }
            _context.Remove(user);
            return await Save();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                throw new UserNotFoundException($"User with email: {email} not found");
            }

            return user;
        }

        public async Task<User> GetUserByEmailChangeToken(string token)
        {
            var user = await _context.Users
                .Where(u => u.EmailChangeToken == token)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                throw new UserNotFoundException("User not found");
            }

            return user;
        }

        public async Task<User> GetUserById(int userId)
        {
            var userExists = await UserExists(userId);

            if (!userExists)
            {
                throw new UserNotFoundException($"User with ID {userId} not found");
            }

            var user = await _context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                throw new UserNotFoundException($"User with ID {userId} not found");
            }

            return user;
        }

        public async Task<ICollection<User>> GetUsers()
        {
            return await _context.Users.OrderBy(u => u.Id).ToListAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UserExists(int userId)
        {
            var userExists = await _context.Users.AnyAsync(a => a.Id == userId);
            if (!userExists)
            {
                throw new UserNotFoundException($"User with ID {userId} not found");
            }
            return true;
        }

    }
}