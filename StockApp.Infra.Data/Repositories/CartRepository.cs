using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockApp.Domain.Entities;
using StockApp.Domain.Interfaces;
using StockApp.Infra.Data.Context;

namespace StockApp.Infra.Data.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartAsync(int? cartId)
        {
            var cartEntity = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cartEntity == null)
                return new Cart { Id = cartId ?? 0 };

            return new Cart
            {
                Id = cartEntity.Id,
                Items = cartEntity.Items.Select(i => new CartItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };
        }

        public async Task AddToCartAsync(int? cartId, CartItem item)
        {
            var cartEntity = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cartEntity == null)
            {
                cartEntity = new Cart { Id = cartId ?? 0 };
                _context.Carts.Add(cartEntity);
            }

            var existingItem = cartEntity.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cartEntity.Items.Add(new CartItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
