using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Application.Features.Inventory.InventoryItems.Predicates;
using Api.Application.Features.Inventory.InventoryItems.Projections;
using Api.Application.Interfaces.Inventory;
using Api.Domain.Entities.InventoryEntities;
using Api.Infrastructure.Extensions;
using Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Persistence.Repositories;

public class InventoryItemRepository : IInventoryItemRepository
{
    protected readonly IDbContext _context;
    protected readonly DbSet<InventoryItem> _db;

    public InventoryItemRepository(IDbContext context)
    {
        _context = context;
        _db = _context.Set<InventoryItem>();
    }

    public Task<List<InventoryItemResponseDto>> GetByName(string name, CancellationToken cancellationToken = default)
    {
        var inventoryItems = _db
            .Where(c => EF.Functions.Like(c.Name, $"%{name}%"))
            .Select(InventoryItemProjections.Search)
            .ToListAsync(cancellationToken);

        return inventoryItems;
    }

    public Task<Paged<InventoryItemResponseDto>> SearchAsync(PaginationQuery query, CancellationToken cancellationToken = default)
    {
        return _context.Set<InventoryItem>()
       .AsNoTracking()
       .Where(InventoryItemsPredicates.Search(query.Search))
       .OrderByDescending(p => p.CreatedDate)
       .Select(InventoryItemProjections.Search)
       .Paginate(query.PageSize, query.PageNumber, cancellationToken);
    }
}
