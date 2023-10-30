using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        // Foreign key to link the comment to the user who made it
        public int UserId { get; set; }

        // Foreign key to link the comment to the image it belongs to
        public int ImageId { get; set; }
    }
}