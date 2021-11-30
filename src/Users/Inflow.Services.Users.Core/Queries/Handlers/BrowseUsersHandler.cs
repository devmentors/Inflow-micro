using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Inflow.Services.Users.Core.DAL;
using Inflow.Services.Users.Core.DTO;
using Inflow.Services.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Users.Core.Queries.Handlers;

internal sealed class BrowseUsersHandler : IQueryHandler<BrowseUsers, PagedResult<UserDto>>
{
    private readonly UsersDbContext _dbContext;

    public BrowseUsersHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<PagedResult<UserDto>> HandleAsync(BrowseUsers query, CancellationToken cancellationToken = default)
    {
        var users = _dbContext.Users.AsQueryable();
        if (!string.IsNullOrWhiteSpace(query.Email))
        {
            users = users.Where(x => x.Email == query.Email);
        }

        if (!string.IsNullOrWhiteSpace(query.Role))
        {
            users = users.Where(x => x.RoleId == query.Role);
        }

        if (!string.IsNullOrWhiteSpace(query.State) && Enum.TryParse<UserState>(query.State, true, out var state))
        {
            users = users.Where(x => x.State == state);
        }

        return users.AsNoTracking()
            .Include(x => x.Role)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => x.AsDto())
            .PaginateAsync(query, cancellationToken);
    }
}