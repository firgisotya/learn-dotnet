using System.ComponentModel.DataAnnotations;
namespace learn.Models.Auth

{
    public class AuthRequest
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        [MinLength(8)]
        public string? Password { get; set; }
    }
}
