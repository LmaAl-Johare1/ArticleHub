using Core.IServices;
using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    /// <summary>
    /// Controller for handling user-related actions such as login, registration, and profile management.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _IUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The service responsible for user-related operations.</param>
        public UserController(IUserService userService)
        {
            _IUserService = userService;
        }

        /// <summary>
        /// Authenticates a user and returns user data.
        /// </summary>
        /// <param name="userLogin">The user's login data containing email and password.</param>
        /// <returns>A <see cref="Task{ActionResult{UserDto}}"/> containing user data if authentication succeeds; otherwise, an Unauthorized response.</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserLoginDto userLogin)
        {
            var userDto = await _IUserService.LoginUserAsync(userLogin);
            if (userDto == null)
            {
                return Unauthorized(new { message = "The email or the password is wrong" });
            }

            return Ok(userDto);
        }

        /// <summary>
        /// Creates a new user account.
        /// </summary>
        /// <param name="userForCreation">The data required to create a new user, including username and email.</param>
        /// <returns>A <see cref="Task{ActionResult{UserDto}}"/> containing the created user data; otherwise, a BadRequest response if email or username is already taken.</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserForCreationDto userForCreation)
        {
            var emailNotAvailable = await _IUserService.EmailAvailableAsync(userForCreation.email);
            if (emailNotAvailable)
            {
                return BadRequest(new { message = "email is exist" });
            }

            var userExists = await _IUserService.UserExistsAsync(userForCreation.username);
            if (userExists)
            {
                return BadRequest(new { message = "username is exists" });
            }

            var userDto = await _IUserService.CreateUserAsync(userForCreation);
            return Ok(userDto);
        }

        /// <summary>
        /// Retrieves the profile of a specific user by username.
        /// </summary>
        /// <param name="username">The username of the user whose profile is to be retrieved.</param>
        /// <returns>A <see cref="Task{ActionResult{UserProfileDto}}"/> containing the user's profile data if found; otherwise, a NotFound response.</returns>
        [HttpGet("{username}")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(string username)
        {
            var profileToReturn = await _IUserService.GetProfileAsync(username);
            if (profileToReturn == null)
            {
                return NotFound();
            }

            return Ok(new { profile = profileToReturn });
        }

        /// <summary>
        /// Updates the user information.
        /// </summary>
        /// <param name="userForUpdate">The updated user data.</param>
        /// <param name="username">The username of the user to update.</param>
        /// <returns>A <see cref="Task{ActionResult{UserDto}}"/> containing the updated user data; otherwise, a BadRequest or Unauthorized response.</returns>
        [HttpPut("{username}")]
        public async Task<ActionResult<UserDto>> UpdateUser(UserForUpdateDto userForUpdate, string username)
        {
            var currentUser = await _IUserService.GetCurrentUserAsync();
            if (currentUser.username != username)
            {
                return Unauthorized();
            }

            var emailNotAvailable = await _IUserService.EmailAvailableAsync(userForUpdate.email);
            if (currentUser.email != userForUpdate.email && emailNotAvailable)
            {
                return BadRequest(new { message = "email is exist" });
            }

            var userExists = await _IUserService.UserExistsAsync(userForUpdate.username);
            if (currentUser.username != userForUpdate.username && userExists)
            {
                return BadRequest(new { message = "username is exists" });
            }

            var updatedUserToReturn = await _IUserService.UpdateUserAsync(username, userForUpdate);
            return Ok(new { user = updatedUserToReturn });
        }

        /// <summary>
        /// Follows a user.
        /// </summary>
        /// <param name="username">The username of the user to follow.</param>
        /// <returns>A <see cref="Task{ActionResult}"/> indicating the result of the follow operation; returns a Created response if successful.</returns>
        [HttpPost("{username}/follow")]
        public async Task<ActionResult> FollowUser(string username)
        {
            var followedProfileToReturn = await _IUserService.FollowUserAsync(username);
            if (!followedProfileToReturn)
            {
                return BadRequest();
            }

            return new ObjectResult(new { profile = followedProfileToReturn }) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary>
        /// Unfollows a user.
        /// </summary>
        /// <param name="username">The username of the user to unfollow.</param>
        /// <returns>A <see cref="Task{ActionResult}"/> indicating the result of the unfollow operation; returns NoContent response if successful.</returns>
        [HttpDelete("{username}/follow")]
        public async Task<ActionResult> UnFollowUser(string username)
        {
            var unFollowedProfileToReturn = await _IUserService.UnFollowUserAsync(username);
            if (!unFollowedProfileToReturn)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
