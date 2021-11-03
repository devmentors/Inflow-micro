using Convey.CQRS.Commands;

namespace Inflow.Services.Users.Core.Commands
{
    public record SubscribeNotification(string Name, string CallbackUrl) : ICommand;
}