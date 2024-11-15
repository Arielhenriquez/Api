using Api.Application.Common.Extensions;
using Api.Domain.Enums;
using System.Linq.Expressions;
using InventoryEntity = Api.Domain.Entities.InventoryEntities.InventoryRequest;

namespace Api.Application.Features.Inventory.InventoryRequest.Predicates;

public static class InventoryRequestPredicates
{
    public static Expression<Func<InventoryEntity, bool>> Search(string criteria)
    {
        var matchingEnum = Enum.GetValues(typeof(RequestStatus))
         .Cast<Enum>()
         .FirstOrDefault(e => e.DisplayName().Equals(criteria, StringComparison.OrdinalIgnoreCase));

        if (matchingEnum != null)
        {
            var parsedStatus = (RequestStatus)matchingEnum;

            return transportEntity =>
                transportEntity.RequestStatus == parsedStatus;
        }

        return inventoryEntity => string.IsNullOrWhiteSpace(criteria);
    }
}
