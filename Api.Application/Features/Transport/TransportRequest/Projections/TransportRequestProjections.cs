using Api.Application.Features.Transport.TransportRequest.Dtos;
using System.Linq.Expressions;
using TransportEntity = Api.Domain.Entities.TransportEntities.TransportRequest;

namespace Api.Application.Features.Transport.TransportRequest.Projections;

public static class TransportRequestProjections
{
    public static Expression<Func<TransportEntity, TransportSummaryDto>> Summary => (TransportEntity transportEntity) => new TransportSummaryDto
    {
        Id = transportEntity.Id,
        Collaborator = transportEntity.Collaborator,
        DeparturePoint = transportEntity.DeparturePoint,
        Destination = transportEntity.Destination,
        NumberOfPeople = transportEntity.NumberOfPeople,
        DepartureDateTime = transportEntity.DepartureDateTime,
        CreatedDate = transportEntity.CreatedDate,
        RequestStatus = transportEntity.RequestStatus,
        PendingApproval = transportEntity.PendingApprovalBy,
        StatusChangedDate = transportEntity.StatusChangedDate,
        ApprovedOrRejectedBy = transportEntity.ApprovedOrRejectedBy,
        PhoneNumber = transportEntity.PhoneNumber,
        DriverResponse = transportEntity.Driver,
        VehicleResponse = transportEntity.Vehicle
    };
}
