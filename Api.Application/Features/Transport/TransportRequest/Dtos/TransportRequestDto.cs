namespace Api.Application.Features.Transport.TransportRequest.Dtos;

public class TransportRequestDto
{
    public Guid CollaboratorId { get; set; }
    public required string DeparturePoint { get; set; }
    public required string Destination { get; set; }
    public required int NumberOfPeople { get; set; }
    public required DateTime DepartureDateTime { get; set; }
    public string? PhoneNumber { get; set; }
}
