using System.Threading.Tasks;
using Convey;
using Convey.Auth;
using Convey.HTTP;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.Prometheus;
using Convey.Security;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Inflow.APIGateway.Correlation;
using Inflow.APIGateway.Identity;
using Inflow.APIGateway.Messaging;
using Inflow.APIGateway.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTracing;
using Yarp.ReverseProxy.Transforms;

namespace Inflow.APIGateway;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly string _appName;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
        _appName = configuration["app:name"];
    }
        
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddConvey()
            .AddJaeger()
            .AddJwt()
            .AddPrometheus()
            .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
            .AddSecurity()
            .Build();
            
        services.AddScoped<LogContextMiddleware>();
        services.AddScoped<UserMiddleware>();
        services.AddScoped<MessagingMiddleware>();
        services.AddSingleton<IJsonSerializer, SystemTextJsonSerializer>();
        services.AddSingleton<ICorrelationIdFactory, CorrelationIdFactory>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<ICorrelationContextBuilder, CorrelationContextBuilder>();
        services.AddSingleton<RouteMatcher>();
        services.Configure<MessagingOptions>(_configuration.GetSection("messaging"));
            
        services.AddAuthorization(options =>
        {
            options.AddPolicy("authenticatedUser", policy =>
                policy.RequireAuthenticatedUser());
        });

        services.AddCors(cors =>
        {
            cors.AddPolicy("cors", x =>
            {
                x.WithOrigins("*")
                    .WithMethods("POST", "PUT", "DELETE")
                    .WithHeaders("Content-Type", "Authorization");
            });
        });
            
        services.AddReverseProxy()
            .LoadFromConfig(_configuration.GetSection("reverseProxy"))
            .AddTransforms(builderContext =>
            {
                builderContext.AddRequestTransform(transformContext =>
                {
                    var correlationIdFactory = transformContext
                        .HttpContext
                        .RequestServices
                        .GetRequiredService<ICorrelationIdFactory>();

                    var correlationId = correlationIdFactory.Create();
                    transformContext.ProxyRequest.Headers.Add("x-correlation-id", correlationId);
                        
                    var tracer = transformContext
                        .HttpContext
                        .RequestServices
                        .GetRequiredService<ITracer>();
                        
                    var span = tracer?.ActiveSpan?.Context?.ToString();
                    if (!string.IsNullOrWhiteSpace(span))
                    {
                        transformContext.ProxyRequest.Headers.Add("uber-trace-id", span);
                    }

                    var correlationContextBuilder = transformContext
                        .HttpContext
                        .RequestServices
                        .GetRequiredService<ICorrelationContextBuilder>();
                        
                    var jsonSerializer = transformContext
                        .HttpContext
                        .RequestServices
                        .GetRequiredService<IJsonSerializer>();
                        
                    var correlationContext = correlationContextBuilder.Build(transformContext.HttpContext, correlationId, span);
                    transformContext.ProxyRequest.Headers.Add("x-correlation-context", jsonSerializer.Serialize(correlationContext));
                        
                    return ValueTask.CompletedTask;
                });
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseJaeger();
        app.UseMiddleware<LogContextMiddleware>();
        app.UseCors("cors");
        app.UseConvey();
        app.UsePrometheus();
        app.UseAccessTokenValidator();
        app.UseAuthentication();
        app.UseRabbitMq();
        app.UseMiddleware<UserMiddleware>();
        app.UseMiddleware<MessagingMiddleware>();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", context => context.Response.WriteAsync(_appName));
            endpoints.MapReverseProxy();
        });
    }
}