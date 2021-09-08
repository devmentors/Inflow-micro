using System;
using System.Threading.Tasks;
using Inflow.Services.Users.Core.Entities;

namespace Inflow.Services.Users.Core.Repositories
{
    internal interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}