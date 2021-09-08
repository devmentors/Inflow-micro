using Convey.HTTP;
using Convey.MessageBrokers;
using Inflow.Services.Customers.Core.Infrastructure.Serialization;
using Microsoft.AspNetCore.Http;

namespace Inflow.Services.Customers.Core.Contexts
{
    internal sealed class ContextFactory : IContextFactory
    {
        private readonly ICorrelationContextAccessor _contextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICorrelationIdFactory _correlationIdFactory;
        private readonly IJsonSerializer _jsonSerializer;

        public ContextFactory(ICorrelationContextAccessor contextAccessor, IHttpContextAccessor httpContextAccessor,
            ICorrelationIdFactory correlationIdFactory, IJsonSerializer jsonSerializer)
        {
            _contextAccessor = contextAccessor;
            _httpContextAccessor = httpContextAccessor;
            _correlationIdFactory = correlationIdFactory;
            _jsonSerializer = jsonSerializer;
        }

        public IContext Create()
        {
            var correlationId = _correlationIdFactory.Create();
            if (_contextAccessor.CorrelationContext is not null)
            {
                var payload = _jsonSerializer.Serialize(_contextAccessor.CorrelationContext);

                return string.IsNullOrWhiteSpace(payload)
                    ? new Context(correlationId)
                    : new Context(_jsonSerializer.Deserialize<CorrelationContext>(payload));
            }

            var correlationContext = _httpContextAccessor.GetCorrelationContext();
            if (correlationContext is not null)
            {
                return new Context(correlationContext);
            }

            var httpContext = _httpContextAccessor.HttpContext;

            return httpContext is not null ? new Context(httpContext) : new Context(correlationId);
        }
    }
}