using Store.Domain.Entities;

namespace Store.Application.Contracts.Infrastructure
{
    public interface IJwtTokenService
    {
        string Generate(Customer customer);
    }
}
