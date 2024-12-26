using Api.Application.Common.Extensions;
using Api.Application.Features.Transport.TransportRequest.Dtos;
using Api.Application.Interfaces;
using Api.Domain.Constants;
using Api.Domain.Entities.TransportEntities;
using TransportEntity = Api.Domain.Entities.TransportEntities.TransportRequest;

namespace Api.Application.Common;

public class EmailService : IEmailService
{
    private readonly IEmailProvider _emailProvider;
    public EmailService(IEmailProvider emailProvider)
    {
        _emailProvider = emailProvider;
    }

    public async Task SendCreatedTransportRequestEmail(TransportResponseDto transportResponseDto, List<string> destinataries)
    {
        string htmlTemplate = FileExtensions.ReadEmailTemplate(EmailConstants.TransportRequestTemplate, EmailConstants.TemplateEmailRoute);

        htmlTemplate = htmlTemplate.Replace("{{Name}}", transportResponseDto.Collaborator.Name)
                                   .Replace("{{DeparturePoint}}", transportResponseDto.DeparturePoint)
                                   .Replace("{{Destination}}", transportResponseDto.Destination)
                                   .Replace("{{DepartureDateTime}}", transportResponseDto.DepartureDateTime.ToString("dd/MM/yyyy HH:mm"))
                                   .Replace("{{NumberOfPeople}}", transportResponseDto.NumberOfPeople.ToString())
                                   .Replace("{{PhoneNumber}}", transportResponseDto.PhoneNumber ?? "N/A");
        //Todo: poner correo department y de chofer
        await _emailProvider.SendBulkEmail(destinataries, "Nueva Solicitud de Transporte", htmlTemplate);
    }

    public async Task SendAssignedTransportRequestEmail(TransportEntity transportRequest, Driver driver, Vehicle vehicle)
    {
        string htmlTemplate = FileExtensions.ReadEmailTemplate(EmailConstants.AssignedTransportRequestTemplate, EmailConstants.TemplateEmailRoute);

        htmlTemplate = htmlTemplate.Replace("{{Name}}", transportRequest.Collaborator.Name)
                                   .Replace("{{DeparturePoint}}", transportRequest.DeparturePoint)
                                   .Replace("{{Destination}}", transportRequest.Destination)
                                   .Replace("{{DepartureDateTime}}", transportRequest.DepartureDateTime.ToString("dd/MM/yyyy HH:mm"))
                                   .Replace("{{DriverName}}", driver.Name)
                                   .Replace("{{VehicleModel}}", vehicle.Model)
                                   .Replace("{{VehicleLicensePlate}}", vehicle.LicensePlate);

        await _emailProvider.SendEmailToSingleRecipient(transportRequest.Collaborator.Email, "Asignación de Conductor y Vehículo a Solicitud de Transporte", htmlTemplate);
    }

    public async Task SendCompletedOrRejectedEmail(string collaboratorEmail, Guid requestId, string comment, bool isApprove)
    {
        if (!isApprove)
        {
            string htmlTemplate = FileExtensions.ReadEmailTemplate(EmailConstants.RejectedRequestTemplate, EmailConstants.TemplateEmailRoute);

            htmlTemplate = htmlTemplate.Replace("{{Email}}", collaboratorEmail)
                                       .Replace("{{RequestId}}", requestId.ToString())
                                       .Replace("{{Comment}}", comment);

            await _emailProvider.SendEmailToSingleRecipient(collaboratorEmail, "Notificación de Solicitud Rechazada", htmlTemplate);
        }
        else
        {
            string htmlTemplate = FileExtensions.ReadEmailTemplate(EmailConstants.ApprovedRequestTemplate, EmailConstants.TemplateEmailRoute);

            htmlTemplate = htmlTemplate.Replace("{{Email}}", collaboratorEmail)
                                       .Replace("{{RequestId}}", requestId.ToString())
                                       .Replace("{{Comment}}", comment);

            await _emailProvider.SendEmailToSingleRecipient(collaboratorEmail, "Notificación de Solicitud Completada", htmlTemplate);
        }

    }
}
