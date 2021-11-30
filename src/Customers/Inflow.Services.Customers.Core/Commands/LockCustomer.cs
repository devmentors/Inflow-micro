using System;
using Convey.CQRS.Commands;

namespace Inflow.Services.Customers.Core.Commands;

public record LockCustomer(Guid CustomerId, string Notes = null) : ICommand;