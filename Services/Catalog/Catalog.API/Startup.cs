using System.Reflection;
using Catalog.Application.Handlers;
using Catalog.Application.Services;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace Catalog.API
{
    public class Startup
    {
        public IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning();
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    policy =>
                    {
                        policy
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithOrigins("http://localhost:9000");
                    }
                );
            });

            services
                .AddHealthChecks()
                .AddMongoDb(
                    Configuration["DatabaseSettings:ConnectionString"],
                    "Catalog  Mongo Db Health Check",
                    HealthStatus.Degraded
                );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog.API", Version = "v1" });
            });
            services.AddAutoMapper(typeof(Startup));

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
            }
            services
                .AddScoped<ICatalogContext, CatalogContext>()
                .AddHealthChecks()
                .AddCheck<CustomHealthCheck>(nameof(CustomHealthCheck));
            services.AddScoped<ICatalogContext, CatalogContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBrandRepository, ProductRepository>();
            services.AddScoped<ITypesRepository, ProductRepository>();
            //services.AddControllers();

            //? Add Authentication
            var userPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter(userPolicy));
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:9009";
                    options.Audience = "Catalog";
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanRead", builder =>
                {
                    builder.RequireClaim("scope", "catalogapi.read");
                });
                options.AddPolicy("CanWrite", builder =>
                {
                    builder.RequireClaim("scope", "catalogapi.write");
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1")
                );
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks(
                    "/health",
                    new HealthCheckOptions()
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    }
                );
            });
        }
    }
}
