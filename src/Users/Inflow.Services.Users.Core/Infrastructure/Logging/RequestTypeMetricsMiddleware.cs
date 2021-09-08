using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.Types;
using Microsoft.AspNetCore.Http;
using Prometheus;

namespace Inflow.Services.Users.Core.Infrastructure.Logging
{
    internal sealed class RequestTypeMetricsMiddleware : IMiddleware
    {
        private static readonly ISet<string> IgnoredPaths = new HashSet<string>
        {
            "/metrics",
            "/ping",
            "/health"
        };

        private readonly Counter _totalRequests;
        private readonly string _service;

        public RequestTypeMetricsMiddleware(AppOptions appOptions)
        {
            _service = appOptions.Service;
            _totalRequests = Metrics.CreateCounter("total_requests", "Number of HTTP requests.", "method", "service");
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (IgnoredPaths.Contains(context.Request.Path))
            {
                return next(context);
            }

            _totalRequests.WithLabels(context.Request.Method, _service).Inc();

            return next(context);
        }
    }
}