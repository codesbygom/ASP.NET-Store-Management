using Microsoft.EntityFrameworkCore;
using Store.Application.Contracts.Persistence;
using Store.Domain.Entities;

namespace Store.Persistence.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Customer> GetByPhone(string phone)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(f => f.Phone == phone);
        }

        public async Task<Customer> GetByEmail(string email)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(f => f.Email == email);
        }
    }
}
