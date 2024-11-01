using Api.Application.Common.Pagination;
using Api.Application.Features.Collaborators.Dtos;
using Api.Application.Features.Inventory.InventoryItems.Dtos;

namespace Api.Application.Interfaces;

public interface IInventoryItemsService : IBaseService<InventoryItemRequestDto, InventoryItemResponseDto>
{
    Task<Paged<InventoryItemResponseDto>> GetPagedInventoryItems(PaginationQuery paginationQuery, CancellationToken cancellationToken);
    Task<List<InventoryItemResponseDto>> FindInventoryItemByName(string criteria);
}
