using Ocelot.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Common.Logging.Correlation;
using System.ComponentModel;

namespace ApiGateways
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //! Here we neednt add the IdentityServer4 configuration as we are not using it in this project
            //var authShema = "EShoppingGatewayAuthScheme";
            // services
            //     .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddJwtBearer(
            //         authShema,
            //         options =>
            //         {
            //             options.Authority = "https://localhost:9009";
            //             options.Audience = "EShoppingGateway";
            //         }
            //     );
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
            services.AddOcelot().AddCacheManager(o => o.WithDictionaryHandle());
            services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                policy => { policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
        });
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.AddCorrelationIdMiddleware();
            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet(
                    "/",
                    async context =>
                    {
                        await context.Response.WriteAsync("Ocelot API Gateway");
                    }
                );
            });
            await app.UseOcelot();
        }
    }
}
