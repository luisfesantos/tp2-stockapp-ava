using Microsoft.AspNetCore.Mvc;
using StockApp.Domain.Entities;
using StockApp.Domain.Interfaces;
using StockApp.Infra.Data.Repositories;

namespace StockApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        [HttpPost("{productId}/review")]
        public async Task<IActionResult> AddReview(int productId, [FromBody] Review review, [FromServices] IReviewRepository reviewRepository)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            review.ProductId = productId;
            review.Date = DateTime.UtcNow;

            try
            {
                await reviewRepository.AddAsync(review);
                return Ok(new { message = "Review added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while adding the review.", details = ex.Message });
            }
        }

        [HttpPost("{productId}/review/update")]
        public async Task<IActionResult> AddOrUpdateReview(int productId, [FromBody] Review review, [FromServices] IReviewRepository reviewRepository)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingReview = await reviewRepository.GetByUserAndProductAsync(review.UserId, productId);

            if (existingReview != null)
            {
                // Atualiza a avaliação existente
                existingReview.Rating = review.Rating;
                existingReview.Comment = review.Comment;
                existingReview.Date = DateTime.UtcNow;

                await reviewRepository.UpdateAsync(existingReview);
                return Ok(new { message = "Review updated successfully." });
            }
            else
            {
                // Adiciona uma nova avaliação
                review.ProductId = productId;
                review.Date = DateTime.UtcNow;

                await reviewRepository.AddAsync(review);
                return Ok(new { message = "Review added successfully." });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> Search(
        [FromServices] IProductRepository productRepository,
        [FromQuery] string? query,
        [FromQuery] string? sortBy = "Name",
        [FromQuery] bool descending = false,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null)
        {
            if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
            {
                return BadRequest("O preço mínimo não pode ser maior que o preço máximo.");
            }
            var filters = new ProductFilters
            {
                Query = query,
                SortBy = sortBy,
                Descending = descending,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            var products = await productRepository.SearchAsync(filters);

            return Ok(products);
        }

    }
}
