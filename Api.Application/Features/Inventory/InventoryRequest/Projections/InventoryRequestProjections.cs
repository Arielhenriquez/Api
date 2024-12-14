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
        //ApprovedOrRejectedBy = inventoryRequest.ApprovedOrRejectedBy,
        StatusChangedDate = inventoryRequest.StatusChangedDate,
        PendingApproval = inventoryRequest.PendingApprovalBy,
        InventoryRequestItems = inventoryRequest.InventoryRequestItems.Select(inventoryItem => new InventoryItemResponseDto
        {
            Id = inventoryItem.InventoryItem.Id,
            InstitutionalCode = inventoryItem.InventoryItem.InstitutionalCode,
            Category = inventoryItem.InventoryItem.Category,
            WarehouseObjectAccount = inventoryItem.InventoryItem.Category,
            AcquisitionObjectAccount = inventoryItem.InventoryItem.Category,
            Name = inventoryItem.InventoryItem.Category,
            Quantity = inventoryItem.InventoryItem.Quantity,
            RequestedQuantity = inventoryItem.InventoryItem.RequestedQuantity,
            UnitOfMeasure = inventoryItem.InventoryItem.Category,
            Value = inventoryItem.InventoryItem.Value,
            Section = inventoryItem.InventoryItem.Section,
        }).ToList()
    };

}
