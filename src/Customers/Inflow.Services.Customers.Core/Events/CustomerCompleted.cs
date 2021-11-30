using System;
using Convey.CQRS.Events;

namespace Inflow.Services.Customers.Core.Events;

[Contract]
public record CustomerCompleted(Guid CustomerId, string Name, string FullName, string Nationality) : IEvent;