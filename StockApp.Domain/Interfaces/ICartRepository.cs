using StockApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(int? cartId);
        Task AddToCartAsync(int? cartId, CartItem item);
    }
}
