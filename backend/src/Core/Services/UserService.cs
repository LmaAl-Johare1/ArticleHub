using AutoMapper;
using Core.Authentication;
using Core.IServices;
using Core.Models;
using Core.Utils;
using Data.Entities;
using Data.IRepositories;
using Data.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Services
{
    /// <summary>
    /// Service class responsible for handling user-related operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _IUserRepository;
        private readonly IAuthentication _IAuthentication;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository for data access.</param>
        /// <param name="authentication">The authentication service for generating tokens.</param>
        /// <param name="accessor">Accessor for HTTP context, used to retrieve current user information.</param>
        /// <param name="mapper">AutoMapper instance for object mapping.</param>
        public UserService(IUserRepository userRepository, IAuthentication authentication,
            IHttpContextAccessor accessor, IMapper mapper)
        {
            _IUserRepository = userRepository;
            _IAuthentication = authentication;
            _accessor = accessor;
            _mapper = mapper;
        }

        /// <summary>
        /// Authenticates a user and returns their DTO if successful.
        /// </summary>
        /// <param name="userLogin">The user login details.</param>
        /// <returns>A <see cref="UserDto"/> containing user details and authentication token, or null if authentication fails.</returns>
        public async Task<UserDto> LoginUserAsync(UserLoginDto userLogin)
        {
            userLogin.email = userLogin.email.ToLower();
            userLogin.password = userLogin.password.GetHash();

            var user = _mapper.Map<User>(userLogin);
            var userlogedin = await _IUserRepository.LoginUserAsync(user);
            if (userlogedin == null)
            {
                return null;
            }

            var userDto = new UserDto
            {
                token = _IAuthentication.Generate(userlogedin),
                username = userlogedin.username
            };
            return userDto;
        }

        /// <summary>
        /// Retrieves the current user's profile information.
        /// </summary>
        /// <returns>A <see cref="UserProfileDto"/> containing the current user's profile data, or null if not found.</returns>
        public async Task<UserProfileDto> GetCurrentUserAsync()
        {
            var currentUsername = _accessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!String.IsNullOrEmpty(currentUsername))
            {
                var currentUser = await _IUserRepository.GetUserAsNoTrackingAsync(currentUsername);
                var userToReturn = _mapper.Map<UserProfileDto>(currentUser, a => a.Items["currentUserId"] = currentUser.id);
                return userToReturn;
            }

            return null;
        }

        /// <summary>
        /// Gets the ID of the currently logged-in user.
        /// </summary>
        /// <returns>The user's ID or -1 if not found.</returns>
        public async Task<int> GetCurrentUserIdAsync()
        {
            var currentUsername = _accessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!String.IsNullOrEmpty(currentUsername))
            {
                var currentUser = await _IUserRepository.GetUserAsNoTrackingAsync(currentUsername);
                return currentUser.id;
            }

            return -1;
        }

        /// <summary>
        /// Creates a new user with the provided details and returns their DTO.
        /// </summary>
        /// <param name="userForCreation">The user details for creation.</param>
        /// <returns>A <see cref="UserDto"/> containing the new user's details and authentication token.</returns>
        public async Task<UserDto> CreateUserAsync(UserForCreationDto userForCreation)
        {
            userForCreation.username = userForCreation.username.ToLower();
            userForCreation.email = userForCreation.email.ToLower();
            userForCreation.password = userForCreation.password.GetHash();
            var userEntityForCreation = _mapper.Map<User>(userForCreation);

            var timestamp = DateTime.UtcNow;
            userEntityForCreation.created = timestamp;
            userEntityForCreation.updated = timestamp;

            await _IUserRepository.CreateUserAsync(userEntityForCreation);
            await _IUserRepository.SaveChangesAsync();

            var userDto = new UserDto
            {
                token = _IAuthentication.Generate(userEntityForCreation),
                username = userEntityForCreation.username
            };

            return userDto;
        }

        /// <summary>
        /// Updates an existing user's details.
        /// </summary>
        /// <param name="username">The username of the user to update.</param>
        /// <param name="userForUpdate">The new user details.</param>
        /// <returns>A <see cref="UserProfileDto"/> containing the updated user's profile information.</returns>
        public async Task<UserProfileDto> UpdateUserAsync(string username, UserForUpdateDto userForUpdate)
        {
            var userEntityForUpdate = _mapper.Map<User>(userForUpdate);

            var timestamp = DateTime.UtcNow;
            userEntityForUpdate.updated = timestamp;

            var updatedUser = await _IUserRepository.UpdateUser(username, userEntityForUpdate);
            await _IUserRepository.SaveChangesAsync();

            var currentUserId = await GetCurrentUserIdAsync();
            var UpdatedUserToReturn = _mapper.Map<UserProfileDto>(updatedUser, a => a.Items["currentUserId"] = currentUserId);

            var articlesDto = new List<ArticleCardDto>();
            foreach (var article in updatedUser.user_articles)
            {
                var articleToReturn = _mapper.Map<ArticleCardDto>(article, a => a.Items["currentUserId"] = currentUserId);

                articlesDto.Add(articleToReturn);
            }

            UpdatedUserToReturn.articles = articlesDto;
            return UpdatedUserToReturn;
        }

        /// <summary>
        /// Checks if the provided email is available for registration.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email is available; otherwise, false.</returns>
        public async Task<bool> EmailAvailableAsync(string email)
        {
            var emailNotAvailable = await _IUserRepository.EmailAvailableAsync(email);
            return emailNotAvailable;
        }

        /// <summary>
        /// Checks if a user with the specified username exists.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>True if the user exists; otherwise, false.</returns>
        public async Task<bool> UserExistsAsync(string username)
        {
            var userExists = await _IUserRepository.UserExistsAsync(username);
            return userExists;
        }

        /// <summary>
        /// Retrieves a user's profile by their username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>A <see cref="UserProfileDto"/> containing the user's profile data, or null if not found.</returns>
        public async Task<UserProfileDto> GetProfileAsync(string username)
        {
            username = username.ToLower();

            var user = await _IUserRepository.GetUserAsync(username);
            if (user == null)
            {
                return null;
            }

            var currentUserId = await GetCurrentUserIdAsync();

            var profileToReturn = _mapper.Map<UserProfileDto>(user, a => a.Items["currentUserId"] = currentUserId);

            var articlesDto = new List<ArticleCardDto>();
            foreach (var article in user.user_articles)
            {
                var articleToReturn = _mapper.Map<ArticleCardDto>(article, a => a.Items["currentUserId"] = currentUserId);

                articlesDto.Add(articleToReturn);
            }

            profileToReturn.articles = articlesDto;
            return profileToReturn;
        }

        /// <summary>
        /// Follows a user with the specified username.
        /// </summary>
        /// <param name="username">The username of the user to follow.</param>
        /// <returns>True if the follow operation was successful; otherwise, false.</returns>
        public async Task<bool> FollowUserAsync(string username)
        {
            username = username.ToLower();

            var currentUser = await GetCurrentUserAsync();
            var currentUserId = await GetCurrentUserIdAsync();
            if (currentUser.username == username)
            {
                return false;
            }

            var userToFollow = await _IUserRepository.GetUserAsync(username);
            if (userToFollow == null)
            {
                return false;
            }

            bool isFollowed = await _IUserRepository.IsFollowedAsync(currentUserId, userToFollow.id);
            if (isFollowed)
            {
                return false;
            }

            await _IUserRepository.FollowUserAsync(currentUserId, userToFollow.id);
            await _IUserRepository.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Unfollows a user with the specified username.
        /// </summary>
        /// <param name="username">The username of the user to unfollow.</param>
        /// <returns>True if the unfollow operation was successful; otherwise, false.</returns>
        public async Task<bool> UnFollowUserAsync(string username)
        {
            username = username.ToLower();

            var currentUser = await GetCurrentUserAsync();
            var currentUserId = await GetCurrentUserIdAsync();
            if (currentUser.username == username)
            {
                return false;
            }

            var userToUnfollow = await _IUserRepository.GetUserAsNoTrackingAsync(username);
            if (userToUnfollow == null)
            {
                return false;
            }

            bool isFollowed = await _IUserRepository.IsFollowedAsync(currentUserId, userToUnfollow.id);
            if (!isFollowed)
            {
                return false;
            }

            _IUserRepository.UnfollowUser(currentUserId, userToUnfollow.id);
            await _IUserRepository.SaveChangesAsync();
            return true;
        }
    }
}
