using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Inflow.Services.Users.Core.DAL;
using Inflow.Services.Users.Core.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Users.Core.Queries.Handlers
{
    internal sealed class GetUserHandler : IQueryHandler<GetUser, UserDetailsDto>
    {
        private readonly UsersDbContext _dbContext;

        public GetUserHandler(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDetailsDto> HandleAsync(GetUser query)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.Id == query.UserId);

            return user?.AsDetailsDto();
        }
    }
}