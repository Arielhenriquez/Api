using System.Linq.Expressions;
using TransportEntity = Api.Domain.Entities.TransportEntities.TransportRequest;

namespace Api.Application.Features.Transport.TransportRequest.Predicates;

public static class TransportRequestPredicates
{
    public static Expression<Func<TransportEntity, bool>> Search(string criteria)
    {
        if (string.IsNullOrWhiteSpace(criteria))
            return _ => true;

        return transportEntity =>
            transportEntity.DeparturePoint.Contains(criteria) ||
            transportEntity.Destination.Contains(criteria) ||
            transportEntity.PhoneNumber.Contains(criteria);
    }
}
