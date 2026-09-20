using System.ComponentModel.DataAnnotations;
using Store.Application.Helpers;

namespace Store.Application.DTOs.Authentication
{
    public class Authenticate
    {
        [Required(ErrorMessage = PublicHelper.RequiredValidationErrorMessage)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = PublicHelper.RequiredValidationErrorMessage)]
        public string Password { get; set; } = string.Empty;
    }
}
