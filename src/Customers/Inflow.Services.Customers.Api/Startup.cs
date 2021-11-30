using Convey;
using Inflow.Services.Customers.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Inflow.Services.Customers.Api;

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
            .AddCore()
            .Build();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCore()
            .UseRouting()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context => context.Response.WriteAsync(_appName));
                endpoints.MapControllers();
            });
    }
}