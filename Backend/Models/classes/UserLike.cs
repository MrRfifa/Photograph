using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.classes
{
    public class UserLike
    {
        public int UserId { get; set; }
        public int ImageId { get; set; }
        public DateTime LikeDate { get; set; }
        public User? User { get; set; }
        public Image? Image { get; set; }
    }
}