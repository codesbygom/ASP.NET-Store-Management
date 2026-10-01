using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.DTOs.Base;
using Store.Application.DTOs.Product;
using Store.Application.Features.Interfaces;
using Store.Application.Responses;

namespace Store.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Produces(typeof(Response<PaginateDTO<ProductDTO>>))]
        public async Task<JsonResult> GetAll([FromQuery] FilterProducts input)
        {
            var res = await _productService.FilterPaginate(input);

            return new Response<PaginateDTO<ProductDTO>>(res).ToJsonResult();
        }

        [HttpGet("{id}")]
        [Produces(typeof(Response<ProductDTO>))]
        public async Task<JsonResult> Get(int id)
        {
            var res = await _productService.Get(id);

            return new Response<ProductDTO>(res).ToJsonResult();
        }

        [HttpPost]
        [Authorize]
        [Produces(typeof(Response<ProductDTO>))]
        public async Task<JsonResult> Create(UpsertProduct input)
        {
            var res = await _productService.Create(input);

            return new Response<ProductDTO>(res).ToJsonResult();
        }

        [HttpPut("{id}")]
        [Authorize]
        [Produces(typeof(Response<ProductDTO>))]
        public async Task<JsonResult> Edit(int id, UpsertProduct input)
        {
            input.Id = id;

            var res = await _productService.Edit(input);

            return new Response<ProductDTO>(res).ToJsonResult();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<JsonResult> Delete(int id)
        {
            await _productService.Delete(id);

            return Response<string>.Succeed();
        }
    }
}
