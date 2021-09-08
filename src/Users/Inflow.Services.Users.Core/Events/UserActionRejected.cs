using Convey.CQRS.Events;

namespace Inflow.Services.Users.Core.Events
{
    [Contract]
    public record UserActionRejected(string Reason, string Code) : IRejectedEvent;
}