using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Core.Entities;

namespace Basket.Core.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName);
        Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart);
        Task<bool> DeleteBasket(string userName);
    }
}