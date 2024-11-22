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
        private readonly IReviewRepository _reviewRepository;

        public ProductController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [HttpPost("{productId}/review")]
        public async Task<IActionResult> AddReview(int productId, [FromBody] Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            review.ProductId = productId;
            review.Date = DateTime.UtcNow;

            try
            {
                await _reviewRepository.AddAsync(review);
                return Ok(new { message = "Review added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while adding the review.", details = ex.Message });
            }
        }

        [HttpPost("{productId}/review/update")]
        public async Task<IActionResult> AddOrUpdateReview(int productId, [FromBody] Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingReview = await _reviewRepository.GetByUserAndProductAsync(review.UserId, productId);

            if (existingReview != null)
            {
                // Atualiza a avaliação existente
                existingReview.Rating = review.Rating;
                existingReview.Comment = review.Comment;
                existingReview.Date = DateTime.UtcNow;

                await _reviewRepository.UpdateAsync(existingReview);
                return Ok(new { message = "Review updated successfully." });
            }
            else
            {
                // Adiciona uma nova avaliação
                review.ProductId = productId;
                review.Date = DateTime.UtcNow;

                await _reviewRepository.AddAsync(review);
                return Ok(new { message = "Review added successfully." });
            }
        }
    }
}
