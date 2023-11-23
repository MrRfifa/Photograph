using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.classes
{
    public class UserComment
    {
        public int UserCommentId { get; set; }
        public int UserId { get; set; }
        public int ImageId { get; set; }
        public DateTime CommentDate { get; set; }
        public User? User { get; set; }
        public Image? Image { get; set; }
    }
}