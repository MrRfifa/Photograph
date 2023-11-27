using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.classes
{
    public class Image
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime UploadDate { get; set; }

        // Property to store the uploaded file
        public ImageFile ImageFile { get; set; } = new ImageFile();

        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection<UserComment>? UsersComments { get; set; } // User's comments on images
        public ICollection<UserLike>? UsersLikes { get; set; } // User's likes on images
    }

}