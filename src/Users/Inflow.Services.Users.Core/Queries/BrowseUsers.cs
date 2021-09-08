using Convey.CQRS.Queries;
using Inflow.Services.Users.Core.DTO;

namespace Inflow.Services.Users.Core.Queries
{
    public class BrowseUsers : PagedQueryBase, IQuery<PagedResult<UserDto>>
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string State { get; set; }
    }
}