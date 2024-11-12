using Api.Domain.Entities.TransportEntities;
using Api.Domain.Enums;
using System.Linq.Expressions;

namespace Api.Application.Features.Transport.Drivers.Predicates;

public static class DriverPredicates
{
    public static Expression<Func<Driver, bool>> Search(string criteria)
    {
        if (Enum.TryParse<DriverStatus>(criteria, ignoreCase: true, out var parsedStatus))
        {
            return transportEntity =>
                transportEntity.Status == parsedStatus ||
                string.IsNullOrWhiteSpace(criteria);
        }
        else
        {
            return (Driver driver) =>
            driver.Name.Contains(criteria) ||
            driver.PhoneNumber.Contains(criteria) ||
            string.IsNullOrWhiteSpace(criteria);
        }
    }
}
