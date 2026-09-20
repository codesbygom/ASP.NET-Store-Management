using Store.Application.DTOs.Order;

namespace Store.Application.Features.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> Create(int customerId, CreateOrder input);
        Task<List<OrderDTO>> GetMyOrders(int customerId);
        Task<OrderDTO> Get(int id, int customerId);
        Task<OrderDTO> UpdateStatus(int id, UpdateOrderStatus input);
    }
}
