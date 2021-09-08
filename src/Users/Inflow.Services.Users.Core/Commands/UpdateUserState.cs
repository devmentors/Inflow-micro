using System;
using Convey.CQRS.Commands;

namespace Inflow.Services.Users.Core.Commands
{
    public record UpdateUserState(Guid UserId, string State) : ICommand;
}