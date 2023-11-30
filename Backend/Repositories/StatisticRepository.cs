using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly DataContext _context;
        public StatisticRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> NumberOfCommentsDonePerUser(int userId)
        {
            try
            {
                var numComments = await _context.UsersComments.CountAsync(nc => nc.UserId == userId);
                return numComments;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<int> NumberOfCommentsReceivedPerUser(int userId)
        {
            try
            {
                int numComments = await _context.Images
         .Where(i => i.UserId == userId)
         .Join(
             _context.Comments,
             image => image.Id,
             comment => comment.ImageId,
             (image, comment) => comment.UserId)
         .CountAsync();

                return numComments;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<int> NumberOfLikesDonePerUser(int userId)
        {
            try
            {
                var numLikes = await _context.UsersLikes.CountAsync(nc => nc.UserId == userId);
                return numLikes;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<int> NumberOfLikesReceivedPerUser(int userId)
        {
            try
            {
                int numLikes = await _context.Images
                    .Where(i => i.UserId == userId)
                    .Join(
                        _context.Likes,
                        image => image.Id,
                        like => like.ImageId,
                        (image, like) => like.UserId)
                    .CountAsync();

                return numLikes;
            }
            catch (Exception)
            {
                return -1;
            }
        }

    }
}