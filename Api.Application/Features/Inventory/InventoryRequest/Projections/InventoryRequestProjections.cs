using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Application.Features.Inventory.InventoryRequest.Dtos;
using System.Linq.Expressions;
using InventoryEntity = Api.Domain.Entities.InventoryEntities.InventoryRequest;

namespace Api.Application.Features.Inventory.InventoryRequest.Projections;
// Todo Agregar mapeo de enum a texto y de articulos
public static class InventoryRequestProjections
{
    public static Expression<Func<InventoryEntity, InventoryResponseDto>> Summary => (InventoryEntity inventoryRequest) => new InventoryResponseDto
    {
        Id = inventoryRequest.Id,
        Collaborator = inventoryRequest.Collaborator,
        RequestDate = inventoryRequest.RequestDate,
        RequestStatus = inventoryRequest.RequestStatus,
        // InventoryRequestItems = inventoryRequest.InventoryRequestItems.Select(inventoryItems => inventoryItems.InventoryItem(InventoryItemResponseDto)),
        InventoryRequestItems = inventoryRequest.InventoryRequestItems.Select(inventoryItem => new InventoryItemResponseDto
        {
            Id = inventoryItem.Id,
            Name = inventoryItem.InventoryItem.Name,
            Quantity = inventoryItem.InventoryItem.Quantity,
            UnitOfMeasure = inventoryItem.InventoryItem.UnitOfMeasure,
            Value = inventoryItem.InventoryItem.Value,
        }).ToList()
    };

}
