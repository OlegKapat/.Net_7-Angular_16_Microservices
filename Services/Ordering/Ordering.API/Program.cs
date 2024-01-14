using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Ordering.API.Extensions;
using Ordering.Aplication.Extentions;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extentions;
using MassTransit;
using EventBus.Messages.Common;
using Ordering.API.EventBusConsumer;
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

//builder.Services.AddMassTransitHostedService();

builder.Services.AddDbContext<OrderContext>(x=>x.UseSqlServer(configuration["ConnectionStrings:OrderingConnectionString"]));
builder.Services.AddApiVersioning();
builder.Services.AddApplicationServices();
builder.Services.AddInfraServices(configuration);
//builder.Services.AddScoped<OrderContextFactory>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<BasketOrderingConsumer>();
// host.MigrateDatabase<OrderContext>(
//     (context, services) =>
//     {
//         var logger = services.GetService<ILogger<OrderContextSeed>>();
//         OrderContextSeed.SeedAsync(context, logger!).Wait();
//     }
// );
builder.Services.AddControllers();
//builder.Services.AddEntityFrameworkSqlServer ();


builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketOrderingConsumer>();
    config.UsingRabbitMq(
        (ctx, cfg) =>
        {
            cfg.Host(configuration["EventBusSettings:HostAddress"]);
             cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue,c=>{
                c.ConfigureConsumer<BasketOrderingConsumer>(ctx);  
             });
        }
        
    );
});
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
