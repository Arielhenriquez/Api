using System.Linq.Expressions;
using InventoryEntity = Api.Domain.Entities.InventoryEntities.InventoryRequest;

namespace Api.Application.Features.Inventory.InventoryRequest.Predicates;

public static class InventoryRequestPredicates
{
    public static Expression<Func<InventoryEntity, bool>> Search(string criteria)
    {
        if (string.IsNullOrWhiteSpace(criteria))
            return _ => true;
        return inventoryEntity => true;
    }
}
