using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRespository
    {
        Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId);
        Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams);
        Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
        void Deletelike(UserLike like);
        void AddLike(UserLike like);
        
    }
}
