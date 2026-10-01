using System.ComponentModel.DataAnnotations;
using Store.Application.Helpers;

namespace Store.Application.DTOs.Authentication
{
    public class Register
    {
        [Required(ErrorMessage = PublicHelper.RequiredValidationErrorMessage)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = PublicHelper.RequiredValidationErrorMessage)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = PublicHelper.RequiredValidationErrorMessage)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = PublicHelper.RequiredValidationErrorMessage)]
        public string LastName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
    }
}
