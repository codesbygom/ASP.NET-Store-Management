using System.ComponentModel.DataAnnotations;
using Store.Application.DTOs.Base;
using Store.Application.Helpers;

namespace Store.Application.DTOs.Customer
{
    public class UpsertCustomer : UpsertDTO
    {
        [Required(ErrorMessage = PublicHelper.RequiredValidationErrorMessage)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = PublicHelper.RequiredValidationErrorMessage)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = PublicHelper.RequiredValidationErrorMessage)]
        public string LastName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
    }
}
