using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models.enums;

namespace Backend.Models.classes
{
    public class User
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

        [Required(ErrorMessage = "Gender is required.")]
        public UsersGenders Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "File Name is required.")]
        public string FileName { get; set; } = string.Empty;

        [Required(ErrorMessage = "File Content is required.")]
        public byte[] FileContentBase64 { get; set; } = Array.Empty<byte>();

        // Verification when a user register
        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }

        // Verification when reset password request
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string NewEmail { get; set; } = string.Empty;

        // Verification when chane email address request
        public string? EmailChangeToken { get; set; }
        public DateTime? EmailChangeTokenExpires { get; set; }

        // Navigation properties for relationships
        public ICollection<UserComment> UsersComments { get; set; } = new List<UserComment>(); // User's comments on images
        public ICollection<UserLike> UsersLikes { get; set; } = new List<UserLike>(); // User's likes on images
        public ICollection<Image> Images { get; set; } = new List<Image>(); // Images uploaded by the user
        public UsersRoles Role { get; set; }
    }
}