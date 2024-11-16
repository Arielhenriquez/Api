using Api.Application.Common.Extensions;
using Api.Domain.Enums;
using System.Linq.Expressions;
using TransportEntity = Api.Domain.Entities.TransportEntities.TransportRequest;

namespace Api.Application.Features.Transport.TransportRequest.Predicates;

public static class TransportRequestPredicates
{
    public static Expression<Func<TransportEntity, bool>> Search(string criteria)
    {
        var matchingEnum = Enum.GetValues(typeof(RequestStatus))
            .Cast<Enum>()
            .FirstOrDefault(e => e.DisplayName().Contains(criteria, StringComparison.OrdinalIgnoreCase));

        if (matchingEnum != null)
        {
            var parsedStatus = (RequestStatus)matchingEnum;

            return transportEntity =>
                transportEntity.RequestStatus == parsedStatus;
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
