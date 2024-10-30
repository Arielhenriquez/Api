using Api.Domain.Entities.InventoryEntities;
using Api.Domain.Enums;

namespace Api.Domain.Entities.TransportEntities;

public class TransportRequest : BaseEntity
{
    public Guid CollaboratorId { get; set; }
    public Collaborator Collaborator { get; set; }
    public string Destination { get; set; }
    public RequestStatus RequestStatus { get; set; }
    public string? PhoneNumber { get; set; }

    // Relación 1:1 con vehículo y chofer
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }

    public Guid DriverId { get; set; }
    public Driver Driver { get; set; }
}
