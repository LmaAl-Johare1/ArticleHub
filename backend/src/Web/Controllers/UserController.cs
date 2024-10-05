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
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _IUserService;

        public UserController(IUserService userService)
        {
            _IUserService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserLoginDto userLogin)
        {
            var userDto = await _IUserService.LoginUserAsync(userLogin);
            if (userDto == null)
            {
                return Unauthorized(new { message = "The email or the password is wrong" } );
            }

            return Ok(userDto);
        }

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