using Api.Application.Common.Exceptions;
using Api.Application.Common.Extensions;
using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.TransportRequest.Dtos;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Collaborators;
using Api.Application.Interfaces.Transport;
using Api.Domain.Constants;
using Api.Domain.Entities.TransportEntities;
using Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using TransportEntity = Api.Domain.Entities.TransportEntities.TransportRequest;

namespace Api.Application.Features.Transport.TransportRequest.Services;

public class TransportService : ITransportService
{
    private readonly ICollaboratorRepository _collaboratorRepository;
    private readonly IEmailService _emailService;
    private readonly IBaseRepository<TransportEntity> _transportRepository;
    private readonly ITransportRequestRepository _transportRequestRepository;
    private readonly IBaseRepository<Driver> _driverRepository;
    private readonly IBaseRepository<Vehicle> _vehicleRepository;

    public TransportService(ICollaboratorRepository collaboratorRepository, IEmailService emailService, IBaseRepository<TransportEntity> transportRepository, ITransportRequestRepository transportRequestRepository, IBaseRepository<Driver> driverRepository, IBaseRepository<Vehicle> vehicleRepository)
    {
        _collaboratorRepository = collaboratorRepository;
        _emailService = emailService;
        _transportRepository = transportRepository;
        _transportRequestRepository = transportRequestRepository;
        _driverRepository = driverRepository;
        _vehicleRepository = vehicleRepository;
    }

    public async Task<Paged<TransportSummaryDto>> GetPagedTransportRequests(PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        var result = await _transportRequestRepository.SearchAsync(paginationQuery, cancellationToken);
        foreach (var item in result.Items)
        {
            item.RequestStatusDescription = item.RequestStatus.DisplayName();
        }
        return result;
    }

    public async Task<IEnumerable<TransportSummaryDto>> GetTransportRequestDetails(Guid id, CancellationToken cancellationToken)
    {
        var result = await _transportRequestRepository.GetSummary(id, cancellationToken);
        foreach (var item in result)
        {
            item.RequestStatusDescription = item.RequestStatus.DisplayName();
        }
        return result;
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
            CreatedDate = DateTime.Now,
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

        await _emailService.SendEmail("departamento@gmail.com", "Nueva Solicitud de Transporte", htmlTemplate);
    }


    private async Task SendAssignedTransportRequestEmail(TransportEntity transportRequest, Driver driver, Vehicle vehicle)
    {
        string htmlTemplate = FileExtensions.ReadEmailTemplate(EmailConstants.AssignedTransportRequestTemplate, EmailConstants.TemplateEmailRoute);

        htmlTemplate = htmlTemplate.Replace("{{Name}}", transportRequest.Collaborator.Name)
                                   .Replace("{{DeparturePoint}}", transportRequest.DeparturePoint)
                                   .Replace("{{Destination}}", transportRequest.Destination)
                                   .Replace("{{DepartureDateTime}}", transportRequest.DepartureDateTime.ToString("dd/MM/yyyy HH:mm"))
                                   .Replace("{{DriverName}}", driver.Name)
                                   .Replace("{{VehicleModel}}", vehicle.Model)
                                   .Replace("{{VehicleLicensePlate}}", vehicle.LicensePlate);

        await _emailService.SendEmail("colaboratorychofer@gmail.com", "Asignación de Conductor y Vehículo a Solicitud de Transporte", htmlTemplate);
    }


    public async Task AssignDriverAndVehicle(Guid transportRequestId, AssignDriverVehicleDto driverVehicleDto, CancellationToken cancellationToken)
    {
        var transportRequest = await _transportRepository.Query()
        .Include(tr => tr.Collaborator)
        .FirstOrDefaultAsync(tr => tr.Id == transportRequestId, cancellationToken)
        ?? throw new NotFoundException($"Transport request with ID {transportRequestId} not found.");

        //Todo: Refactor cuando se arreglen relaciones jj
        //bool isConflict = await _transportRepository.Query()
        //.AnyAsync(tr => tr.DriverId == driverVehicleDto.DriverId &&
        //              tr.VehicleId == driverVehicleDto.VehicleId &&
        //              tr.DepartureDateTime == transportRequest.DepartureDateTime &&
        //              tr.Id != transportRequestId, // Excluir la solicitud actual de la verificación
        //              cancellationToken);

        //if (isConflict)
        //{
        //    throw new BadRequestException("El chofer y el vehículo ya están asignados a otra solicitud en la misma hora.");
        //}

        var driver = await _driverRepository.GetById(driverVehicleDto.DriverId, cancellationToken);
        var vehicle = await _vehicleRepository.GetById(driverVehicleDto.VehicleId, cancellationToken);

        transportRequest.VehicleId = driverVehicleDto.VehicleId;
        transportRequest.DriverId = driverVehicleDto.DriverId;

        var updatedTransportRequest = await _transportRepository.UpdateAsync(transportRequest, cancellationToken);

        await SendAssignedTransportRequestEmail(updatedTransportRequest, driver, vehicle);
    }

    public async Task<string> UpdateExpiredTransportRequestsStatus(CancellationToken cancellationToken = default)
    {
        var currentTime = DateTime.Now; // validar tiempo ejemplo si tiene mas de 1 dia sin asignar vehiculo ni chofer y esta pendiente.

        var overdueRequests = await _transportRepository.Query()
            .Where(tr => tr.RequestStatus == RequestStatus.Pending && tr.DepartureDateTime <= currentTime)
            .Include(x => x.Collaborator)
            .ToListAsync(cancellationToken);

        if (overdueRequests.Count == 0) return "No hay solicitudes con fecha de salida vencidas";
        
        foreach (var request in overdueRequests)
        {
            if (request.VehicleId == null || request.DriverId == null)
            {
                request.RequestStatus = RequestStatus.Rejected;
                request.Comment = "La solicitud fue rechazada porque pasó la fecha de salida sin asignar vehículo y chofer.";
                await SendRejectionEmail(request.Collaborator.Name, request.Id, request.Comment);
            }
            else
            {
                request.RequestStatus = RequestStatus.Approved;
                request.Comment = "La solicitud fue completada automáticamente al pasar la fecha de salida.";
            }
            request.ApprovedDate = currentTime;
        }

        await _transportRepository.UpdateRange(overdueRequests, cancellationToken);
        return "Los estados de las solicitudes han sido actualizados correctamente.";
    }

    private async Task SendRejectionEmail(string collaboratorName, Guid requestId, string comment)
    {
        string htmlTemplate = FileExtensions.ReadEmailTemplate(EmailConstants.RejectedRequestTemplate, EmailConstants.TemplateEmailRoute);

        htmlTemplate = htmlTemplate.Replace("{{Name}}", collaboratorName)
                                   .Replace("{{RequestId}}", requestId.ToString())
                                   .Replace("{{Comment}}", comment);

        await _emailService.SendEmail("colaborador@example.com", "Notificación de Solicitud Rechazada", htmlTemplate);
    }

    //Todo este endpoint se usara con roles jj sera un patch
    public async Task RejectRequest(Guid requestId, string comment, CancellationToken cancellationToken)
    {
        //Todo: agregar colaborator
        var request = await _transportRepository.GetById(requestId, cancellationToken);

        if (request == null)
        {
            throw new NotFoundException($"Request with ID {requestId} not found.");
        }

        // Solo permite rechazos para solicitudes pendientes
        if (request.RequestStatus != RequestStatus.Pending)
        {
            throw new BadRequestException("Only pending requests can be rejected.");
        }

        // Actualiza el estado de la solicitud
        request.RequestStatus = RequestStatus.Rejected;
        request.Comment = comment;
        request.ApprovedDate = DateTime.UtcNow; // Marcar la fecha del rechazo
        await _transportRepository.UpdateAsync(request, cancellationToken);

        // Enviar el correo de notificación
        await SendRejectionEmail(request.Collaborator.Name, requestId, comment);
    }
}
