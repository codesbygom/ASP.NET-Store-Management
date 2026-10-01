using Store.Application.DTOs.Category;

namespace Store.Application.Features.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAll();
        Task<CategoryDTO> Get(int id);
        Task<CategoryDTO> Create(UpsertCategory input);
        Task<CategoryDTO> Edit(UpsertCategory input);
        Task<bool> Delete(int id);
    }
}
