using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Drivers.Dtos;
using Api.Application.Features.Transport.Drivers.Predicates;
using Api.Application.Features.Transport.Drivers.Projections;
using Api.Application.Interfaces.Transport;
using Api.Domain.Entities.TransportEntities;
using Api.Infrastructure.Extensions;
using Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

namespace Api.Infrastructure.Persistence.Repositories;

public class DriverRepository : IDriverRepository
{
    protected readonly IDbContext _context;
    protected readonly DbSet<Driver> _db;

    public DriverRepository(IDbContext context)
    {
        _context = context;
        _db = _context.Set<Driver>();
    }

    public async Task<IEnumerable<DriverSummaryDto>> GetSummary(Guid id, CancellationToken cancellationToken = default)
    {
        var query = _db
        .AsNoTracking()
        .Include(ir => ir.TransportRequests)
        .ThenInclude(ir => ir.Vehicle)
        .Where(x => x.Id == id)
        .OrderByDescending(x => x.CreatedDate);

        return await query
            .Select(DriverProjections.Summary)
            .ToListAsync(cancellationToken);
    }
    public Task<List<DriverResponseDto>> GetByName(string name, CancellationToken cancellationToken = default)
    {
        var drivers = _db
                .Where(c => EF.Functions.Like(c.Name, $"%{name}%"))
                .Select(DriverProjections.Search)
                .ToListAsync(cancellationToken);

        return drivers;
    }

    public Task<Paged<DriverResponseDto>> SearchAsync(PaginationQuery query, bool isDeleted, CancellationToken cancellationToken = default)
    {
        return _db
         .AsNoTracking()
         .IgnoreQueryFilters() // Desactivamos el filtro global
         .Where(DriverPredicates.Search(query.Search))
         .Where(x => isDeleted == null || x.IsDeleted == isDeleted) // Filtro dinámico
         .OrderByDescending(p => p.CreatedDate)
         .Select(DriverProjections.Search)
         .Paginate(query.PageSize, query.PageNumber, cancellationToken);
    }

    public async Task<DriverResponseDto> DeleteWithComment(Guid id, string comment, CancellationToken cancellationToken)
    {
        var driver = await _db
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        driver.DeleteComment = comment;
        var result = _db.Remove(driver);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }
}
