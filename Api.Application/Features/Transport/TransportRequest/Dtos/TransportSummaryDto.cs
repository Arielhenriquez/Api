using Api.Application.Features.Collaborators.Dtos;
using Api.Application.Features.Transport.Drivers.Dtos;
using Api.Application.Features.Transport.Vehicles.Dtos;
using Api.Domain.Enums;

namespace Api.Application.Features.Transport.TransportRequest.Dtos;

public class TransportSummaryDto
{
    public Guid Id { get; set; }
    public required CollaboratorResponseDto Collaborator { get; set; }
    public required string Destination { get; set; }
    public required string DeparturePoint { get; set; }
    public int NumberOfPeople { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public RequestStatus RequestStatus { get; set; }
    public PendingApprovalBy? PendingApproval { get; set; }
    public DateTime? StatusChangedDate { get; set; }
    public string? ApprovedOrRejectedBy { get; set; }
    public string? RequestStatusDescription { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DriverResponseDto? DriverResponse { get; set; } 
    public VehicleResponseDto? VehicleResponse { get; set; } 
}