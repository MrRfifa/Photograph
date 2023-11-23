using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.classes
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Comment text is required.")]
        public string Text { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime CommentDate { get; set; }
        public int UserId { get; set; }
        public int ImageId { get; set; }

        [ForeignKey("UserCommentId")]
        public int UserCommentId { get; set; }
        public UserComment? UserComment;
    }
}