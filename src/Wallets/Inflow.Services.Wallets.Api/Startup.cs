using Convey;
using Inflow.Services.Wallets.Application;
using Inflow.Services.Wallets.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Inflow.Services.Wallets.Api;

public class Startup
{
    private readonly string _appName;

    public Startup(IConfiguration configuration)
    {
        _appName = configuration["app:name"];
    }
        
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services
            .AddConvey()
            .AddApplication()
            .AddInfrastructure()
            .Build();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseInfrastructure()
            .UseRouting()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context => context.Response.WriteAsync(_appName));
                endpoints.MapControllers();
            });
    }
}