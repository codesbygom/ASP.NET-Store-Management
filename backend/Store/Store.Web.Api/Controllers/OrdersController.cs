using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.DTOs.Order;
using Store.Application.Extensions;
using Store.Application.Features.Interfaces;
using Store.Application.Responses;

namespace Store.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Produces(typeof(Response<List<OrderDTO>>))]
        public async Task<JsonResult> GetMyOrders()
        {
            var res = await _orderService.GetMyOrders(User.GetUserId());

            return new Response<List<OrderDTO>>(res).ToJsonResult();
        }

        [HttpGet("{id}")]
        [Produces(typeof(Response<OrderDTO>))]
        public async Task<JsonResult> Get(int id)
        {
            var res = await _orderService.Get(id, User.GetUserId());

            return new Response<OrderDTO>(res).ToJsonResult();
        }

        [HttpPost]
        [Produces(typeof(Response<OrderDTO>))]
        public async Task<JsonResult> Create(CreateOrder input)
        {
            var res = await _orderService.Create(User.GetUserId(), input);

            return new Response<OrderDTO>(res).ToJsonResult();
        }

        [HttpPut("{id}/status")]
        [Produces(typeof(Response<OrderDTO>))]
        public async Task<JsonResult> UpdateStatus(int id, UpdateOrderStatus input)
        {
            var res = await _orderService.UpdateStatus(id, input);

            return new Response<OrderDTO>(res).ToJsonResult();
        }
    }
}
