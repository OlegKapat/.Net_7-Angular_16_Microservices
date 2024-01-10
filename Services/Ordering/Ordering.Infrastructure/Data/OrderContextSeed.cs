using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public  class OrderContextSeed
    {
        public static async Task SeedAsync(
            OrderContext orderContext,
            ILogger<OrderContextSeed> logger
        )
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetOrders());
                await orderContext.SaveChangesAsync();
                //logger.LogInformation($"Ordering Database: {typeof(OrderContext).Name} seeded.");
            }
        }

        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
            {
                new()
                {
                    UserName = "oleh",
                    FirstName = "Oleh",
                    LastName = "bashuk",
                    EmailAddress = "asd@eshop.net",
                    AddressLine = "Kyiv",
                    Country = "Ukraine",
                    TotalPrice = 750,
                    State = "Mark",
                    ZipCode = "03140",
                    CardName = "Visa",
                    CardNumber = "1234567890123456",
                    CreatedBy = "Oleh",
                    Expiration = "12/28",
                    Cvv = "356",
                    PaymentMethod = 1,
                    LastModifiedBy = "Oleh",
                    LastModifiedDate = new DateTime(),
                }
            };
        }
    }
}
