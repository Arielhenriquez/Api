using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Drivers.Dtos;
using Api.Application.Features.Transport.Drivers.Predicates;
using Api.Application.Features.Transport.Drivers.Projections;
using Api.Application.Interfaces.Transport;
using Api.Domain.Entities.TransportEntities;
using Api.Infrastructure.Extensions;
using Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

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
    public Task<List<DriverResponseDto>> GetByName(string name, CancellationToken cancellationToken = default)
    {
        var drivers = _db
                .Where(c => EF.Functions.Like(c.Name, $"%{name}%"))
                .Select(DriverProjections.Search)
                .ToListAsync(cancellationToken);

        return drivers;
    }

    public Task<Paged<DriverResponseDto>> SearchAsync(PaginationQuery query, CancellationToken cancellationToken = default)
    {
        return _db
                .AsNoTracking()
                .Where(DriverPredicates.Search(query.Search))
                .OrderByDescending(p => p.CreatedDate)
                .Select(DriverProjections.Search)
                .Paginate(query.PageSize, query.PageNumber, cancellationToken);
    }
}
