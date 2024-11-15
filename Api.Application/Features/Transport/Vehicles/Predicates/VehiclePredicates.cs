using Api.Application.Common.Extensions;
using Api.Domain.Entities.TransportEntities;
using Api.Domain.Enums;
using System.Linq.Expressions;

namespace Api.Application.Features.Transport.Vehicles.Predicates;

public static class VehiclePredicates
{
    public static Expression<Func<Vehicle, bool>> Search(string criteria)
    {
        var matchingEnum = Enum.GetValues(typeof(VehicleStatus))
            .Cast<Enum>()
            .FirstOrDefault(e => e.DisplayName().Equals(criteria, StringComparison.OrdinalIgnoreCase));

        if (matchingEnum != null)
        {
            var parsedStatus = (VehicleStatus)matchingEnum;

            return transportEntity =>
                transportEntity.Status == parsedStatus;
        }
        else
        {
            return (Vehicle vehicle) =>
             vehicle.Type.Contains(criteria) ||
             vehicle.Model.Contains(criteria) ||
             vehicle.Brand.Contains(criteria) ||
             vehicle.LicensePlate.Contains(criteria) ||
             vehicle.InsuranceType.Contains(criteria) ||
             string.IsNullOrWhiteSpace(criteria);
        }
    }
}
