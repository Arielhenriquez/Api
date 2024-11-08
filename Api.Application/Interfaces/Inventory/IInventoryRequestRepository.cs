using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryRequest.Dtos;

namespace Api.Application.Interfaces.Inventory;

public interface IInventoryRequestRepository
{
    Task<IEnumerable<InventorySummaryDto>> GetSummary(Guid id, CancellationToken cancellationToken = default);
    Task<Paged<InventorySummaryDto>> SearchAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default);
}
