using Store.Application.DTOs.Base;

namespace Store.Application.DTOs.Category
{
    public class CategoryDTO : BaseDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TotalProducts { get; set; }
    }
}
