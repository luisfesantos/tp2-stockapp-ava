using System.ComponentModel.DataAnnotations;

namespace StockApp.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }

        [Range(1, 5, ErrorMessage = "The rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [MaxLength(500, ErrorMessage = "The comment cannot exceed 500 characters.")]
        public string Comment { get; set; }

        public DateTime Date { get; set; }
    }
}
