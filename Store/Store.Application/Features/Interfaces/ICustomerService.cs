using Store.Application.DTOs.Customer;

namespace Store.Application.Features.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDTO>> GetAll();
        Task<CustomerDTO> Get(int id);
        Task<CustomerDTO> Edit(UpsertCustomer input);
        Task<bool> Delete(int id);
        Task<bool> Recover(int id);
    }
}
