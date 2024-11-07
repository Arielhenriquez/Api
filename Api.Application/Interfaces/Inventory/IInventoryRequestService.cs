using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryRequest.Dtos;
using InventoryEntity = Api.Domain.Entities.InventoryEntities.InventoryRequest;

namespace Api.Application.Interfaces.Inventory;

public interface IInventoryRequestService
{
    //Todo: Endpoints Listar solicitudes por estado, 
    //get request details: colaborador y articulos include refactor para agregar paginacion..
    Task<List<InventoryResponseDto>> GetInventoryRequestDetails(PaginationQuery paginationQuery, Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<InventoryEntity>> ListInventoryRequests();
    Task AddInventoryRequest(InventoryRequestDto inventoryRequestDto, CancellationToken cancellationToken);
}
