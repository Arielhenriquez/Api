using Api.Application.Common.Extensions;
using Api.Application.Features.Transport.TransportRequest.Dtos;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Collaborators;
using Api.Application.Interfaces.Transport;
using Api.Domain.Constants;
using Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using TransportEntity = Api.Domain.Entities.TransportEntities.TransportRequest;

namespace Api.Application.Features.Transport.TransportRequest.Services;

//Todo: Get paged transport requests and get transport request detail
public class TransportService : ITransportService
{
    private readonly ICollaboratorRepository _collaboratorRepository;
    private readonly IEmailService _emailService;
    private readonly IBaseRepository<TransportEntity> _transportRepository;

    public TransportService(ICollaboratorRepository collaboratorRepository, IEmailService emailService, IBaseRepository<TransportEntity> transportRepository)
    {
        _collaboratorRepository = collaboratorRepository;
        _emailService = emailService;
        _transportRepository = transportRepository;
    }

    public async Task<TransportResponseDto> AddTransportRequest(TransportRequestDto transportRequestDto, CancellationToken cancellationToken)
    {
        await _collaboratorRepository.GetById(transportRequestDto.CollaboratorId, cancellationToken);
        var transportRequestEntity = MapTransportEntity(transportRequestDto);
        var createdInventoryRequest = await _transportRepository.AddAsync(transportRequestEntity, cancellationToken);

        var inventoryRequestWithCollaborator = await _transportRepository.Query()
        .Include(ir => ir.Collaborator)
        .FirstOrDefaultAsync(ir => ir.Id == createdInventoryRequest.Id, cancellationToken);

        await SendTransportRequestEmail(createdInventoryRequest);

        return inventoryRequestWithCollaborator;
    }

    private TransportEntity MapTransportEntity(TransportRequestDto transportRequestDto)
    {
        return new TransportEntity
        {
            CollaboratorId = transportRequestDto.CollaboratorId,
            RequestDate = DateTime.Now,
            RequestStatus = RequestStatus.Pending,
            DeparturePoint = transportRequestDto.DeparturePoint,
            Destination = transportRequestDto.Destination,
            NumberOfPeople = transportRequestDto.NumberOfPeople,
            DepartureDateTime = transportRequestDto.DepartureDateTime,
            PhoneNumber = transportRequestDto.PhoneNumber,
        };
    }

    private async Task SendTransportRequestEmail(TransportResponseDto transportResponseDto)
    {
        string htmlTemplate = FileExtensions.ReadEmailTemplate(EmailConstants.TransportRequestTemplate, EmailConstants.TemplateEmailRoute);

        htmlTemplate = htmlTemplate.Replace("{{Name}}", transportResponseDto.Collaborator.Name)
                                   .Replace("{{DeparturePoint}}", transportResponseDto.DeparturePoint)
                                   .Replace("{{Destination}}", transportResponseDto.Destination)
                                   .Replace("{{DepartureDateTime}}", transportResponseDto.DepartureDateTime.ToString("dd/MM/yyyy HH:mm"))
                                   .Replace("{{NumberOfPeople}}", transportResponseDto.NumberOfPeople.ToString())
                                   .Replace("{{PhoneNumber}}", transportResponseDto.PhoneNumber ?? "N/A");

        await _emailService.SendEmail("supervisorEmail@gmail.com", "Nueva Solicitud de Transporte", htmlTemplate);
    }
}
