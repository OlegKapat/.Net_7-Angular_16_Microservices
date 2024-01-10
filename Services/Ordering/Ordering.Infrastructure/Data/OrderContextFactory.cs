using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
            var optionsBuilder = new DbContextOptionsBuilder<OrderContext>();
            var connecxtionstring = configuration.GetConnectionString("OrderingConnectionString");
            optionsBuilder.UseSqlServer(connecxtionstring, b=>b.MigrationsAssembly("initialcreate"));
            return new OrderContext(optionsBuilder.Options);
        }
    }
}
