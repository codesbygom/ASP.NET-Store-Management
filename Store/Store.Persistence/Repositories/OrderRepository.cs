using Microsoft.EntityFrameworkCore;
using Store.Application.Contracts.Persistence;
using Store.Domain.Entities;

namespace Store.Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Order> GetWithItems(int id)
        {
            return await _dbContext.Orders
                .Include(i => i.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(f => f.IsActive && f.Id == id);
        }

        public async Task<List<Order>> GetByCustomer(int customerId)
        {
            return await _dbContext.Orders
                .Include(i => i.Items)
                .ThenInclude(i => i.Product)
                .Where(w => w.IsActive && w.CustomerId == customerId)
                .AsNoTracking()
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
    }
}
