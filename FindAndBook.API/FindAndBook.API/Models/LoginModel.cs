using System.ComponentModel.DataAnnotations;

namespace FindAndBook.API.Models
{
    public class LoginModel
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        [RegularExpression("[a-zA-Z]+[0-9]+", ErrorMessage = "Password must contain at least one letter and digit.")]
        public string Password { get; set; }
    }
}