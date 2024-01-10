using System.Text.Json;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data
{
    public class BrandContextSeed
    {
        public static async void SeedData(IMongoCollection<ProductBrand> brandCollection)
        {
            bool checkBrands = brandCollection.Find(b => true).Any();
          
            if (!checkBrands)
            {
                string path = await File.ReadAllTextAsync(@"D:\eShoppreload\Services\Catalog\Catalog.Infrastructure\Data\SeedData\brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(path);
                if (brands != null)
                {
                    foreach (var item in brands)
                    {
                        await brandCollection.InsertOneAsync(item);
                    }
                }
            }
        }
    }
}
