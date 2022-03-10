using Convey.CQRS.Events;

namespace Inflow.Services.Customers.Core.Events;

public record CustomerActionRejected(string Code, string Reason) : IRejectedEvent;