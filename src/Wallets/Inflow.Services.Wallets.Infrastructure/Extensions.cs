using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Convey;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.Docs.Swagger;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.Outbox.EntityFramework;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.Prometheus;
using Convey.Security;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi.CQRS;
using Convey.WebApi.Security;
using Inflow.Services.Wallets.Application;
using Inflow.Services.Wallets.Application.Owners.Events.External;
using Inflow.Services.Wallets.Application.Services;
using Inflow.Services.Wallets.Application.Wallets.Commands;
using Inflow.Services.Wallets.Application.Wallets.Events.External;
using Inflow.Services.Wallets.Application.Wallets.Storage;
using Inflow.Services.Wallets.Core.Owners.Repositories;
using Inflow.Services.Wallets.Core.Wallets.Repositories;
using Inflow.Services.Wallets.Infrastructure.Contexts;
using Inflow.Services.Wallets.Infrastructure.Decorators;
using Inflow.Services.Wallets.Infrastructure.EF;
using Inflow.Services.Wallets.Infrastructure.EF.Repositories;
using Inflow.Services.Wallets.Infrastructure.Exceptions;
using Inflow.Services.Wallets.Infrastructure.Logging;
using Inflow.Services.Wallets.Infrastructure.Messaging;
using Inflow.Services.Wallets.Infrastructure.Serialization;
using Inflow.Services.Wallets.Infrastructure.Storage;
using Inflow.Services.Wallets.Infrastructure.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Inflow.Services.Wallets.Api")]
[assembly: InternalsVisibleTo("Inflow.Services.Wallets.Tests.EndToEnd")]
[assembly: InternalsVisibleTo("Inflow.Services.Wallets.Tests.Integration")]
[assembly: InternalsVisibleTo("Inflow.Services.Wallets.Tests.Shared")]

namespace Inflow.Services.Wallets.Infrastructure;

internal static class Extensions
{
    public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
    {
        builder
            .AddExceptionToFailedMessageMapper<ExceptionToFailedMessageMapper>()
            .AddCommandHandlers()
            .AddEventHandlers()
            .AddQueryHandlers()
            .AddInMemoryCommandDispatcher()
            .AddInMemoryEventDispatcher()
            .AddInMemoryQueryDispatcher()
            .AddInMemoryDispatcher()
            .AddJwt()
            .AddHttpClient()
            .AddConsul()
            .AddFabio()
            .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
            .AddMessageOutbox(outbox => outbox.AddEntityFramework<WalletsDbContext>())
            .AddPrometheus()
            .AddJaeger()
            .AddSwaggerDocs()
            .AddCertificateAuthentication()
            .AddSecurity()
            .Build();
            
        var postgresOptions = builder.GetOptions<PostgresOptions>("postgres");
        builder
            .Services
            .AddSingleton(postgresOptions)
            .AddDbContext<WalletsDbContext>(x => x.UseNpgsql(postgresOptions.ConnectionString))
            .AddSingleton<ErrorHandlerMiddleware>()
            .AddSingleton<ExceptionToResponseMapper>()
            .AddSingleton<IJsonSerializer, SystemTextJsonSerializer>()
            .AddScoped<IMessageBroker, MessageBroker>()
            .AddSingleton<IClock, UtcClock>()
            .AddSingleton<RequestTypeMetricsMiddleware>()
            .AddScoped<LogContextMiddleware>()
            .AddSingleton<ICorrelationIdFactory, CorrelationIdFactory>()
            .AddTransient<IContextFactory, ContextFactory>()
            .AddTransient(ctx => ctx.GetRequiredService<IContextFactory>().Create())
            .AddScoped<ITransferStorage, TransferStorage>()
            .AddScoped<IWalletStorage, WalletStorage>()
            .AddScoped<ICorporateOwnerRepository, CorporateOwnerRepository>()
            .AddScoped<IIndividualOwnerRepository, IndividualOwnerRepository>()
            .AddScoped<IWalletRepository, WalletRepository>()
            .AddAuthorization(authorization =>
            {
                authorization.AddPolicy("transfers", x => x.RequireClaim("permissions", "transfers"));
                authorization.AddPolicy("wallets", x => x.RequireClaim("permissions", "wallets"));
            });

        builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(LoggingEventHandlerDecorator<>));
        builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
        builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

        // Temporary fix for EF Core issue related to https://github.com/npgsql/efcore.pg/issues/2000
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        return builder;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseMiddleware<LogContextMiddleware>()
            .UseJaeger()
            .UseMiddleware<RequestTypeMetricsMiddleware>()
            .UseMiddleware<ErrorHandlerMiddleware>()
            .UseSwaggerDocs()
            .UseConvey()
            .UsePublicContracts<ContractAttribute>()
            .UsePrometheus()
            .UseCertificateAuthentication()
            .UseAuthentication()
            .UseRabbitMq()
            .SubscribeCommand<AddFunds>()
            .SubscribeEvent<CustomerCompleted>()
            .SubscribeEvent<CustomerVerified>()
            .SubscribeEvent<DepositAccountAdded>()
            .SubscribeEvent<DepositCompleted>()
            .SubscribeEvent<WithdrawalStarted>();

        using var scope = app.ApplicationServices.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<WalletsDbContext>().Database;
        database.Migrate();

        return app;
    }
        
    internal static CorrelationContext GetCorrelationContext(this IHttpContextAccessor accessor)
    {
        if (accessor.HttpContext is null)
        {
            return null;
        }

        if (!accessor.HttpContext.Request.Headers.TryGetValue("x-correlation-context", out var json))
        {
            return null;
        }

        var jsonSerializer = accessor.HttpContext.RequestServices.GetRequiredService<IJsonSerializer>();
        var value = json.FirstOrDefault();

        return string.IsNullOrWhiteSpace(value) ? null : jsonSerializer.Deserialize<CorrelationContext>(value);
    }
        
    public static string GetUserIpAddress(this HttpContext context)
    {
        if (context is null)
        {
            return string.Empty;
        }
            
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        if (context.Request.Headers.TryGetValue("x-forwarded-for", out var forwardedFor))
        {
            var ipAddresses = forwardedFor.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);
            if (ipAddresses.Any())
            {
                ipAddress = ipAddresses[0];
            }
        }

        return ipAddress ?? string.Empty;
    }
}