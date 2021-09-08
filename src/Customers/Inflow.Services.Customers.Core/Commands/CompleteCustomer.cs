using System;
using Convey.CQRS.Commands;

namespace Inflow.Services.Customers.Core.Commands
{
    public record CompleteCustomer(Guid CustomerId, string Name, string FullName, string Address, string Nationality,
        string IdentityType, string IdentitySeries) : ICommand;
}