using System.Text.Json;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data
{
    public class TypeContextSeed
    {
        public static async  void SeedData(IMongoCollection<ProductType> typeCollection)
        {
            bool checkTypes = typeCollection.Find(b => true).Any();
           
            if (!checkTypes)
            {
                 string path = await File.ReadAllTextAsync(@"D:\eShoppreload\Services\Catalog\Catalog.Infrastructure\Data\SeedData\types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(path);
                if (types != null)
                {
                    foreach (var item in types)
                    {
                       await typeCollection.InsertOneAsync(item);
                    }
                }
            }
        }
    }
}
