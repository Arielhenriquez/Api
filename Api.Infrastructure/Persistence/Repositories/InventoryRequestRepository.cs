using Api.Application.Common.Exceptions;
using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryRequest.Dtos;
using Api.Application.Features.Inventory.InventoryRequest.Predicates;
using Api.Application.Features.Inventory.InventoryRequest.Projections;
using Api.Application.Interfaces.Inventory;
using Api.Domain.Entities.InventoryEntities;
using Api.Infrastructure.Extensions;
using Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Persistence.Repositories;

public class InventoryRequestRepository : IInventoryRequestRepository
{
    protected readonly IDbContext _context;
    protected readonly DbSet<InventoryRequest> _db;

    public InventoryRequestRepository(IDbContext context)
    {
        _context = context;
        _db = context.Set<InventoryRequest>();
    }

    public  Task<Paged<InventorySummaryDto>> SearchAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
    {
        return _db
        .AsNoTracking()
        .Where(InventoryRequestPredicates.Search(paginationQuery.Search))
        .OrderByDescending(p => p.CreatedDate)
        .Select(InventoryRequestProjections.Summary)
        .Paginate(paginationQuery.PageSize, paginationQuery.PageNumber, cancellationToken);


    }
    public async Task<IEnumerable<InventorySummaryDto>> GetSummary(Guid id, CancellationToken cancellationToken = default)
    {
        var query = _db
        .AsNoTracking()
        .Include(ir => ir.Collaborator)
        .Include(ir => ir.InventoryRequestItems)
        .ThenInclude(iri => iri.InventoryItem)
        .Where(x => x.Id == id)
        .OrderByDescending(x => x.CreatedDate);

        return await query
            .Select(InventoryRequestProjections.Summary)
            .ToListAsync(cancellationToken);
    }

    public async Task<InventoryRequest> UpdateRequestAsync(Guid requestId, InventoryRequest updatedRequest, CancellationToken cancellationToken = default)
    {
        var existingRequest = await _db.FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken)
            ?? throw new NotFoundException($"Inventory request with ID {requestId} not found.");

        existingRequest.ApprovedOrRejectedBy = new List<string>(updatedRequest.ApprovedOrRejectedBy);
        existingRequest.RequestStatus = updatedRequest.RequestStatus;
        existingRequest.PendingApprovalBy = updatedRequest.PendingApprovalBy;
        existingRequest.Comment = updatedRequest.Comment;
        existingRequest.StatusChangedDate = updatedRequest.StatusChangedDate;

        await _context.SaveChangesAsync(cancellationToken);

        return existingRequest;
    }
}
