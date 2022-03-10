using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Inflow.Services.Customers.Core.Events.External.Handlers;

internal sealed class SignedUpHandler : IEventHandler<SignedUp>
{
    public async Task HandleAsync(SignedUp @event, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
    }
}