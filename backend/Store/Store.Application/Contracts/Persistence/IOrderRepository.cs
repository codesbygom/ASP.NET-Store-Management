using Store.Domain.Entities;

namespace Store.Application.Contracts.Persistence
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetWithItems(int id);
        Task<List<Order>> GetByCustomer(int customerId);
    }
}
