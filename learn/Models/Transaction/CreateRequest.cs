using System.ComponentModel.DataAnnotations;

namespace learn.Models.Transaction
{
    public class CreateRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Total { get; set; }
    }
}
