using Api.Application.Features.Collaborators.Dtos;
using Api.Domain.Enums;

namespace Api.Application.Features.Transport.TransportRequest.Dtos;

public class TransportResponseDto
{
    public CollaboratorResponseDto Collaborator { get; set; }
    public required string Destination { get; set; }
    public required string DeparturePoint { get; set; } // Punto de partida

    public int NumberOfPeople { get; set; } // Cantidad de personas
    public DateTime DepartureDateTime { get; set; } // Fecha y hora de salida
    public RequestStatus RequestStatus { get; set; } = RequestStatus.Pending;
    public string RequestStatusDescription { get; set; } = "Pending";
    public string? PhoneNumber { get; set; }
    public DateTime RequestDate { get; set; } = DateTime.Now;
}
