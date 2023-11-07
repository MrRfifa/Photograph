using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models.classes;

namespace Backend.Interfaces
{
    public interface IUserRepository
    {
        //Get users
        Task<ICollection<User>> GetUsers();
        //Get users by
        Task<User> GetUserById(int userId);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByEmailChangeToken(string token);

        //User Existence
        Task<bool> UserExists(int userId);

        //User Changes
        Task<bool> DeleteAccount(int userId, string currentPassword);
        Task<bool> ChangeNames(int userId, string newFirstname, string newLastname, string currentPassword);
        Task<bool> ChangePassword(int userId, string currentPassword, string newPassword, string confirmPassword);
        Task<bool> ChangeEmail(int userId, string newEmail, string currentPassword);
        Task<bool> Save();
    }
}