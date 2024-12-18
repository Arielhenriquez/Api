using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryItems.Dtos;

namespace Api.Application.Interfaces.Inventory;

public interface IInventoryItemsService : IBaseService<InventoryItemRequestDto, InventoryItemResponseDto>
{
    Task<Paged<InventoryItemResponseDto>> GetPagedInventoryItems(PaginationQuery paginationQuery, bool isDeleted, CancellationToken cancellationToken);
    Task<IEnumerable<InventoryItemResponseDto>> FindInventoryItemByName(string criteria);
    Task UpdateArticleQuantity(Guid id, UpdateArticleQuantityDto updateArticle, CancellationToken cancellationToken);
    Task<InventoryItemResponseDto> DeleteItemWithComment(Guid id, string comment, CancellationToken cancellationToken);
}
