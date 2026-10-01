using Store.Domain.Entities;

namespace Store.Application.Contracts.Persistence
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer> GetByPhone(string phone);
        Task<Customer> GetByEmail(string email);
    }
}
