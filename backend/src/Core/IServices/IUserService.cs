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
        Task<string> LoginUserAsync(UserLoginDto userLogin);
        Task<string> CreateUserAsync(UserForCreationDto userForCreation);
        Task<UserProfileDto> GetCurrentUserAsync();
        Task<int> GetCurrentUserIdAsync();
        Task<UserProfileDto> GetProfileAsync(string username);
        Task<bool> FollowUserAsync(string username);
        Task<bool> UnFollowUserAsync(string username);
    }
}
