using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.classes
{
    public class ImageFile
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "File Name is required.")]
        public string FileName { get; set; } = string.Empty;

        [Required(ErrorMessage = "File Content is required.")]
        public byte[]? FileContent { get; set; }
    }
}