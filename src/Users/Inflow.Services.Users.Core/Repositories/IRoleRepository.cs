using System.Collections.Generic;
using System.Threading.Tasks;
using Inflow.Services.Users.Core.Entities;

namespace Inflow.Services.Users.Core.Repositories;

internal interface IRoleRepository
{
    Task<Role> GetAsync(string name);
    Task<IReadOnlyList<Role>> GetAllAsync();
    Task AddAsync(Role role);
}