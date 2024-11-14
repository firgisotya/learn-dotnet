using System.ComponentModel.DataAnnotations;

namespace learn.Models.Product
{
    public class CreateRequest
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
