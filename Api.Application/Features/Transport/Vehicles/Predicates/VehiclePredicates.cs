using Api.Domain.Entities.TransportEntities;
using System.Linq.Expressions;

namespace Api.Application.Features.Transport.Vehicles.Predicates;

public static class VehiclePredicates
{
    public static Expression<Func<Vehicle, bool>> Search(string criteria)
    {
        return (Vehicle vehicle) =>
            vehicle.Type.Contains(criteria) ||
            vehicle.Model.Contains(criteria) ||
            vehicle.Brand.Contains(criteria) ||
            vehicle.LicensePlate.Contains(criteria) ||
            vehicle.InsuranceType.Contains(criteria) ||
            // vehicle.Status.ToString().Contains(criteria) ||
            string.IsNullOrWhiteSpace(criteria);
    }
}
