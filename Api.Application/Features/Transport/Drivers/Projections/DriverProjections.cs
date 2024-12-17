using Api.Application.Features.Collaborators.Dtos;
using Api.Application.Features.Transport.Drivers.Dtos;
using Api.Application.Features.Transport.TransportRequest.Dtos;
using Api.Domain.Entities.TransportEntities;
using System.Linq.Expressions;

namespace Api.Application.Features.Transport.Drivers.Projections;

public static class DriverProjections
{
    public static Expression<Func<Driver, DriverResponseDto>> Search => (Driver driver) => new DriverResponseDto()
    {
        Id = driver.Id,
        Name = driver.Name,
        Status = driver.Status,
        LicenseExpiration = driver.LicenseExpiration,
        PhoneNumber = driver.PhoneNumber,
        DeleteComment = driver.DeleteComment
    };

    public static Expression<Func<Driver, DriverSummaryDto>> Summary => (Driver driver) => new DriverSummaryDto()
    {
        Driver = driver,
        TransportRequests = driver.TransportRequests.Select(transport => new TransportResponseDto
        {
            Id = transport.Id,
            Collaborator = transport.Collaborator != null ? (CollaboratorResponseDto)transport.Collaborator : null,
            Destination = transport.Destination,
            DeparturePoint = transport.DeparturePoint,
            NumberOfPeople = transport.NumberOfPeople,
            DepartureDateTime = transport.DepartureDateTime,
            RequestStatus = transport.RequestStatus,
            PhoneNumber = transport.PhoneNumber,
            CreatedDate = transport.CreatedDate,
        }).ToList()
    };
}
