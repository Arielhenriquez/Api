using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Vehicles.Dtos;
using Api.Application.Features.Transport.Vehicles.Predicates;
using Api.Application.Features.Transport.Vehicles.Projections;
using Api.Application.Interfaces.Transport;
using Api.Domain.Entities.TransportEntities;
using Api.Infrastructure.Extensions;
using Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Persistence.Repositories;

public class VehicleRepository : IVehicleRepository
{
    protected readonly IDbContext _context;
    protected readonly DbSet<Vehicle> _db;

    public VehicleRepository(IDbContext context)
    {
        _context = context;
        _db = _context.Set<Vehicle>();
    }
    //public Task<List<VehicleResponseDto>> GetByName(string name, CancellationToken cancellationToken = default)
    //{
    //    var vehicles = _db
    //            .Where(c => EF.Functions.Like(c.Name, $"%{name}%"))
    //            .Select(VechicleProjections.Search)
    //            .ToListAsync(cancellationToken);

    //    return vehicles;
    //}

    public Task<Paged<VehicleResponseDto>> SearchAsync(PaginationQuery query, CancellationToken cancellationToken = default)
    {
        return _db
                .AsNoTracking()
                .Where(VehiclePredicates.Search(query.Search))
                .OrderByDescending(p => p.CreatedDate)
                .Select(VechicleProjections.Search)
                .Paginate(query.PageSize, query.PageNumber, cancellationToken);
    }
}
