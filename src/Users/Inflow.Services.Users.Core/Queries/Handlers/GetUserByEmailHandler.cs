using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Inflow.Services.Users.Core.DAL;
using Inflow.Services.Users.Core.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Users.Core.Queries.Handlers;

internal sealed class GetUserByEmailHandler : IQueryHandler<GetUserByEmail, UserDetailsDto>
{
    private readonly UsersDbContext _dbContext;

    public GetUserByEmailHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDetailsDto> HandleAsync(GetUserByEmail query, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .SingleOrDefaultAsync(x => x.Email == query.Email);

        return user?.AsDetailsDto();
    }
}