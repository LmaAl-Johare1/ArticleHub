using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IUserService
    {
        Task<bool> EmailAvailableAsync(string email);
        Task<bool> UserExistsAsync(string username);
        Task<UserDto> LoginUserAsync(UserLoginDto userLogin);
        Task<UserDto> CreateUserAsync(UserForCreationDto userForCreation);
        Task<UserProfileDto> UpdateUserAsync(string username, UserForUpdateDto userForUpdateDto);
        Task<UserProfileDto> GetCurrentUserAsync();
        Task<int> GetCurrentUserIdAsync();
        Task<UserProfileDto> GetProfileAsync(string username);
        Task<bool> FollowUserAsync(string username);
        Task<bool> UnFollowUserAsync(string username);
    }
}