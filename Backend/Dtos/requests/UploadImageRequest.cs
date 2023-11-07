using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.requests
{
    public class UploadImageRequest
    {
        [Required]
        public string ImageTitle { get; set; } = string.Empty;
        [Required]
        public string ImageDescription { get; set; } = string.Empty;

        public IFormFile? file { get; set; }
    }
}