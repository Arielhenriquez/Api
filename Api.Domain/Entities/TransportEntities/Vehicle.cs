using Api.Domain.Enums;

namespace Api.Domain.Entities.TransportEntities;

public class Vehicle : BaseEntity
{
    public string? Type { get; set; }
    public int Capacity { get; set; }
    public VehicleStatus Status { get; set; }
    public required string Brand { get; set; }
    public required string Model { get; set; }
    public required string LicensePlate { get; set; }
    public required DateTime InsuranceValidity { get; set; }
    public required string InsuranceType { get; set; }
    public ICollection<TransportRequest> TransportRequests { get; set; } = [];
}
