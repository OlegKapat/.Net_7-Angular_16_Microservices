using System.Text.Json;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContextSeed
    {
        public static async void SeedData(IMongoCollection<Product> productCollection)
        {
            bool checkProducts = productCollection.Find(b => true).Any();
           
            if (!checkProducts)
            {
                 string path = await File.ReadAllTextAsync(@"D:\eShoppreload\Services\Catalog\Catalog.Infrastructure\Data\SeedData\products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(path);
                if (products != null)
                {
                    foreach (var item in products)
                    {
                        await productCollection.InsertOneAsync(item);
                    }
                }
            }
        }
    }
}
