using Backend.Dtos.requests;
using Backend.Models.classes;

namespace Backend.Interfaces
{
    public interface ITokenRepository
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public string CreateToken(User user);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);

        public Task<string> GenerateUniqueToken();
        public string CreateVerificationTokens();

        public void SendEmail(SendEmailRequest sendEmailRequest);
    }
}