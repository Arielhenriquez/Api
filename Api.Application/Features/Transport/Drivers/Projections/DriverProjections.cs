using System.Linq.Expressions;
using Api.Application.Features.Transport.Drivers.Dtos;
using Api.Application.Features.Transport.TransportRequest.Dtos;
using Api.Domain.Entities.TransportEntities;

namespace Api.Application.Features.Transport.Drivers.Projections;

public static class DriverProjections
{
    public static Expression<Func<Driver, DriverResponseDto>> Search => (Driver driver) => new DriverResponseDto()
    {
        Id = driver.Id,
        Name = driver.Name,
        Status = driver.Status,
        LicenseNumber = driver.LicenseNumber,
        LicenseExpiration = driver.LicenseExpiration,
        PhoneNumber = driver.PhoneNumber,
        DeleteComment = driver.DeleteComment
    };

    public static Expression<Func<Driver, DriverSummaryDto>> Summary => (Driver driver) => new DriverSummaryDto()
    {
        Driver = driver,
        TransportRequests = driver.TransportRequests.Select(transport => new TransportSummaryDto
        {
            Id = transport.Id,
            Collaborator = transport.Collaborator,
            DeparturePoint = transport.DeparturePoint,
            Destination = transport.Destination,
            NumberOfPeople = transport.NumberOfPeople,
            DepartureDateTime = transport.DepartureDateTime,
            CreatedDate = transport.CreatedDate,
            RequestStatus = transport.TransportRequestStatus,
            PendingApproval = transport.PendingApprovalBy,
            StatusChangedDate = transport.StatusChangedDate,
            ApprovedOrRejectedBy = transport.ApprovedOrRejectedBy,
            PhoneNumber = transport.PhoneNumber,  
            VehicleResponse = transport.Vehicle
        }).ToList()
    };
}
