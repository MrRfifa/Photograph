using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IStatisticRepository
    {
        public Task<int> NumberOfCommentsDonePerUser(int userId);
        public Task<int> NumberOfLikesDonePerUser(int userId);
        public Task<int> NumberOfCommentsReceivedPerUser(int userId);
        public Task<int> NumberOfLikesReceivedPerUser(int userId);
    }
}