using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Interfaces;
using Backend.Models.classes;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DataContext _context;

        public LikeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> LikedImage(int userId, int imageId)
        {
            var userLike = await _context.UsersLikes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.ImageId == imageId);

            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.ImageId == imageId);
            if (like == null && userLike == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> LikeImage(int userId, int imageId)
        {
            var like = new Like()
            {
                ImageId = imageId,
                UserId = userId,
                LikeDate = DateTime.Now
            };

            var userLike = new UserLike()
            {
                UserId = userId,
                ImageId = imageId,
                LikeDate = DateTime.Now
            };
            _context.UsersLikes.Add(userLike);
            _context.Likes.Add(like);
            return await Save();
        }

        public async Task<int> NumberOfLikesPerImage(int imageId)
        {
            var imageExists = await _context.Images.AnyAsync(i => i.Id == imageId);

            if (!imageExists)
            {
                throw new Exception($"Image with ID {imageId} not found");
            }

            var numLikes = await _context.UsersLikes.CountAsync(i => i.ImageId == imageId);
            return numLikes;
        }


        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UnlikeImage(int userId, int imageId)
        {
            var userLike = await _context.UsersLikes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.ImageId == imageId);

            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.ImageId == imageId);

            if (like == null || userLike == null)
            {
                return false;
            }

            _context.Likes.Remove(like);
            _context.UsersLikes.Remove(userLike);

            return await Save();
        }
    }
}