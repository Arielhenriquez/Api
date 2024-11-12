using Api.Domain.Entities.TransportEntities;
using Api.Domain.Enums;
using System.Linq.Expressions;

namespace Api.Application.Features.Transport.Vehicles.Predicates;

public static class VehiclePredicates
{
    public static Expression<Func<Vehicle, bool>> Search(string criteria)
    {
        if (Enum.TryParse<VehicleStatus>(criteria, ignoreCase: true, out var parsedStatus))
        {
            return transportEntity =>
                transportEntity.Status == parsedStatus ||
                string.IsNullOrWhiteSpace(criteria);
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
