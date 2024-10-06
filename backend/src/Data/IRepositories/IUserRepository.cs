using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.IRepositories
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(string username);
        Task<User> GetUserAsync(string username);
        Task<User> GetUserAsNoTrackingAsync(string username);
        Task<User> LoginUserAsync(User user);
        Task CreateUserAsync(User user);
        Task<User> UpdateUser(string username, User userForUpdate);
        Task<bool> EmailAvailableAsync(string email);
        Task FollowUserAsync(int currentUserId, int userToFollowId);
        void UnfollowUser(int currentUserId, int userToUnfollowId);
        Task<bool> IsFollowedAsync(int FollowerId, int FolloweingId);
        Task SaveChangesAsync();
    }
}
