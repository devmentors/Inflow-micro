using Convey.CQRS.Commands;

namespace Inflow.Services.Customers.Core.Commands
{
    public record ProcessNotification(string Name, object Data) : ICommand;
}