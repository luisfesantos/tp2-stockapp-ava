using Microsoft.AspNetCore.Mvc;
using StockApp.Application.Interfaces;
using StockApp.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace StockApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("{cartId}/items")]
        public async Task<IActionResult> AddToCart(int? cartId, [FromBody] CartItem cartItem)
        {
            if (cartItem == null || cartItem.ProductId <= 0 || cartItem.Quantity <= 0)
                return BadRequest("Dados inválidos para o item.");

            await _cartService.AddToCartAsync(cartId, cartItem);
            return Ok(new { Message = "Item adicionado ao carrinho com sucesso." });
        }

        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCart(int? cartId)
        {
            if (cartId == null)
                return BadRequest("ID do carrinho inválido.");

            var cart = await _cartService.GetCartAsync(cartId!);

            if (cart == null || cart.Items.Count == 0)
                return NotFound(new { Message = "Carrinho vazio ou não encontrado." });

            return Ok(cart);
        }
    }
}
