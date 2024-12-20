using Api.Application.Features.Collaborators.Dtos;
using Api.Domain.Enums;
using TransportEntity = Api.Domain.Entities.TransportEntities.TransportRequest;

namespace Api.Application.Features.Transport.TransportRequest.Dtos;

public class TransportResponseDto
{
    public Guid Id { get; set; }
    public required CollaboratorResponseDto Collaborator { get; set; }
    public required string Destination { get; set; }
    public required string DeparturePoint { get; set; }
    public int NumberOfPeople { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public TransportRequestStatus RequestStatus { get; set; }
    public string? RequestStatusDescription { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public required string RequestCode { get; set; }

    public static implicit operator TransportResponseDto(TransportEntity transportEntity)
    {
        return new TransportResponseDto
        {
            Id = transportEntity.Id,
            Collaborator = transportEntity.Collaborator != null ? (CollaboratorResponseDto)transportEntity.Collaborator : null,
            Destination = transportEntity.Destination,
            DeparturePoint = transportEntity.DeparturePoint,
            NumberOfPeople = transportEntity.NumberOfPeople,
            DepartureDateTime = transportEntity.DepartureDateTime,
            RequestStatus = transportEntity.TransportRequestStatus,
            PhoneNumber = transportEntity.PhoneNumber,
            CreatedDate = transportEntity.CreatedDate,
            RequestCode = transportEntity.RequestCode,
        };
    }
}
