using System;
using Convey.CQRS.Queries;
using Inflow.Services.Users.Core.DTO;

namespace Inflow.Services.Users.Core.Queries
{
    public class GetUser : IQuery<UserDetailsDto>
    {
        public Guid UserId { get; set; }
    }
}