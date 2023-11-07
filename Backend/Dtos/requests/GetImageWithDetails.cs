using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dtos.requests
{
    public class GetImageWithDetails
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public byte[] FileContentBase64 { get; set; } = Array.Empty<byte>();
    }
}