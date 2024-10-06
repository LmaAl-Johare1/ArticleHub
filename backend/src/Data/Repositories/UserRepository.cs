using Data.DbContexts;
using Data.Entities;
using Data.IRepositories;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    /// <summary>
    /// Repository for managing user-related data operations.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ArticleHubDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for data operations.</param>
        public UserRepository(ArticleHubDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Checks if a user exists by username.
        /// </summary>
        /// <param name="username">The username to check for existence.</param>
        /// <returns>A <see cref="Task{bool}"/> indicating whether the user exists.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="username"/> is null or empty.</exception>
        public async Task<bool> UserExistsAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            bool userExists = await _context.user.AnyAsync(u => u.username == username);
            return userExists;
        }

        /// <summary>
        /// Retrieves a user by username, including related data.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>A <see cref="Task{User}"/> containing the user entity if found; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="username"/> is null or empty.</exception>
        public async Task<User> GetUserAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            var user = await _context.user.Include(u => u.user_articles)
                                           .ThenInclude(u => u.article_likes)
                                           .Include(u => u.user_followings)
                                           .Include(u => u.user_followers)
                                           .Include(u => u.user_likes)
                                           .FirstOrDefaultAsync(u => u.username == username);
            return user;
        }

        /// <summary>
        /// Retrieves a user by username without tracking changes.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>A <see cref="Task{User}"/> containing the user entity if found; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="username"/> is null or empty.</exception>
        public async Task<User> GetUserAsNoTrackingAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            var user = await _context.user.AsNoTracking().Include(u => u.user_articles)
                                           .Include(u => u.user_followings)
                                           .Include(u => u.user_followers)
                                           .FirstOrDefaultAsync(u => u.username == username);
            return user;
        }

        /// <summary>
        /// Authenticates a user by checking email and password.
        /// </summary>
        /// <param name="user">The user data containing email and password for login.</param>
        /// <returns>A <see cref="Task{User}"/> representing the authenticated user if successful; otherwise, null.</returns>
        public async Task<User> LoginUserAsync(User user)
        {
            var LoginUser = await _context.user.FirstOrDefaultAsync(u => u.email == user.email
                                                             && u.password == user.password);
            return LoginUser;
        }

        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="user">The user entity to be created.</param>
        public async Task CreateUserAsync(User user)
        {
            await _context.user.AddAsync(user);
        }

        /// <summary>
        /// Updates an existing user with new information.
        /// </summary>
        /// <param name="username">The username of the user to update.</param>
        /// <param name="userForUpdate">The updated user data.</param>
        /// <returns>A <see cref="Task{User}"/> containing the updated user entity.</returns>
        public async Task<User> UpdateUser(string username, User userForUpdate)
        {
            var updatedUser = await GetUserAsync(username);

            if (!string.IsNullOrWhiteSpace(userForUpdate.first_name))
            {
                updatedUser.first_name = userForUpdate.first_name;
            }
            if (!string.IsNullOrWhiteSpace(userForUpdate.last_name))
            {
                updatedUser.last_name = userForUpdate.last_name;
            }
            if (!string.IsNullOrWhiteSpace(userForUpdate.username))
            {
                updatedUser.username = userForUpdate.username.ToLower();
            }
            if (!string.IsNullOrWhiteSpace(userForUpdate.email))
            {
                updatedUser.email = userForUpdate.email.ToLower();
            }
            if (!string.IsNullOrWhiteSpace(userForUpdate.bio))
            {
                updatedUser.bio = userForUpdate.bio;
            }

            return updatedUser;
        }

        /// <summary>
        /// Checks if an email is available for new user registration.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>A <see cref="Task{bool}"/> indicating whether the email is available.</returns>
        public async Task<bool> EmailAvailableAsync(string email)
        {
            var emailNotAvailable = await _context.user.Select(a => a.email).ContainsAsync(email);
            return emailNotAvailable;
        }

        /// <summary>
        /// Follows a user.
        /// </summary>
        /// <param name="currentUserId">The ID of the user who is following.</param>
        /// <param name="userToFollowId">The ID of the user to be followed.</param>
        public async Task FollowUserAsync(int currentUserId, int userToFollowId)
        {
            var timestamp = DateTime.UtcNow;
            var userFollower =
                new UserFollower { User_follower_id = currentUserId, user_followeing_id = userToFollowId, created = timestamp, updated = timestamp };
            await _context.user_follower.AddAsync(userFollower);
        }

        /// <summary>
        /// Unfollows a user.
        /// </summary>
        /// <param name="currentUserId">The ID of the user who is unfollowing.</param>
        /// <param name="userToUnfollowId">The ID of the user to be unfollowed.</param>
        public void UnfollowUser(int currentUserId, int userToUnfollowId)
        {
            var userFollower =
                new UserFollower { User_follower_id = currentUserId, user_followeing_id = userToUnfollowId };
            _context.user_follower.Remove(userFollower);
        }

        /// <summary>
        /// Checks if a user is followed by another user.
        /// </summary>
        /// <param name="FollowerId">The ID of the follower.</param>
        /// <param name="FolloweingId">The ID of the user being followed.</param>
        /// <returns>A <see cref="Task{bool}"/> indicating whether the user is followed.</returns>
        public async Task<bool> IsFollowedAsync(int FollowerId, int FolloweingId)
        {
            bool isFollowed =
               await _context.user_follower.AnyAsync(uf => uf.User_follower_id == FollowerId && uf.user_followeing_id == FolloweingId);
            return isFollowed;
        }

        /// <summary>
        /// Saves all changes made to the context.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
