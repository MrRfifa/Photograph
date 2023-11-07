using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models.classes;
using Backend.Models.enums;

namespace Backend.Dtos
{
    public class GetUserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public byte[] FileContentBase64 { get; set; } = Array.Empty<byte>();
        // public UsersGenders Gender { get; set; }
        // public DateTime DateOfBirth { get; set; }
        // public UsersRoles Role { get; set; }
        // public ImageFile ProfileImage { get; set; } = new ImageFile();
    }
}