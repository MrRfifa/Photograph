using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface ILikeRepository
    {
        public Task<bool> LikeImage(int userId, int imageId);
        public Task<bool> UnlikeImage(int userId, int imageId);
        public Task<bool> LikedImage(int userId, int imageId);
        public Task<int> NumberOfLikesPerImage(int imageId);
        public Task<bool> Save();
    }
}