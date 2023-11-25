using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models.classes;

namespace Backend.Interfaces
{
    public interface ICommentRepository
    {
        public Task<bool> CommentImage(int userId, int imageId, string commentText);
        public Task<bool> DeleteCommentImage(int userId, int imageId, int userCommentId);
        public Task<bool> UpdateCommentImage(int userId, int imageId, int userCommentId, string newComment);
        public Task<int> NumberOfCommentsPerImage(int imageId);
        public Task<List<Comment>> GetCommentPerImage(int imageId);
        public Task<Comment> GetCommentByUserCommentId(int userCommentId);
        public Task<bool> Save();
    }
}