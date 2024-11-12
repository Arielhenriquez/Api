using Api.Domain.Enums;
using System.Linq.Expressions;
using TransportEntity = Api.Domain.Entities.TransportEntities.TransportRequest;

namespace Api.Application.Features.Transport.TransportRequest.Predicates;

public static class TransportRequestPredicates
{
    public static Expression<Func<TransportEntity, bool>> Search(string criteria)
    {
        if (Enum.TryParse<RequestStatus>(criteria, ignoreCase: true, out var parsedStatus))
        {
            return transportEntity =>
                transportEntity.RequestStatus == parsedStatus ||
                string.IsNullOrWhiteSpace(criteria);
        }
        else
        {
            return transportEntity =>
                transportEntity.DeparturePoint.Contains(criteria) ||
                transportEntity.Destination.Contains(criteria) ||
                transportEntity.PhoneNumber.Contains(criteria) ||
                string.IsNullOrWhiteSpace(criteria);
        }
    }
}
