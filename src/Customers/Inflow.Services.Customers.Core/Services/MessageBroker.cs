using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Convey;
using Convey.CQRS.Events;
using Convey.HTTP;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.RabbitMQ;
using Inflow.Services.Customers.Core.Contexts;
using Inflow.Services.Customers.Core.Infrastructure.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace Inflow.Services.Customers.Core.Services;

internal sealed class MessageBroker : IMessageBroker
{
    private const string DefaultSpanContextHeader = "span_context";
    private readonly IBusPublisher _busPublisher;
    private readonly IMessageOutbox _outbox;
    private readonly ICorrelationContextAccessor _contextAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMessagePropertiesAccessor _messagePropertiesAccessor;
    private readonly ICorrelationIdFactory _correlationIdFactory;
    private readonly ITracer _tracer;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ILogger<IMessageBroker> _logger;
    private readonly string _spanContextHeader;

    public MessageBroker(IBusPublisher busPublisher, IMessageOutbox outbox,
        ICorrelationContextAccessor contextAccessor, IHttpContextAccessor httpContextAccessor,
        IMessagePropertiesAccessor messagePropertiesAccessor, ICorrelationIdFactory correlationIdFactory,
        RabbitMqOptions options, ITracer tracer, IJsonSerializer jsonSerializer, ILogger<MessageBroker> logger)
    {
        _busPublisher = busPublisher;
        _outbox = outbox;
        _contextAccessor = contextAccessor;
        _httpContextAccessor = httpContextAccessor;
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

    private async Task PublishAsync(IEnumerable<IEvent> events)
    {
        if (events is null)
        {
            return;
        }

        var messageProperties = _messagePropertiesAccessor.MessageProperties;
        var originatedMessageId = messageProperties?.MessageId;
        var correlationId = _correlationIdFactory.Create();
        var spanContext = GetSpanContext(messageProperties);
        if (string.IsNullOrWhiteSpace(spanContext))
        {
            spanContext = _tracer.ActiveSpan is null ? string.Empty : _tracer.ActiveSpan.Context.ToString();
        }

        var correlationContext = GetCorrelationContext();
        var headers = new Dictionary<string, object>();

        foreach (var @event in events)
        {
            if (@event is null)
            {
                continue;
            }

            var messageName = @event.GetType().Name.Underscore();
            var messageId = Guid.NewGuid().ToString("N");
            _logger.LogInformation("Publishing an integration event: {MessageName}  [ID: {MessageId}, Correlation ID: {CorrelationId}]...",
                messageName, messageId, correlationId);
            if (_outbox.Enabled)
            {
                await _outbox.SendAsync(@event, originatedMessageId, messageId, correlationId, spanContext,
                    correlationContext, headers);
                continue;
            }

            await _busPublisher.PublishAsync(@event, messageId, correlationId, spanContext, correlationContext,
                headers);
        }
    }

    private CorrelationContext GetCorrelationContext()
    {
        CorrelationContext correlationContext;
        if (_contextAccessor.CorrelationContext is not null)
        {
            var payload = ((JsonElement)_contextAccessor.CorrelationContext).GetRawText();
            correlationContext = _jsonSerializer.Deserialize<CorrelationContext>(payload);
        }
        else
        {
            correlationContext = _httpContextAccessor.GetCorrelationContext();
        }

        return correlationContext;
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