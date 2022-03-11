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
using Inflow.Services.Users.Core.Commands;
using Inflow.Services.Users.Core.Contexts;
using Inflow.Services.Users.Core.DAL;
using Inflow.Services.Users.Core.DAL.Repositories;
using Inflow.Services.Users.Core.Entities;
using Inflow.Services.Users.Core.Infrastructure;
using Inflow.Services.Users.Core.Infrastructure.Decorators;
using Inflow.Services.Users.Core.Infrastructure.Exceptions;
using Inflow.Services.Users.Core.Infrastructure.Logging;
using Inflow.Services.Users.Core.Infrastructure.Serialization;
using Inflow.Services.Users.Core.Repositories;
using Inflow.Services.Users.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Inflow.Services.Users.Api")]

namespace Inflow.Services.Users.Core;

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
            .AddMessageOutbox(outbox => outbox.AddEntityFramework<UsersDbContext>())
            .AddPrometheus()
            .AddJaeger()
            .AddSwaggerDocs()
            .AddCertificateAuthentication()
            .AddSecurity()
            .Build();
            
        var postgresOptions = builder.GetOptions<PostgresOptions>("postgres");
        var registrationOptions = builder.GetOptions<RegistrationOptions>("registration");
            
        builder
            .Services
            .AddSingleton(postgresOptions)
            .AddSingleton(registrationOptions)
            .AddScoped<UsersInitializer>()
            .AddDbContext<UsersDbContext>(x => x.UseNpgsql(postgresOptions.ConnectionString))
            .AddSingleton<ErrorHandlerMiddleware>()
            .AddSingleton<ExceptionToResponseMapper>()
            .AddSingleton<IJsonSerializer, SystemTextJsonSerializer>()
            .AddScoped<IMessageBroker, MessageBroker>()
            .AddSingleton<IClock, UtcClock>()
            .AddSingleton<RequestTypeMetricsMiddleware>()
            .AddScoped<LogContextMiddleware>()
            .AddSingleton<ICorrelationIdFactory, CorrelationIdFactory>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddSingleton<IUserRequestStorage, UserRequestStorage>()
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddTransient<IContextFactory, ContextFactory>()
            .AddTransient(ctx => ctx.GetRequiredService<IContextFactory>().Create())
            .AddAuthorization(authorization =>
            {
                authorization.AddPolicy("users", x => x.RequireClaim("permissions", "users"));
            });

        builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(LoggingEventHandlerDecorator<>));
        builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
        builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

        // Temporary fix for EF Core issue related to https://github.com/npgsql/efcore.pg/issues/2000
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
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
            .SubscribeCommand<SignUp>();

        using var scope = app.ApplicationServices.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<UsersDbContext>().Database;
        database.Migrate();
        var initializer = scope.ServiceProvider.GetRequiredService<UsersInitializer>();
        initializer.InitAsync().GetAwaiter().GetResult();

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