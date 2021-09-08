using System.Threading.Tasks;
using Convey.HTTP;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Inflow.APIGateway.Correlation
{
    internal sealed class LogContextMiddleware : IMiddleware
    {
        private readonly ICorrelationIdFactory _correlationIdFactory;

        public LogContextMiddleware(ICorrelationIdFactory correlationIdFactory)
        {
            _correlationIdFactory = correlationIdFactory;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var correlationId = _correlationIdFactory.Create();
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await next(context);
            }
        }
    }
}