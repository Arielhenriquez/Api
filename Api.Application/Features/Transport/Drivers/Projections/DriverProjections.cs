using Api.Application.Features.Transport.Drivers.Dtos;
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
    };
}
