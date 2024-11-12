using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Application.Features.Inventory.InventoryRequest.Dtos;
using System.Linq.Expressions;
using InventoryEntity = Api.Domain.Entities.InventoryEntities.InventoryRequest;

namespace Api.Application.Features.Inventory.InventoryRequest.Projections;

public static class InventoryRequestProjections
{
    public static Expression<Func<InventoryEntity, InventorySummaryDto>> Summary => (InventoryEntity inventoryRequest) => new InventorySummaryDto
    {
        Id = inventoryRequest.Id,
        Collaborator = inventoryRequest.Collaborator,
        CreatedDate = inventoryRequest.CreatedDate,
        RequestStatus = inventoryRequest.RequestStatus,
        InventoryRequestItems = inventoryRequest.InventoryRequestItems.Select(inventoryItem => new InventoryItemResponseDto
        {
            Id = inventoryItem.InventoryItem.Id,
            Name = inventoryItem.InventoryItem.Name,
            Quantity = inventoryItem.InventoryItem.Quantity,
            UnitOfMeasure = inventoryItem.InventoryItem.UnitOfMeasure,
            Value = inventoryItem.InventoryItem.Value
        }).ToList()
    };

}
