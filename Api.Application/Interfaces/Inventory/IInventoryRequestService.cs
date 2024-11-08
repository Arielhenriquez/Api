using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryRequest.Dtos;

namespace Api.Application.Interfaces.Inventory;

public interface IInventoryRequestService
{
    //Todo: Endpoint y editar solicitud
    Task<Paged<InventorySummaryDto>> GetPagedInventoryRequest(PaginationQuery paginationQuery, CancellationToken cancellationToken);
    Task<IEnumerable<InventorySummaryDto>> GetInventoryRequestDetails(Guid id, CancellationToken cancellationToken);
    Task<InventoryResponseDto> AddInventoryRequest(InventoryRequestDto inventoryRequestDto, CancellationToken cancellationToken);
}
