using Convey.CQRS.Commands;

namespace Inflow.Services.Customers.Core.Commands;

public record CreateCustomer(string Email) : ICommand;