using System.Threading.Tasks;
using Chronicle;
using Convey.CQRS.Events;
using Inflow.Saga.Api.Messages;

namespace Inflow.Saga.Api.Handlers
{
    internal sealed class SagaEventHandler :
        IEventHandler<SignedUp>,
        IEventHandler<CustomerCompleted>,
        IEventHandler<CustomerVerified>,
        IEventHandler<WalletAdded>,
        IEventHandler<DepositCompleted>,
        IEventHandler<FundsAdded>
    {
        private readonly ISagaCoordinator _sagaCoordinator;

        public SagaEventHandler(ISagaCoordinator sagaCoordinator)
        {
            _sagaCoordinator = sagaCoordinator;
        }

        public Task HandleAsync(SignedUp @event) => ProcessAsync(@event);

        public Task HandleAsync(CustomerCompleted @event) => ProcessAsync(@event);

        public Task HandleAsync(CustomerVerified @event) => ProcessAsync(@event);

        public Task HandleAsync(WalletAdded @event) => ProcessAsync(@event);

        public Task HandleAsync(DepositCompleted @event) => ProcessAsync(@event);

        public Task HandleAsync(FundsAdded @event) => ProcessAsync(@event);

        private Task ProcessAsync<T>(T message) where T : class
            => _sagaCoordinator.ProcessAsync(message, SagaContext.Empty);
    }
}