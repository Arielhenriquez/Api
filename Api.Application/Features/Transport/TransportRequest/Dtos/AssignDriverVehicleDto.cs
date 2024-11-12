namespace Api.Application.Features.Transport.TransportRequest.Dtos;

public class AssignDriverVehicleDto
{
    public required Guid DriverId { get; set; }
    public required Guid VehicleId { get; set; }
}
