using System.ComponentModel.DataAnnotations;
using Store.Application.DTOs.Base;
using Store.Application.Helpers;

namespace Store.Application.DTOs.Product
{
    public class UpsertProduct : UpsertDTO
    {
        [Required(ErrorMessage = PublicHelper.RequiredValidationErrorMessage)]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int Stock { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }
    }
}
