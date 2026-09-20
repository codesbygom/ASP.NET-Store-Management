using Store.Domain.Entities;

namespace Store.Application.Contracts.Persistence
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IQueryable<Product> GetAllWithCategory();
    }
}
