using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Application.Contracts.Persistence;
using Store.Application.DTOs.Category;
using Store.Application.Exceptions;
using Store.Application.Features.Interfaces;
using Store.Domain.Entities;

namespace Store.Application.Features.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoryDTO>> GetAll()
        {
            var items = await _categoryRepository.GetAllQueryable()
                .Include(i => i.Products)
                .Where(w => w.IsActive)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<CategoryDTO>>(items);
        }

        public async Task<CategoryDTO> Get(int id)
        {
            var find = await _categoryRepository.GetAllQueryable()
                .Include(i => i.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.IsActive && f.Id == id);

            if (find == null)
            {
                throw new NotFoundException("Category not found");
            }

            return _mapper.Map<CategoryDTO>(find);
        }

        public async Task<CategoryDTO> Create(UpsertCategory input)
        {
            var model = _mapper.Map<Category>(input);

            model = await _categoryRepository.Add(model);

            return _mapper.Map<CategoryDTO>(model);
        }

        public async Task<CategoryDTO> Edit(UpsertCategory input)
        {
            var model = await _categoryRepository.Get(input.Id.GetValueOrDefault());

            if (model == null)
            {
                throw new NotFoundException("Category not found");
            }

            _mapper.Map(input, model);

            await _categoryRepository.Update(model);

            return _mapper.Map<CategoryDTO>(model);
        }

        public async Task<bool> Delete(int id)
        {
            await _categoryRepository.Delete(id);

            return true;
        }
    }
}
