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
        Task<UserDto> GetCurrentUserAsync();
        Task<int> GetCurrentUserIdAsync();
    }
}
