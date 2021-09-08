using Convey;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Inflow.Saga.Api
{
    public class Startup
    {
        private readonly string _appName;

        public Startup(IConfiguration configuration)
        {
            _appName = configuration["app:name"];
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
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

            app.UseCore();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context => context.Response.WriteAsync(_appName));
            });
        }
    }
}