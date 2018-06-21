using System.ComponentModel.DataAnnotations;

namespace FindAndBook.API.Models
{
    public class UserModel
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        [RegularExpression("[a-zA-Z]+[0-9]+", ErrorMessage = "Password must contain at least one letter and digit.")]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "First name must contain only letters.")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Last name must contain only letters.")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("[0-9]+", ErrorMessage = "Phone number must contain only digits.")]
        public string PhoneNumber { get; set; }

        public string Role { get; set; }
    }
}