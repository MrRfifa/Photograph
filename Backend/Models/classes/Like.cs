using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.classes
{
    public class Like
    {
        public int Id { get; set; }

        // Foreign key to link the like to the user who made it
        public int UserId { get; set; }

        // Foreign key to link the like to the image it belongs to
        public int ImageId { get; set; }

        [DataType(DataType.Date)]
        public DateTime LikeDate { get; set; }
    }
}