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
            var token = await _IUserService.LoginUserAsync(userLogin);
            if (token == null)
            {
                return Unauthorized(new { msg = "The email or the password is wrong" } );
            }

            return Ok(new { token = token });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserForCreationDto userForCreation)
        {
            var emailNotAvailable = await _IUserService.EmailAvailableAsync(userForCreation.email);
            if (emailNotAvailable)
            {
                return NotFound("email Not Available");
            }

            var userExists = await _IUserService.UserExistsAsync(userForCreation.username);
            if (userExists)
            {
                return NotFound("user Exists");
            }

            var token = await _IUserService.CreateUserAsync(userForCreation);
            return Ok(new {token = token});
        }
    }
}