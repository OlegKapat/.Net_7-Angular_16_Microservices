using Discount.API.Services;
using Discount.Application.Commands;
using Discount.Core.Repositories;
using Discount.Infrastructure.Extentions;
using Discount.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
using IHost host = Host.CreateApplicationBuilder(args).Build();
DbExtension.MigrateDatabase<Program>(host);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo {Title = "Ordering.API", Version = "v1"});
});
builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(CreateDiscountCommandHandler))
);
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddGrpc();

var app = builder.Build();

//var scodeInstance = builder.Services.


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
#pragma warning disable ASP0014
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<DiscountService>();
    endpoints.MapGet(
        "/",
        async context =>
        {
            await context.Response.WriteAsync(
                "Communication with gRPC endpoints must be made through a gRPC client."
            );
        }
    );
});
app.UseHttpsRedirection();
app.Run();
