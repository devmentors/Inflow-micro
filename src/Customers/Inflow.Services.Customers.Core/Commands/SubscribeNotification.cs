using Convey.CQRS.Commands;

namespace Inflow.Services.Customers.Core.Commands
{
    public record SubscribeNotification(string Name, string CallbackUrl) : ICommand;
}