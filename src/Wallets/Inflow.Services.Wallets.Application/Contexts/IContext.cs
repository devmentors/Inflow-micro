using System;

namespace Inflow.Services.Wallets.Application.Contexts;

public interface IContext
{
    Guid RequestId { get; }
    string CorrelationId { get; }
    string TraceId { get; }
    string IpAddress { get; }
    string UserAgent { get; }
    IIdentityContext Identity { get; }
}