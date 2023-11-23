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
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;
        public CommentRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CommentImage(int userId, int imageId, string commentText)
        {
            var userComment = new UserComment()
            {
                UserId = userId,
                ImageId = imageId,
                CommentDate = DateTime.Now,
            };

            _context.UsersComments.Add(userComment);

            await Save();

            var comment = new Comment()
            {
                ImageId = imageId,
                UserId = userId,
                CommentDate = DateTime.Now,
                Text = commentText,
                UserComment = userComment,
                UserCommentId = userComment.UserCommentId
            };

            _context.Comments.Add(comment);
            return await Save();
        }

        public async Task<bool> DeleteCommentImage(int userId, int imageId, int userCommentId)
        {
            var userComment = await _context.UsersComments
                .FirstOrDefaultAsync(l => l.UserCommentId == userCommentId && l.UserId == userId && l.ImageId == imageId);

            var comment = await _context.Comments
                .FirstOrDefaultAsync(l => l.UserCommentId == userCommentId && l.UserId == userId && l.ImageId == imageId);

            if (comment == null || userComment == null)
            {
                return false;
            }

            _context.Comments.Remove(comment);
            _context.UsersComments.Remove(userComment);

            return await Save();
        }

        public async Task<Comment> GetCommentByUserCommentId(int userCommentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(uc => uc.UserCommentId == userCommentId);
            if (comment == null)
            {
                throw new Exception("Comment not found");
            }
            return comment;
        }

        public async Task<int> NumberOfCommentsPerImage(int imageId)
        {
            var numComments = await _context.UsersComments.CountAsync(i => i.ImageId == imageId);
            return numComments;
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateCommentImage(int userId, int imageId, int userCommentId, string newComment)
        {
            var commentToUpdate = await GetCommentByUserCommentId(userCommentId);

            commentToUpdate.Text = newComment;

            return await Save();

        }
    }
}