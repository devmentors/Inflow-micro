using System.Threading.Tasks;
using Convey;
using Convey.CQRS.Commands;
using Convey.MessageBrokers;
using Convey.Types;
using Inflow.Services.Payments.Core.Contexts;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Payments.Core.Infrastructure.Decorators
{
    [Decorator]
    internal sealed class LoggingCommandHandlerDecorator<T> : ICommandHandler<T> where T : class, ICommand
    {
        private readonly ICommandHandler<T> _handler;
        private readonly IContext _context;
        private readonly IMessagePropertiesAccessor _messagePropertiesAccessor;
        private readonly ILogger<LoggingCommandHandlerDecorator<T>> _logger;

        public LoggingCommandHandlerDecorator(ICommandHandler<T> handler, IContext context,
            IMessagePropertiesAccessor messagePropertiesAccessor, ILogger<LoggingCommandHandlerDecorator<T>> logger)
        {
            _handler = handler;
            _context = context;
            _messagePropertiesAccessor = messagePropertiesAccessor;
            _logger = logger;
        }

        public async Task HandleAsync(T command)
        {
            var name = command.GetType().Name.Underscore();
            var requestId = _context.RequestId;
            var traceId = _context.TraceId;
            var userId = _context.Identity?.Id;
            var correlationId = _context.CorrelationId;
            var messageId = _messagePropertiesAccessor.MessageProperties?.MessageId;
            _logger.LogInformation("Handling a command: {Name} [Request ID: {RequestId}, Correlation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}', Message ID: '{MessageId}']...",
                name, requestId,  correlationId, traceId, userId, messageId);
            await _handler.HandleAsync(command);
            _logger.LogInformation("Handled a command: {Name} [Request ID: {RequestId}, Correlation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}', Message ID: '{MessageId}']",
                name, requestId,  correlationId, traceId, userId, messageId);
        }
    }
}