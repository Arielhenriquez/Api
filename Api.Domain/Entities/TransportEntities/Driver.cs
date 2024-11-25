using Api.Domain.Enums;

namespace Api.Domain.Entities.TransportEntities;

public class Driver : BaseEntity
{
    public required string Name { get; set; }
    public DriverStatus Status { get; set; }
    public required DateTime LicenseExpiration { get; set; }
    public string? PhoneNumber { get; set; }
    public ICollection<TransportRequest> TransportRequests { get; set; } = [];

}
