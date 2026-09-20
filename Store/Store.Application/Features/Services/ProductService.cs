using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Application.Contracts.Persistence;
using Store.Application.DTOs.Base;
using Store.Application.DTOs.Product;
using Store.Application.Exceptions;
using Store.Application.Extensions;
using Store.Application.Features.Interfaces;
using Store.Domain.Entities;

namespace Store.Application.Features.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<PaginateDTO<ProductDTO>> FilterPaginate(FilterProducts input)
        {
            var query = _productRepository.GetAllWithCategory()
                .Where(w => w.IsActive)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(input.Search))
            {
                query = query.Where(w => w.Title.Contains(input.Search) || w.Description.Contains(input.Search));
            }

            if (input.CategoryId.HasValue)
            {
                query = query.Where(w => w.CategoryId == input.CategoryId.Value);
            }

            if (input.MinPrice.HasValue)
            {
                query = query.Where(w => w.Price >= input.MinPrice.Value);
            }

            if (input.MaxPrice.HasValue)
            {
                query = query.Where(w => w.Price <= input.MaxPrice.Value);
            }

            var page = await query
                .OrderByDescending(o => o.UpdatedAt)
                .ToPaginatedAsync(input.Page, input.PageSize);

            return new PaginateDTO<ProductDTO>
            {
                Items = _mapper.Map<List<ProductDTO>>(page.Items),
                TotalRecords = page.TotalRecords,
                Page = page.Page,
                PageSize = page.PageSize
            };
        }

        public async Task<ProductDTO> Get(int id)
        {
            var find = await _productRepository.GetAllWithCategory()
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.IsActive && f.Id == id);

            if (find == null)
            {
                throw new NotFoundException("Product not found");
            }

            return _mapper.Map<ProductDTO>(find);
        }

        public async Task<ProductDTO> Create(UpsertProduct input)
        {
            await EnsureCategoryExists(input.CategoryId);

            var model = _mapper.Map<Product>(input);

            model = await _productRepository.Add(model);

            return await Get(model.Id);
        }

        public async Task<ProductDTO> Edit(UpsertProduct input)
        {
            var model = await _productRepository.Get(input.Id.GetValueOrDefault());

            if (model == null)
            {
                throw new NotFoundException("Product not found");
            }

            await EnsureCategoryExists(input.CategoryId);

            _mapper.Map(input, model);

            await _productRepository.Update(model);

            return await Get(model.Id);
        }

        public async Task<bool> Delete(int id)
        {
            await _productRepository.Delete(id);

            return true;
        }

        private async Task EnsureCategoryExists(int categoryId)
        {
            if (!await _categoryRepository.Exist(categoryId))
            {
                throw new BadRequestException("Category does not exist");
            }
        }
    }
}
