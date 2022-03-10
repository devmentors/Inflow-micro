using System;
using Convey.CQRS.Events;

namespace Inflow.Services.Customers.Core.Events;

public record CustomerLocked(Guid CustomerId) : IEvent;