using Api.Domain.Entities.InventoryEntities;
using System.Linq.Expressions;

namespace Api.Application.Features.Inventory.InventoryItems.Predicates;

public static class InventoryItemsPredicates
{
    public static Expression<Func<InventoryItem, bool>> Search(string criteria)
    {
        return (InventoryItem inventoryItem) =>
            inventoryItem.Name.Contains(criteria) ||
            inventoryItem.UnitOfMeasure.Contains(criteria) ||
            inventoryItem.Quantity.ToString().Contains(criteria) ||
            string.IsNullOrWhiteSpace(criteria);
    }
}
