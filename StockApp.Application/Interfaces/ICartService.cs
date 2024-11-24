using StockApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Application.Interfaces
{
    public interface ICartService
    {
        Task AddToCartAsync(int? cartId, CartItem item);
        Task<Cart> GetCartAsync(int? cartId);
    }
}
