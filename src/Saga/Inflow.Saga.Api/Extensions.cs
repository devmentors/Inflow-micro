using System.Runtime.CompilerServices;
using Chronicle;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.Prometheus;
using Convey.Security;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi.CQRS;
using Convey.WebApi.Security;
using Inflow.Saga.Api.Infrastructure;
using Inflow.Saga.Api.Infrastructure.Serialization;
using Inflow.Saga.Api.Messages;
using Inflow.Saga.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Inflow.Saga.Api;

internal static class Extensions
{
    public static IConveyBuilder AddCore(this IConveyBuilder builder)
    {
        builder
            .AddCommandHandlers()
            .AddEventHandlers()
            .AddQueryHandlers()
            .AddInMemoryCommandDispatcher()
            .AddInMemoryEventDispatcher()
            .AddInMemoryQueryDispatcher()
            .AddInMemoryDispatcher()
            .AddHttpClient()
            .AddConsul()
            .AddFabio()
            .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
            .AddPrometheus()
            .AddJaeger()
            .AddCertificateAuthentication()
            .AddSecurity()
            .Build();

        builder
            .Services
            .AddChronicle()
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
            .AddSingleton<IJsonSerializer, SystemTextJsonSerializer>()
            .AddScoped<IMessageBroker, MessageBroker>()
            .AddSingleton<IClock, UtcClock>()
            .AddSingleton<ICorrelationIdFactory, CorrelationIdFactory>()
            .AddAuthorization(authorization =>
            {
                authorization.AddPolicy("deposits", x => x.RequireClaim("permissions", "deposits"));
                authorization.AddPolicy("withdrawals", x => x.RequireClaim("permissions", "withdrawals"));
            });

        return builder;
    }

    public static IApplicationBuilder UseCore(this IApplicationBuilder app)
    {
        app.UseJaeger()
            .UseConvey()
            .UsePrometheus()
            .UseCertificateAuthentication()
            .UseRabbitMq()
            .SubscribeEvent<CustomerVerified>()
            .SubscribeEvent<DepositCompleted>()
            .SubscribeEvent<FundsAdded>()
            .SubscribeEvent<WalletAdded>();

        return app;
    }
}