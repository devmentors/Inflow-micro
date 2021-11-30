using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.HTTP;
using Convey.MessageBrokers;
using Convey.MessageBrokers.RabbitMQ;
using Inflow.Saga.Api.Infrastructure.Contexts;
using Inflow.Saga.Api.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace Inflow.Saga.Api.Services;

internal sealed class MessageBroker : IMessageBroker
{
    private const string DefaultSpanContextHeader = "span_context";
    private readonly IBusPublisher _busPublisher;
    private readonly ICorrelationContextAccessor _contextAccessor;
    private readonly IMessagePropertiesAccessor _messagePropertiesAccessor;
    private readonly ICorrelationIdFactory _correlationIdFactory;
    private readonly ITracer _tracer;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ILogger<IMessageBroker> _logger;
    private readonly string _spanContextHeader;

    public MessageBroker(IBusPublisher busPublisher, ICorrelationContextAccessor contextAccessor,
        IMessagePropertiesAccessor messagePropertiesAccessor, ICorrelationIdFactory correlationIdFactory,
        RabbitMqOptions options, ITracer tracer, IJsonSerializer jsonSerializer, ILogger<MessageBroker> logger)
    {
        _busPublisher = busPublisher;
        _contextAccessor = contextAccessor;
        _messagePropertiesAccessor = messagePropertiesAccessor;
        _correlationIdFactory = correlationIdFactory;
        _tracer = tracer;
        _jsonSerializer = jsonSerializer;
        _logger = logger;
        _spanContextHeader = string.IsNullOrWhiteSpace(options.SpanContextHeader)
            ? DefaultSpanContextHeader
            : options.SpanContextHeader;
    }

    public Task PublishAsync(params IEvent[] events) => PublishAsync(events?.AsEnumerable());

    public Task SendAsync(params ICommand[] commands) => PublishAsync(commands?.AsEnumerable());

    private async Task PublishAsync(IEnumerable<object> messages)
    {
        if (messages is null)
        {
            return;
        }

        var messageProperties = _messagePropertiesAccessor.MessageProperties;
        var correlationId = _correlationIdFactory.Create();
        var spanContext = GetSpanContext(messageProperties);
        if (string.IsNullOrWhiteSpace(spanContext))
        {
            spanContext = _tracer.ActiveSpan is null ? string.Empty : _tracer.ActiveSpan.Context.ToString();
        }

        var correlationContext = GetCorrelationContext();
        var headers = new Dictionary<string, object>();

        foreach (var message in messages)
        {
            if (message is null)
            {
                continue;
            }

            var messageName = message.GetType().Name.Underscore();
            var messageId = Guid.NewGuid().ToString("N");
            _logger.LogInformation("Publishing a message: {MessageName}  [ID: {MessageId}, Correlation ID: {CorrelationId}]...",
                messageName, messageId, correlationId);
            await _busPublisher.PublishAsync(message, messageId, correlationId, spanContext, correlationContext,
                headers);
        }
    }

    private CorrelationContext GetCorrelationContext()
    {
        if (_contextAccessor.CorrelationContext is null)
        {
            return null;
        }

        var payload = ((JsonElement)_contextAccessor.CorrelationContext).GetRawText();
        return _jsonSerializer.Deserialize<CorrelationContext>(payload);
    }

    private string GetSpanContext(IMessageProperties messageProperties)
    {
        if (messageProperties is null)
        {
            return string.Empty;
        }

        if (messageProperties.Headers.TryGetValue(_spanContextHeader, out var span) && span is byte[] spanBytes)
        {
            return Encoding.UTF8.GetString(spanBytes);
        }

        return string.Empty;
    }
}