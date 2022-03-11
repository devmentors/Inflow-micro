using System.Threading;
using System.Threading.Tasks;
using Chronicle;
using Convey.CQRS.Events;
using Inflow.Saga.Api.Messages;

namespace Inflow.Saga.Api.Handlers;

internal sealed class SagaHandler : 
    IEventHandler<SignedUp>,
    IEventHandler<SignedIn>,
    IEventHandler<CustomerVerified>,
    IEventHandler<DepositCompleted>,
    IEventHandler<FundsAdded>,
    IEventHandler<WalletAdded>
{
    private readonly ISagaCoordinator _sagaCoordinator;

    public SagaHandler(ISagaCoordinator sagaCoordinator)
    {
        _sagaCoordinator = sagaCoordinator;
    }

    public Task HandleAsync(SignedUp @event, CancellationToken cancellationToken = default)
        => _sagaCoordinator.ProcessAsync(@event, SagaContext.Empty);

    public Task HandleAsync(SignedIn @event, CancellationToken cancellationToken = default)
        => _sagaCoordinator.ProcessAsync(@event, SagaContext.Empty);

    public Task HandleAsync(CustomerVerified @event, CancellationToken cancellationToken = default)
        => _sagaCoordinator.ProcessAsync(@event, SagaContext.Empty);

    public Task HandleAsync(DepositCompleted @event, CancellationToken cancellationToken = default)
        => _sagaCoordinator.ProcessAsync(@event, SagaContext.Empty);

    public Task HandleAsync(FundsAdded @event, CancellationToken cancellationToken = default)
        => _sagaCoordinator.ProcessAsync(@event, SagaContext.Empty);

    public Task HandleAsync(WalletAdded @event, CancellationToken cancellationToken = default)
        => _sagaCoordinator.ProcessAsync(@event, SagaContext.Empty);
}