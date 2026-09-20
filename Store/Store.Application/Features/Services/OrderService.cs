using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Application.Contracts.Persistence;
using Store.Application.DTOs.Order;
using Store.Application.Exceptions;
using Store.Application.Features.Interfaces;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Application.Features.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<OrderDTO> Create(int customerId, CreateOrder input)
        {
            var productIds = input.Items.Select(s => s.ProductId).Distinct().ToList();

            var products = await _productRepository.GetAllQueryable()
                .Where(w => w.IsActive && productIds.Contains(w.Id))
                .ToListAsync();

            if (products.Count != productIds.Count)
            {
                throw new NotFoundException("One or more products were not found");
            }

            var order = new Order { CustomerId = customerId, Status = OrderStatus.Pending };

            foreach (var line in input.Items)
            {
                var product = products.First(f => f.Id == line.ProductId);

                if (product.Stock < line.Quantity)
                {
                    throw new BadRequestException($"Not enough stock for {product.Title}");
                }

                product.Stock -= line.Quantity;

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = line.Quantity,
                    UnitPrice = product.Price
                });
            }

            order.TotalAmount = order.Items.Sum(s => s.UnitPrice * s.Quantity);

            order = await _orderRepository.Add(order);

            return await Get(order.Id, customerId);
        }

        public async Task<List<OrderDTO>> GetMyOrders(int customerId)
        {
            var orders = await _orderRepository.GetByCustomer(customerId);

            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> Get(int id, int customerId)
        {
            var order = await _orderRepository.GetWithItems(id);

            if (order == null || order.CustomerId != customerId)
            {
                throw new NotFoundException("Order not found");
            }

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> UpdateStatus(int id, UpdateOrderStatus input)
        {
            var order = await _orderRepository.GetWithItems(id);

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            order.Status = input.Status;

            await _orderRepository.Update(order);

            return _mapper.Map<OrderDTO>(order);
        }
    }
}
