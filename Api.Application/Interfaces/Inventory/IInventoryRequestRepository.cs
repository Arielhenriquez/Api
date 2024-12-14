using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryRequest.Dtos;
using Api.Domain.Entities.InventoryEntities;

namespace Api.Application.Interfaces.Inventory;

public interface IInventoryRequestRepository
{
    Task<IEnumerable<InventorySummaryDto>> GetSummary(Guid id, CancellationToken cancellationToken = default);
    Task<Paged<InventorySummaryDto>> SearchAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default);
    Task<InventoryRequest> UpdateRequestAsync(Guid requestId, InventoryRequest updatedRequest, string loggedUserName, CancellationToken cancellationToken = default);
}
