using AutoMapper;
using Core.Authentication;
using Core.Models;
using Core.Utils;
using Data.Entities;
using Data.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _IUserRepository;
        private readonly IAuthentication _IAuthentication;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IAuthentication authentication,
            IHttpContextAccessor accessor, IMapper mapper)
        {
            _IUserRepository = userRepository;
            _IAuthentication = authentication;
            _accessor = accessor;
            _mapper = mapper;
        }

        public async Task<string> LoginUserAsync(UserLoginDto userLogin)
        {
            userLogin.email = userLogin.email.ToLower();
            userLogin.password = userLogin.password.GetHash();

            var user = _mapper.Map<User>(userLogin);
            var userlogedin = await _IUserRepository.LoginUserAsync(user);
            if (userlogedin == null)
            {
                return null;
            }

            var token = _IAuthentication.Generate(userlogedin);
            return token;
        }

        public async Task<UserDto> GetCurrentUserAsync()
        {
            var currentUsername = _accessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!String.IsNullOrEmpty(currentUsername))
            {
                var currentUser = await _IUserRepository.GetUserAsNoTrackingAsync(currentUsername);
                var userToReturn = _mapper.Map<UserDto>(currentUser);
                return userToReturn;
            }

            return null;
        }

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

        public async Task<string> CreateUserAsync(UserForCreationDto userForCreation)
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

            var token = _IAuthentication.Generate(userEntityForCreation);
            return token;
        }

        public async Task<bool> EmailAvailableAsync(string email)
        {
            var emailNotAvailable = await _IUserRepository.EmailAvailableAsync(email);
            return emailNotAvailable;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            var userExists = await _IUserRepository.UserExistsAsync(username);
            return userExists;
        }
    }
}