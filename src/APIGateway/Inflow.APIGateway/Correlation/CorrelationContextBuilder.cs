using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Inflow.APIGateway.Correlation
{
    internal sealed class CorrelationContextBuilder : ICorrelationContextBuilder
    {
        public CorrelationContext Build(HttpContext context, string correlationId, string spanContext,
            string name = null, string resourceId = null)
            => new()
            {
                CorrelationId = correlationId,
                Name = name ?? string.Empty,
                ResourceId = resourceId ?? string.Empty,
                SpanContext = spanContext ?? string.Empty,
                TraceId = context.TraceIdentifier,
                ConnectionId = context.Connection.Id,
                CreatedAt = DateTime.UtcNow,
                User = context.User.Identity is not null
                    ? new CorrelationContext.UserContext
                    {
                        Id = context.User.Identity.Name,
                        IsAuthenticated = context.User.Identity.IsAuthenticated,
                        Role = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
                        Claims = context.User.Claims.GroupBy(x => x.Type)
                            .ToDictionary(x => x.Key, x => x.Select(c => c.Value))
                    }
                    : new CorrelationContext.UserContext
                    {
                        Claims = new Dictionary<string, IEnumerable<string>>()
                    }
            };
    }
}