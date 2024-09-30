using Data.DbContexts;
using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ArticleHubDbContext _context;

        public UserRepository(ArticleHubDbContext context)
        {
            _context = context;
        }
        public async Task<bool> UserExistsAsync(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            bool userExists = await _context.user.AnyAsync(u => u.username == username);
            return userExists;
        }

        public async Task<User> GetUserAsync(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            var user = await _context.user.Include(u => u.user_articles)
                                           .Include(u => u.user_followings)
                                           .Include(u => u.user_followers)
                                           .Include(u=>u.user_likes)
                                           .FirstOrDefaultAsync(u => u.username == username);
            return user;
        }
        public async Task<User> GetUserAsNoTrackingAsync(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            var user = await _context.user.AsNoTracking().Include(u => u.user_articles)
                                           .Include(u => u.user_followings)
                                           .Include(u => u.user_followers)
                                           .FirstOrDefaultAsync(u => u.username == username);
            return user;
        }
        public async Task<User> LoginUserAsync(User user)
        {
            var LoginUser = await _context.user.FirstOrDefaultAsync(u => u.email == user.email
                                                             && u.password == user.password);
            return LoginUser;
        }
        public async Task CreateUserAsync(User user)
        {
            await _context.user.AddAsync(user);
        }
        public async Task<bool> EmailAvailableAsync(string email)
        {
            var emailNotAvailable = await _context.user.Select(a => a.email).ContainsAsync(email);
            return emailNotAvailable;
        }
        public async Task FollowUserAsync(int currentUserId, int userToFollowId)
        {
            var userFollower =
                new UserFollower { User_follower_id = currentUserId, user_followeing_id = userToFollowId };
            await _context.user_follower.AddAsync(userFollower);
        }
        public void UnfollowUser(int currentUserId, int userToUnfollowId)
        {
            var userFollower =
                new UserFollower { User_follower_id = currentUserId, user_followeing_id = userToUnfollowId };
            _context.user_follower.Remove(userFollower);
        }
        public async Task<bool> IsFollowedAsync(int FollowerId, int FolloweingId)
        {
            bool isFollowed =
               await _context.user_follower.AnyAsync(uf => uf.User_follower_id == FollowerId && uf.user_followeing_id == FolloweingId);
            return isFollowed;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}