using Api.Application.Features.Transport.TransportRequest.Dtos;

namespace Api.Application.Features.Transport.Drivers.Dtos;

public class DriverSummaryDto
{
    public DriverResponseDto? Driver { get; set; }
    public IEnumerable<TransportSummaryDto> TransportRequests { get; set; } = [];
}
