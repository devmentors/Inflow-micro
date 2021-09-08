using System.Threading.Tasks;
using Convey;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.Types;
using Inflow.Services.Customers.Core.Contexts;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Customers.Core.Infrastructure.Decorators
{
    [Decorator]
    internal sealed class LoggingEventHandlerDecorator<T> : IEventHandler<T> where T : class, IEvent
    {
        private readonly IEventHandler<T> _handler;
        private readonly IContext _context;
        private readonly IMessagePropertiesAccessor _messagePropertiesAccessor;
        private readonly ILogger<LoggingEventHandlerDecorator<T>> _logger;

        public LoggingEventHandlerDecorator(IEventHandler<T> handler, IContext context,
            IMessagePropertiesAccessor messagePropertiesAccessor, ILogger<LoggingEventHandlerDecorator<T>> logger)
        {
            _handler = handler;
            _context = context;
            _messagePropertiesAccessor = messagePropertiesAccessor;
            _logger = logger;
        }

        public async Task HandleAsync(T @event)
        {
            var name = @event.GetType().Name.Underscore();
            var requestId = _context.RequestId;
            var traceId = _context.TraceId;
            var userId = _context.Identity?.Id;
            var messageId = _messagePropertiesAccessor.MessageProperties?.MessageId;
            var correlationId = _messagePropertiesAccessor?.MessageProperties?.CorrelationId ?? _context.CorrelationId;
            _logger.LogInformation("Handling an event: {Name} [Request ID: {RequestId}, Correlation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}', Message ID: '{MessageId}']...",
                name, requestId,  correlationId, traceId, userId, messageId);
            await _handler.HandleAsync(@event);
            _logger.LogInformation("Handled an event: {Name} [Request ID: {RequestId}, Correlation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}', Message ID: '{MessageId}']",
                name, requestId,  correlationId, traceId, userId, messageId);
        }
    }
}