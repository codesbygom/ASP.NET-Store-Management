using Store.Application.DTOs.Base;
using Store.Application.DTOs.Product;

namespace Store.Application.Features.Interfaces
{
    public interface IProductService
    {
        Task<PaginateDTO<ProductDTO>> FilterPaginate(FilterProducts input);
        Task<ProductDTO> Get(int id);
        Task<ProductDTO> Create(UpsertProduct input);
        Task<ProductDTO> Edit(UpsertProduct input);
        Task<bool> Delete(int id);
    }
}
