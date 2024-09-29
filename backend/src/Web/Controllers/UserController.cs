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
            _IUserService = userService ??
               throw new ArgumentNullException(nameof(userService));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserLoginDto userLogin)
        {
            var userDto = await _IUserService.LoginUserAsync(userLogin);
            if (userDto == null)
            {
                return Unauthorized(new { msg = "The email or the password is wrong" } );
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
                return NotFound(new { message = "email is exist" });
            }

            var userExists = await _IUserService.UserExistsAsync(userForCreation.username);
            if (userExists)
            {
                return NotFound(new { message = "username is exist" } );
            }

            var userDto = await _IUserService.CreateUserAsync(userForCreation);
            return Ok(userDto);
        }
    }
}