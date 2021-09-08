using Convey.CQRS.Commands;

namespace Inflow.Services.Customers.Core.Commands
{
    [Contract]
    public record CreateCustomer(string Email) : ICommand;
}