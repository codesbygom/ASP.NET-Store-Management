using System.ComponentModel.DataAnnotations;
using Store.Domain.Enums;

namespace Store.Application.DTOs.Order
{
    public class CreateOrder
    {
        [Required]
        [MinLength(1, ErrorMessage = "An order needs at least one item")]
        public List<CreateOrderItem> Items { get; set; } = new List<CreateOrderItem>();
    }

    public class CreateOrderItem
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, 1000)]
        public int Quantity { get; set; }
    }

    public class UpdateOrderStatus
    {
        public OrderStatus Status { get; set; }
    }
}
