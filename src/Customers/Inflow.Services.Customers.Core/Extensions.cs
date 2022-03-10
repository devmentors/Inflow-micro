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
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.Outbox.EntityFramework;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.Prometheus;
using Convey.Security;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi.CQRS;
using Convey.WebApi.Security;
using Inflow.Services.Customers.Core.Clients;
using Inflow.Services.Customers.Core.Contexts;
using Inflow.Services.Customers.Core.DAL;
using Inflow.Services.Customers.Core.DAL.Repositories;
using Inflow.Services.Customers.Core.Domain.Repositories;
using Inflow.Services.Customers.Core.Infrastructure;
using Inflow.Services.Customers.Core.Infrastructure.Decorators;
using Inflow.Services.Customers.Core.Infrastructure.Exceptions;
using Inflow.Services.Customers.Core.Infrastructure.Logging;
using Inflow.Services.Customers.Core.Infrastructure.Serialization;
using Inflow.Services.Customers.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Inflow.Services.Customers.Api")]

namespace Inflow.Services.Customers.Core;

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
            .AddJwt()
            .AddHttpClient()
            .AddConsul()
            .AddFabio()
            .AddRabbitMq()
            .AddSwaggerDocs()
            .AddCertificateAuthentication()
            .AddSecurity()
            .Build();
            
        var postgresOptions = builder.GetOptions<PostgresOptions>("postgres");
        builder
            .Services
            .AddSingleton(postgresOptions)
            .AddDbContext<CustomersDbContext>(x => x.UseNpgsql(postgresOptions.ConnectionString))
            .AddSingleton<ErrorHandlerMiddleware>()
            .AddSingleton<ExceptionToResponseMapper>()
            .AddSingleton<IJsonSerializer, SystemTextJsonSerializer>()
            .AddScoped<IMessageBroker, MessageBroker>()
            .AddSingleton<IClock, UtcClock>()
            .AddSingleton<RequestTypeMetricsMiddleware>()
            .AddScoped<LogContextMiddleware>()
            .AddSingleton<ICorrelationIdFactory, CorrelationIdFactory>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddTransient<IContextFactory, ContextFactory>()
            .AddTransient(ctx => ctx.GetRequiredService<IContextFactory>().Create())
            .AddAuthorization(authorization =>
            {
                authorization.AddPolicy("customers", x => x.RequireClaim("permissions", "customers"));
            });

        builder.Services.AddSingleton<IUserApiClient, UserApiClient>();

        // builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
        // builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

        return builder;
    }

    public static IApplicationBuilder UseCore(this IApplicationBuilder app)
    {
        app.UseMiddleware<LogContextMiddleware>()
            .UseMiddleware<RequestTypeMetricsMiddleware>()
            .UseMiddleware<ErrorHandlerMiddleware>()
            .UseSwaggerDocs()
            .UseConvey()
            .UseCertificateAuthentication()
            .UseAuthentication()
            .UseRabbitMq();

        using var scope = app.ApplicationServices.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<CustomersDbContext>().Database;
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