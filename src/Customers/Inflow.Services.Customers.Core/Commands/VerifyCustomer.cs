using System;
using Convey.CQRS.Commands;

namespace Inflow.Services.Customers.Core.Commands;

public record VerifyCustomer(Guid CustomerId) : ICommand;