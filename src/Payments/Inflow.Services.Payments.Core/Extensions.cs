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
using Inflow.Services.Payments.Core.Contexts;
using Inflow.Services.Payments.Core.DAL;
using Inflow.Services.Payments.Core.DAL.Repositories;
using Inflow.Services.Payments.Core.Deposits.Commands;
using Inflow.Services.Payments.Core.Deposits.Domain.Factories;
using Inflow.Services.Payments.Core.Deposits.Domain.Repositories;
using Inflow.Services.Payments.Core.Deposits.Domain.Services;
using Inflow.Services.Payments.Core.Deposits.Events.External;
using Inflow.Services.Payments.Core.Infrastructure;
using Inflow.Services.Payments.Core.Infrastructure.Decorators;
using Inflow.Services.Payments.Core.Infrastructure.Exceptions;
using Inflow.Services.Payments.Core.Infrastructure.Logging;
using Inflow.Services.Payments.Core.Infrastructure.Serialization;
using Inflow.Services.Payments.Core.Services;
using Inflow.Services.Payments.Core.Withdrawals.Commands;
using Inflow.Services.Payments.Core.Withdrawals.Domain.Repositories;
using Inflow.Services.Payments.Core.Withdrawals.Events.External;
using Inflow.Services.Payments.Core.Withdrawals.Services;
using Inflow.Services.Payments.Shared.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Inflow.Services.Payments.Api")]

namespace Inflow.Services.Payments.Core;

internal static class Extensions
{
    public static IConveyBuilder AddCore(this IConveyBuilder builder)
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
            .AddMessageOutbox(outbox => outbox.AddEntityFramework<PaymentsDbContext>())
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
            .AddDbContext<PaymentsDbContext>(x => x.UseNpgsql(postgresOptions.ConnectionString))
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
            .AddSingleton<IWithdrawalMetadataResolver, WithdrawalMetadataResolver>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<IDepositRepository, DepositRepository>()
            .AddScoped<IDepositAccountRepository, DepositAccountRepository>()
            .AddScoped<IWithdrawalRepository, WithdrawalRepository>()
            .AddScoped<IWithdrawalAccountRepository, WithdrawalAccountRepository>()
            .AddSingleton<ICurrencyResolver, CurrencyResolver>()
            .AddSingleton<IDepositAccountFactory, DepositAccountFactory>()
            .AddAuthorization(authorization =>
            {
                authorization.AddPolicy("deposits", x => x.RequireClaim("permissions", "deposits"));
                authorization.AddPolicy("withdrawals", x => x.RequireClaim("permissions", "withdrawals"));
            });

        builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(LoggingEventHandlerDecorator<>));
        builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
        builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));
            
        return builder;
    }

    public static IApplicationBuilder UseCore(this IApplicationBuilder app)
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
            .SubscribeCommand<StartDeposit>()
            .SubscribeCommand<StartWithdrawal>()
            .SubscribeEvent<CustomerCompleted>()
            .SubscribeEvent<CustomerLocked>()
            .SubscribeEvent<CustomerUnlocked>()
            .SubscribeEvent<CustomerVerified>()
            .SubscribeEvent<DeductFundsRejected>()
            .SubscribeEvent<FundsDeducted>();

        using var scope = app.ApplicationServices.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>().Database;
        database.Migrate();

        // Temporary fix for EF Core issue related to https://github.com/npgsql/efcore.pg/issues/2000
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
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