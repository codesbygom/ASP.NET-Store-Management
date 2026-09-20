using Store.Application.DTOs.Base;

namespace Store.Application.DTOs.Product
{
    public class ProductDTO : BaseDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
