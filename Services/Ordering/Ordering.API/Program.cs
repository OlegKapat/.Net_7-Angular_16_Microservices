using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Ordering.API.Extensions;
using Ordering.Aplication.Extentions;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extentions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

using IHost host = Host.CreateApplicationBuilder(args).Build();
// .MigrateDatabase<OrderContext>((context,services)=>
// {
//         var logger = services.GetService<ILogger<OrderContextSeed>>();
//         OrderContextSeed.SeedAsync(context).Wait();
// });

builder.Logging.AddConsole();
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();


builder.Services.AddControllers();
builder.Services.AddScoped<OrderContextFactory>();
//builder.Services.AddDbContext<OrderContext>(x=>x.UseSqlServer(configuration["ConnectionString:OrderingConnectionString"]));
builder.Services.AddApiVersioning();
builder.Services.AddApplicationServices();

builder.Services.AddInfraServices(configuration);
builder.Services.AddAutoMapper(typeof(Program));

host.MigrateDatabase<OrderContext>(
    (context, services) =>
    {
        var logger = services.GetService<ILogger<OrderContextSeed>>();
        OrderContextSeed.SeedAsync(context, logger!).Wait();
    }
);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
});
;


builder.Services.AddHealthChecks().Services.AddDbContext<OrderContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks(
        "/health",
        new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }
    );
});

app.Run();
