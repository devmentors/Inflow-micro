using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Outbox;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Customers.Core.Services;

internal sealed class MessageBroker : IMessageBroker
{
    private readonly IBusPublisher _busPublisher;
    private readonly IMessageOutbox _messageOutbox;
    private readonly ILogger<IMessageBroker> _logger;

    public MessageBroker(IBusPublisher busPublisher, IMessageOutbox messageOutbox, ILogger<MessageBroker> logger)
    {
        _busPublisher = busPublisher;
        _messageOutbox = messageOutbox;
        _logger = logger;
    }

    public Task PublishAsync(params IEvent[] events) => PublishAsync(events?.AsEnumerable());

    private async Task PublishAsync(IEnumerable<IEvent> events)
    {
        if (events is null)
        {
            return;
        }

        foreach (var @event in events)
        {
            if (@event is null)
            {
                continue;
            }

            var messageName = @event.GetType().Name.Underscore();
            var messageId = Guid.NewGuid().ToString("N");
            _logger.LogInformation("Publishing an integration event: {MessageName}  [ID: {MessageId}]...",
                messageName, messageId);

            if (_messageOutbox.Enabled)
            {
                await _messageOutbox.SendAsync(@event, messageId: messageId);
                continue;
            }
            
            await _busPublisher.PublishAsync(@event, messageId);
        }
    }
}