using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dtos.requests
{
    public class ChangeCommentRequest
    {
        public int UserCommentId { get; set; }
        public string NewComment { get; set; } = string.Empty;
    }
}