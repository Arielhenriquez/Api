using System.Linq.Expressions;
using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Domain.Entities.InventoryEntities;

namespace Api.Application.Features.Inventory.InventoryItems.Projections;

public static class InventoryItemProjections
{
    public static Expression<Func<InventoryItem, InventoryItemResponseDto>> Search =>
        (InventoryItem inventoryItem) => new InventoryItemResponseDto()
        {
            Id = inventoryItem.Id,
            InstitutionalCode = inventoryItem.InstitutionalCode,
            Category = inventoryItem.Category,
            WarehouseObjectAccount = inventoryItem.WarehouseObjectAccount,
            AcquisitionObjectAccount = inventoryItem.AcquisitionObjectAccount,
            Name = inventoryItem.Name,
            Quantity = inventoryItem.Quantity,
            RequestedQuantity = inventoryItem.RequestedQuantity,
            UnitOfMeasure = inventoryItem.UnitOfMeasure,
            Value = inventoryItem.Value,
            Section = inventoryItem.Section,
            DeleteComment = inventoryItem.DeleteComment
        };
}
