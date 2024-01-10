using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure.Extentions
{
    public static  class InfraServices
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection serviceCollection,IConfiguration configuration)
        {
            serviceCollection.AddDbContext<OrderContext>(options=>options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString") ,builder=>{
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            }));
            serviceCollection.AddScoped(typeof(IAsyncRepository<>),typeof(RepositoryBase<>));
            serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
            return serviceCollection;
        }
    }
}