using Microsoft.EntityFrameworkCore;
using Store.Application.Contracts.Persistence;
using Store.Domain.Entities;

namespace Store.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Product> GetAllWithCategory()
        {
            return _dbContext.Products.Include(i => i.Category);
        }
    }
}
