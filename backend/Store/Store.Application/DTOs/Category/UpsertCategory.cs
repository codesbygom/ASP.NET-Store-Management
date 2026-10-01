using System.ComponentModel.DataAnnotations;
using Store.Application.DTOs.Base;
using Store.Application.Helpers;

namespace Store.Application.DTOs.Category
{
    public class UpsertCategory : UpsertDTO
    {
        [Required(ErrorMessage = PublicHelper.RequiredValidationErrorMessage)]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
