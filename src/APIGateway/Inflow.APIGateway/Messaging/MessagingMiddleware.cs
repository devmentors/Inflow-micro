using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Convey.HTTP;
using Convey.MessageBrokers.RabbitMQ;
using Convey.MessageBrokers.RabbitMQ.Conventions;
using Inflow.APIGateway.Correlation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTracing;

namespace Inflow.APIGateway.Messaging
{
    internal sealed class MessagingMiddleware : IMiddleware
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new JsonStringEnumConverter()}
        };
        
        private static readonly ConcurrentDictionary<string, IConventions> Conventions = new();
        private readonly IRabbitMqClient _rabbitMqClient;
        private readonly RouteMatcher _routeMatcher;
        private readonly ITracer _tracer;
        private readonly ICorrelationContextBuilder _correlationContextBuilder;
        private readonly ICorrelationIdFactory _correlationIdFactory;
        private readonly ILogger<MessagingMiddleware> _logger;
        private readonly IDictionary<string, List<MessagingOptions.EndpointOptions>> _endpoints;

        public MessagingMiddleware(IRabbitMqClient rabbitMqClient, RouteMatcher routeMatcher, ITracer tracer,
            ICorrelationContextBuilder correlationContextBuilder, ICorrelationIdFactory correlationIdFactory,
            IOptions<MessagingOptions> messagingOptions, ILogger<MessagingMiddleware> logger)
        {
            _rabbitMqClient = rabbitMqClient;
            _routeMatcher = routeMatcher;
            _tracer = tracer;
            _correlationContextBuilder = correlationContextBuilder;
            _correlationIdFactory = correlationIdFactory;
            _logger = logger;
            _endpoints = messagingOptions.Value.Endpoints?.Any() is true
                ? messagingOptions.Value.Endpoints.GroupBy(e => e.Method.ToUpperInvariant())
                    .ToDictionary(e => e.Key, e => e.ToList())
                : new Dictionary<string, List<MessagingOptions.EndpointOptions>>();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!_endpoints.TryGetValue(context.Request.Method, out var endpoints))
            {
                await next(context);
                return;
            }

            foreach (var endpoint in endpoints)
            {
                var match = _routeMatcher.Match(endpoint.Path, context.Request.Path);
                if (match is null)
                {
                    continue;
                }

                var key = $"{endpoint.Exchange}:{endpoint.RoutingKey}";
                if (!Conventions.TryGetValue(key, out var conventions))
                {
                    conventions = new MessageConventions(typeof(object), endpoint.RoutingKey, endpoint.Exchange, null);
                    Conventions.TryAdd(key, conventions);
                }

                var spanContext = _tracer.ActiveSpan is null ? string.Empty : _tracer.ActiveSpan.Context.ToString();
                var messageId = Guid.NewGuid().ToString("N");
                var correlationId = _correlationIdFactory.Create();
                var resourceId = Guid.NewGuid().ToString("N");
                var correlationContext = _correlationContextBuilder.Build(context, correlationId, spanContext,
                    endpoint.RoutingKey, resourceId);
                var message = await context.Request.ReadFromJsonAsync<object>(SerializerOptions);
                _logger.LogInformation("Publishing a message with ID: {MessageId}, Correlation ID: {CorrelationId}...", messageId, correlationId);
                _rabbitMqClient.Send(message, conventions, messageId, correlationId, spanContext, correlationContext);
                context.Response.StatusCode = StatusCodes.Status202Accepted;
                return;
            }

            await next(context);
        }
    }
}