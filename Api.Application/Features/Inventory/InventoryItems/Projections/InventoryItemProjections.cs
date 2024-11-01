using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Domain.Entities.InventoryEntities;
using System.Linq.Expressions;

namespace Api.Application.Features.Inventory.InventoryItems.Projections;

public class InventoryItemProjections
{
    public static Expression<Func<InventoryItem, InventoryItemResponseDto>> Search => (InventoryItem inventoryItem) => new InventoryItemResponseDto()
    {
        Id = inventoryItem.Id,
        Name = inventoryItem.Name,
        Quantity = inventoryItem.Quantity,
        UnitOfMeasure = inventoryItem.UnitOfMeasure,
        Value = inventoryItem.Value,
    };
}
