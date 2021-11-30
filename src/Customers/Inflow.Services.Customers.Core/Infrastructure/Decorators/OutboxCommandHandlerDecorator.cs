using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Outbox;
using Convey.Types;

namespace Inflow.Services.Customers.Core.Infrastructure.Decorators;

[Decorator]
internal sealed class OutboxCommandHandlerDecorator<T> : ICommandHandler<T> where T : class, ICommand
{
    private readonly ICommandHandler<T> _handler;
    private readonly IMessageOutbox _outbox;
    private readonly string _messageId;
    private readonly bool _enabled;

    public OutboxCommandHandlerDecorator(ICommandHandler<T> handler, IMessageOutbox outbox,
        OutboxOptions outboxOptions, IMessagePropertiesAccessor messagePropertiesAccessor)
    {
        _handler = handler;
        _outbox = outbox;
        _enabled = outboxOptions.Enabled;
        _messageId = messagePropertiesAccessor.MessageProperties?.MessageId;
    }

    public Task HandleAsync(T command, CancellationToken cancellationToken = default)
        => _enabled && !string.IsNullOrWhiteSpace(_messageId)
            ? _outbox.HandleAsync(_messageId, () => _handler.HandleAsync(command, cancellationToken))
            : _handler.HandleAsync(command, cancellationToken);
}