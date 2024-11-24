using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockApp.Application.Interfaces;
using StockApp.Domain.Entities;
using StockApp.Domain.Interfaces;

namespace StockApp.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task AddToCartAsync(int? cartId, CartItem item)
        {
            await _cartRepository.AddToCartAsync(cartId, item);
        }

        public async Task<Cart> GetCartAsync(int? cartId)
        {
            return await _cartRepository.GetCartAsync(cartId);
        }
    }

}
