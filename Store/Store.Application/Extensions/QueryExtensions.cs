using Microsoft.EntityFrameworkCore;
using Store.Application.DTOs.Base;

namespace Store.Application.Extensions
{
    public static class QueryExtensions
    {
        public static async Task<PaginateDTO<T>> ToPaginatedAsync<T>(this IQueryable<T> query, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : Math.Min(pageSize, 100);

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginateDTO<T>
            {
                Items = items,
                TotalRecords = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
