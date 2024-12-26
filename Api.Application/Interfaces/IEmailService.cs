using Api.Application.Features.Transport.TransportRequest.Dtos;
using Api.Domain.Entities.TransportEntities;
using TransportEntity = Api.Domain.Entities.TransportEntities.TransportRequest;

namespace Api.Application.Interfaces;

public interface IEmailService
{
    Task SendCreatedTransportRequestEmail(TransportResponseDto transportResponseDto, List<string> destinataries); //Pending
    Task SendAssignedTransportRequestEmail(TransportEntity transportRequest, Driver driver, Vehicle vehicle); //Status in process
    Task SendCompletedOrRejectedEmail(string collaboratorEmail, Guid requestId, string comment, bool isApprove); // Cancelado y completado
}
