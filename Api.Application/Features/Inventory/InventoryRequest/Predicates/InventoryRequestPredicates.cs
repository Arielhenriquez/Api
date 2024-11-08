using Api.Domain.Enums;
using System.Linq.Expressions;
using InventoryEntity = Api.Domain.Entities.InventoryEntities.InventoryRequest;

namespace Api.Application.Features.Inventory.InventoryRequest.Predicates;

public static class InventoryRequestPredicates
{
    public static Expression<Func<InventoryEntity, bool>> Search(string criteria)
    {
        if (Enum.TryParse<RequestStatus>(criteria, ignoreCase: true, out var parsedStatus))
        {
            return inventoryEntity =>
                inventoryEntity.RequestStatus == parsedStatus;
        }

        return inventoryEntity => string.IsNullOrWhiteSpace(criteria);
    }
}
