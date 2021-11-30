using Convey.CQRS.Events;

namespace Inflow.Services.Customers.Core.Events;

[Contract]
public record CustomerActionRejected(string Reason, string Code) : IRejectedEvent;