using Convey.CQRS.Queries;
using Inflow.Services.Users.Core.DTO;

namespace Inflow.Services.Users.Core.Queries;

public class GetUserByEmail : IQuery<UserDetailsDto>
{
    public string Email { get; set; }
}