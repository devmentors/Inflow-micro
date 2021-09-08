using Convey.CQRS.Events;

namespace Inflow.Services.Payments.Core.Infrastructure.Exceptions
{
    [Contract]
    public record PaymentsActionRejected(string Reason, string Code) : IRejectedEvent;
}