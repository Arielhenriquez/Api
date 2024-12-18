using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryItems.Dtos;

namespace Api.Application.Interfaces.Inventory;

public interface IInventoryItemRepository
{
    Task<List<InventoryItemResponseDto>> GetByName(string name, CancellationToken cancellationToken = default);
    Task<Paged<InventoryItemResponseDto>> SearchAsync(PaginationQuery query, bool isDeleted, CancellationToken cancellationToken = default);
    Task<InventoryItemResponseDto> DeleteItemWithComment(Guid id, string comment, CancellationToken cancellationToken);
}
