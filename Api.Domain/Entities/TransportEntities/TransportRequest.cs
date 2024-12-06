namespace Api.Domain.Entities.TransportEntities;

public class TransportRequest : BaseRequestEntity
{
    public required string Destination { get; set; }
    public required string DeparturePoint { get; set; }
    public int NumberOfPeople { get; set; }
    public required DateTime DepartureDateTime { get; set; }
    public string? ApprovedOrRejectedBy { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }
    public Guid? DriverId { get; set; }
    public Driver? Driver { get; set; }
}
