using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryItems.Dtos;

namespace Api.Application.Interfaces.Inventory;

public interface IInventoryItemRepository
{
    Task<List<InventoryItemResponseDto>> GetByName(string name, CancellationToken cancellationToken = default);
    Task<Paged<InventoryItemResponseDto>> SearchAsync(PaginationQuery query, CancellationToken cancellationToken = default);
}
