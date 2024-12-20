using Api.Domain.Enums;
using System.Text.Json.Serialization;

namespace Api.Application.Features.Transport.Drivers.Dtos;

public class DriverRequestDto
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DriverStatus Status { get; set; }
    public required string LicenseNumber { get; set; }
    public required DateTime LicenseExpiration { get; set; }
    public string? PhoneNumber { get; set; }
}
