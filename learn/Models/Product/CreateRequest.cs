using System.ComponentModel.DataAnnotations;

namespace learn.Models.Product
{
    public class CreateRequest
    {
        [Required]
        public string? name { get; set; }

        [Required]
        public string? description { get; set; }

        [Required]
        public decimal price { get; set; }
    }
}
