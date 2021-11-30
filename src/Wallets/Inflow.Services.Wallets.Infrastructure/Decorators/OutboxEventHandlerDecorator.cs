using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Outbox;
using Convey.Types;

namespace Inflow.Services.Wallets.Infrastructure.Decorators;

[Decorator]
internal sealed class OutboxEventHandlerDecorator<T> : IEventHandler<T> where T : class, IEvent
{
    private readonly IEventHandler<T> _handler;
    private readonly IMessageOutbox _outbox;
    private readonly string _messageId;
    private readonly bool _enabled;

    public OutboxEventHandlerDecorator(IEventHandler<T> handler, IMessageOutbox outbox,
        OutboxOptions outboxOptions, IMessagePropertiesAccessor messagePropertiesAccessor)
    {
        _handler = handler;
        _outbox = outbox;
        _enabled = outboxOptions.Enabled;
        _messageId = messagePropertiesAccessor.MessageProperties?.MessageId;
    }

    public Task HandleAsync(T @event, CancellationToken cancellationToken = default)
        => _enabled && !string.IsNullOrWhiteSpace(_messageId)
            ? _outbox.HandleAsync(_messageId, () => _handler.HandleAsync(@event, cancellationToken))
            : _handler.HandleAsync(@event, cancellationToken);
}