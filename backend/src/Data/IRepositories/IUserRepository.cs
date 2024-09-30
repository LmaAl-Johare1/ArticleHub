using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(string username);
        Task<User> GetUserAsNoTrackingAsync(string username);
        Task<User> LoginUserAsync(User user);
        Task CreateUserAsync(User user);
        Task<bool> EmailAvailableAsync(string email);
        Task SaveChangesAsync();
    }
}
