using Api.Domain.Entities.TransportEntities;
using System.Linq.Expressions;

namespace Api.Application.Features.Transport.Drivers.Predicates;

public static class DriverPredicates
{
    public static Expression<Func<Driver, bool>> Search(string criteria)
    {
        return (Driver driver) =>
            driver.Name.Contains(criteria) ||
            // driver.Status.ToString().Contains(criteria) ||
            string.IsNullOrWhiteSpace(criteria);
    }
}
