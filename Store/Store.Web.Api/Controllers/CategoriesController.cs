using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.DTOs.Category;
using Store.Application.Features.Interfaces;
using Store.Application.Responses;

namespace Store.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Produces(typeof(Response<List<CategoryDTO>>))]
        public async Task<JsonResult> GetAll()
        {
            var res = await _categoryService.GetAll();

            return new Response<List<CategoryDTO>>(res).ToJsonResult();
        }

        [HttpGet("{id}")]
        [Produces(typeof(Response<CategoryDTO>))]
        public async Task<JsonResult> Get(int id)
        {
            var res = await _categoryService.Get(id);

            return new Response<CategoryDTO>(res).ToJsonResult();
        }

        [HttpPost]
        [Authorize]
        [Produces(typeof(Response<CategoryDTO>))]
        public async Task<JsonResult> Create(UpsertCategory input)
        {
            var res = await _categoryService.Create(input);

            return new Response<CategoryDTO>(res).ToJsonResult();
        }

        [HttpPut("{id}")]
        [Authorize]
        [Produces(typeof(Response<CategoryDTO>))]
        public async Task<JsonResult> Edit(int id, UpsertCategory input)
        {
            input.Id = id;

            var res = await _categoryService.Edit(input);

            return new Response<CategoryDTO>(res).ToJsonResult();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<JsonResult> Delete(int id)
        {
            await _categoryService.Delete(id);

            return Response<string>.Succeed();
        }
    }
}
