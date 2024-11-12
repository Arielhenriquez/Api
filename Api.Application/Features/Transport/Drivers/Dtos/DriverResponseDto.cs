using Api.Domain.Entities.TransportEntities;
using Api.Domain.Enums;

namespace Api.Application.Features.Transport.Drivers.Dtos;

public class DriverResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required DriverStatus Status { get; set; }
    public required DateTime LicenseExpiration { get; set; }
    public string? PhoneNumber { get; set; }

    public static implicit operator DriverResponseDto(Driver driver)
    {
        return driver is null ?
        null :
        new DriverResponseDto
        {
            Id = driver.Id,
            Name = driver.Name,
            Status = driver.Status,
            LicenseExpiration = driver.LicenseExpiration,
            PhoneNumber = driver.PhoneNumber,
        };
    }
}
