using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.DTOs.Customer;
using Store.Application.Extensions;
using Store.Application.Features.Interfaces;
using Store.Application.Responses;

namespace Store.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("me")]
        [Produces(typeof(Response<CustomerDTO>))]
        public async Task<JsonResult> Me()
        {
            var res = await _customerService.Get(User.GetUserId());

            return new Response<CustomerDTO>(res).ToJsonResult();
        }

        [HttpPut("me")]
        [Produces(typeof(Response<CustomerDTO>))]
        public async Task<JsonResult> UpdateMe(UpsertCustomer input)
        {
            input.Id = User.GetUserId();

            var res = await _customerService.Edit(input);

            return new Response<CustomerDTO>(res).ToJsonResult();
        }

        [HttpDelete("me")]
        public async Task<JsonResult> DeleteMe()
        {
            await _customerService.Delete(User.GetUserId());

            return Response<string>.Succeed();
        }
    }
}
