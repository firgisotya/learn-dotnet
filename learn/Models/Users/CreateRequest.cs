using learn.Entities;
using System.ComponentModel.DataAnnotations;

namespace learn.Models.Users
{
    public class CreateRequest
    {
        [Required]
        public string? Fullname { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [EnumDataType(typeof(Role))]
        public string? Role { get; set; }

        [Required]
        [MinLength(8)]
        public string? Password { get; set; }

        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
    }
}
