using Microsoft.EntityFrameworkCore;
using Store.Application.Contracts.Persistence;
using Store.Domain.Entities.Base;

namespace Store.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> Get(int id)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(f => f.IsActive && f.Id == id);
        }

        public async Task<T> GetNoTracking(int id)
        {
            return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(f => f.IsActive && f.Id == id);
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await _dbContext.Set<T>()
                .Where(w => w.IsActive)
                .AsNoTracking()
                .OrderByDescending(o => o.UpdatedAt)
                .ToListAsync();
        }

        public IQueryable<T> GetAllQueryable()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public async Task<T> Add(T entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var find = await _dbContext.Set<T>().FindAsync(id);
            if (find != null)
            {
                find.IsActive = false;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task Recover(int id)
        {
            var find = await _dbContext.Set<T>().FindAsync(id);
            if (find != null)
            {
                find.IsActive = true;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> Exist(int id)
        {
            return await _dbContext.Set<T>().AnyAsync(a => a.IsActive && a.Id == id);
        }
    }
}
