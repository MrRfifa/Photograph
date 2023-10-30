
using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.requests
{
    public class ChangeNamesRequest
    {
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewFirstname { get; set; } = string.Empty;
        public string NewLastname { get; set; } = string.Empty;
    }
}