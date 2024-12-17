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
    public Task<Paged<VehicleResponseDto>> SearchAsync(PaginationQuery query, bool isDeleted, CancellationToken cancellationToken = default)
    {
        return _db
        .AsNoTracking()
        .IgnoreQueryFilters()
        .Where(VehiclePredicates.Search(query.Search))
        .Where(x => isDeleted == null || x.IsDeleted == isDeleted)
        .OrderByDescending(p => p.CreatedDate)
        .Select(VechicleProjections.Search)
        .Paginate(query.PageSize, query.PageNumber, cancellationToken);
    }
    public async Task<VehicleResponseDto> DeleteWithComment(Guid id, string comment, CancellationToken cancellationToken)
    {
        var vehicle = await _db
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        vehicle.DeleteComment = comment;
        var result = _db.Remove(vehicle);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }
}
