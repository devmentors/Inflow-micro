using System;
using Convey.CQRS.Commands;

namespace Inflow.Services.Users.Core.Commands
{
    public record ChangePassword(Guid UserId, string CurrentPassword, string NewPassword) : ICommand;
}