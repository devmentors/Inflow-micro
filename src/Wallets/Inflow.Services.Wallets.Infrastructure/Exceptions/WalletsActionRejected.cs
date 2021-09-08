using Convey.CQRS.Events;
using Inflow.Services.Wallets.Application;

namespace Inflow.Services.Wallets.Infrastructure.Exceptions
{
    [Contract]
    public record WalletsActionRejected(string Reason, string Code) : IRejectedEvent;
}