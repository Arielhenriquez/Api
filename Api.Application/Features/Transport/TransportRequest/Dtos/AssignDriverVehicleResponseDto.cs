using Api.Application.Features.Transport.Drivers.Dtos;
using Api.Application.Features.Transport.Vehicles.Dtos;

namespace Api.Application.Features.Transport.TransportRequest.Dtos;
//Todo: borrar si el front no lo va a usar
public class AssignDriverVehicleResponseDto
{
    public Guid Id { get; set; }
    public DriverResponseDto? DriverResponse { get; set; }
    public VehicleResponseDto? VehicleResponse { get; set; }
}
