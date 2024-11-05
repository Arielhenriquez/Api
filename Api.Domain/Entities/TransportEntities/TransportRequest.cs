using Api.Domain.Entities.InventoryEntities;
using Api.Domain.Enums;

namespace Api.Domain.Entities.TransportEntities;

public class TransportRequest : BaseEntity
{
    public Guid CollaboratorId { get; set; }
    public Collaborator Collaborator { get; set; }
    public string Destination { get; set; }
    public string DeparturePoint { get; set; } // Punto de partida

    public int NumberOfPeople { get; set; } // Cantidad de personas
    public DateTime DepartureDateTime { get; set; } // Fecha y hora de salida
    public RequestStatus RequestStatus { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime RequestDate { get; set; } = DateTime.Now;
    // Relación 1:1 con vehículo y chofer
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }

    public Guid DriverId { get; set; }
    public Driver Driver { get; set; }
}
