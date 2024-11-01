using Api.Application.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Extensions;

public static class PaginationExtension
{
    public static async Task<Paged<T>> Paginate<T>(this IQueryable<T> query, int pageSize, int pageNumber, CancellationToken cancellationToken) where T : class
    {
        int page = pageNumber <= 0 ? 1 : pageNumber;
        int skip = (page - 1) * pageSize;

        int totalRecords = await query.CountAsync(cancellationToken);
        List<T> items = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken); 

        return Paged<T>.Create(items, totalRecords, page, pageSize);
    }

    public static Paged<T> Paginate<T>(this ICollection<T> query, int pageNumber, int pageSize) where T : class
    {
        int page = pageNumber <= 0 ? 1 : pageNumber;
        int skip = (page - 1) * pageSize;
        return Paged<T>.Create(totalRecords: query.Count, items: query.Skip(skip).Take(pageSize).ToList(), currentPage: page, pageSize: pageSize);
    }
}
