using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.TransportRequest.Dtos;
using Api.Application.Features.Transport.TransportRequest.Projections;
using Api.Application.Interfaces.Transport;
using Api.Domain.Entities.TransportEntities;
using Api.Infrastructure.Extensions;
using Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Persistence.Repositories;

public class TransportRequestRepository : ITransportRequestRepository
{
    protected readonly IDbContext _context;
    protected readonly DbSet<TransportRequest> _db;

    public TransportRequestRepository(IDbContext context)
    {
        _context = context;
        _db = context.Set<TransportRequest>();
    }

    public async Task<IEnumerable<TransportSummaryDto>> GetSummary(Guid id, CancellationToken cancellationToken = default)
    {
        var query = _db
       .AsNoTracking()
       .Include(ir => ir.Collaborator)
       .Include(ir => ir.Driver)
       .Include(iri => iri.Vehicle)
       .Where(x => x.Id == id)
       .OrderByDescending(x => x.CreatedDate);

        return await query
            .Select(TransportRequestProjections.Summary)
            .ToListAsync(cancellationToken);
    }

    public Task<Paged<TransportSummaryDto>> SearchAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
    {
        return _db
            .AsNoTracking()
            .Include(ir => ir.Collaborator)
            .Include(ir => ir.Driver)
            .Include(iri => iri.Vehicle)
            .OrderByDescending(p => p.CreatedDate)
            .Select(TransportRequestProjections.Summary)
            .Paginate(paginationQuery.PageSize, paginationQuery.PageNumber, cancellationToken);
    }
}
