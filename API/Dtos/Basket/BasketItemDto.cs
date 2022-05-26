using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Basket
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity must be at least one")]
        public int Quantity { get; set; }
        [Required]
        public string Category { get; set; }

    }
}
