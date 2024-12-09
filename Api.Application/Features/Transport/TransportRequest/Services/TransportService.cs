using Api.Application.Common.Exceptions;
using Api.Application.Common.Extensions;
using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Application.Features.Transport.TransportRequest.Dtos;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Collaborators;
using Api.Application.Interfaces.Transport;
using Api.Domain.Constants;
using Api.Domain.Entities;
using Api.Domain.Entities.TransportEntities;
using Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using TransportEntity = Api.Domain.Entities.TransportEntities.TransportRequest;

namespace Api.Application.Features.Transport.TransportRequest.Services;

public class TransportService : ITransportService
{
    //Todo: Unificar collaborator repository con el base.
    private readonly ICollaboratorRepository _collaboratorRepository;
    private readonly IBaseRepository<Collaborator> _collaboratorRepository2;
    private readonly IEmailService _emailService;
    private readonly IBaseRepository<TransportEntity> _transportRepository;
    private readonly ITransportRequestRepository _transportRequestRepository;
    private readonly IBaseRepository<Driver> _driverRepository;
    private readonly IBaseRepository<Vehicle> _vehicleRepository;
    private readonly IGraphUserService _graphUserService;

    public TransportService(ICollaboratorRepository collaboratorRepository, IEmailService emailService, IBaseRepository<TransportEntity> transportRepository, ITransportRequestRepository transportRequestRepository, IBaseRepository<Driver> driverRepository, IBaseRepository<Vehicle> vehicleRepository, IBaseRepository<Collaborator> collaboratorRepository2, IGraphUserService graphUserService)
    {
        _collaboratorRepository = collaboratorRepository;
        _emailService = emailService;
        _transportRepository = transportRepository;
        _transportRequestRepository = transportRequestRepository;
        _driverRepository = driverRepository;
        _vehicleRepository = vehicleRepository;
        _collaboratorRepository2 = collaboratorRepository2;
        _graphUserService = graphUserService;
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

        // await SendTransportRequestEmail(createdInventoryRequest);

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

    private async Task SendTransportRequestEmail(TransportResponseDto transportResponseDto, string department)
    {
        string htmlTemplate = FileExtensions.ReadEmailTemplate(EmailConstants.TransportRequestTemplate, EmailConstants.TemplateEmailRoute);

        htmlTemplate = htmlTemplate.Replace("{{Name}}", transportResponseDto.Collaborator.Name)
                                   .Replace("{{DeparturePoint}}", transportResponseDto.DeparturePoint)
                                   .Replace("{{Destination}}", transportResponseDto.Destination)
                                   .Replace("{{DepartureDateTime}}", transportResponseDto.DepartureDateTime.ToString("dd/MM/yyyy HH:mm"))
                                   .Replace("{{NumberOfPeople}}", transportResponseDto.NumberOfPeople.ToString())
                                   .Replace("{{PhoneNumber}}", transportResponseDto.PhoneNumber ?? "N/A");
        //Todo: poner correo department
        await _emailService.SendEmail(department, "Nueva Solicitud de Transporte", htmlTemplate);
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

        await _emailService.SendEmail(transportRequest.Collaborator.Email, "Asignación de Conductor y Vehículo a Solicitud de Transporte", htmlTemplate);
    }

    //Todo use patch here
    public async Task AssignDriverAndVehicle(Guid transportRequestId, AssignDriverVehicleDto driverVehicleDto, CancellationToken cancellationToken)
    {
        var transportRequest = await _transportRepository.Query()
        .Include(tr => tr.Collaborator)
        .FirstOrDefaultAsync(tr => tr.Id == transportRequestId, cancellationToken)
        ?? throw new NotFoundException($"Transport request with ID {transportRequestId} not found.");

        bool isConflict = await _transportRepository.Query()
        .AnyAsync(tr => tr.DriverId == driverVehicleDto.DriverId &&
                      tr.VehicleId == driverVehicleDto.VehicleId &&
                      tr.DepartureDateTime == transportRequest.DepartureDateTime &&
                      tr.Id != transportRequestId,
                      cancellationToken);

        if (isConflict)
            throw new BadRequestException("El chofer y el vehículo ya están asignados a otra solicitud en la misma hora.");


        var driver = await _driverRepository.GetById(driverVehicleDto.DriverId, cancellationToken);
        var vehicle = await _vehicleRepository.GetById(driverVehicleDto.VehicleId, cancellationToken);

        transportRequest.VehicleId = driverVehicleDto.VehicleId;
        transportRequest.DriverId = driverVehicleDto.DriverId;

        var updatedTransportRequest = await _transportRepository.UpdateAsync(transportRequest, cancellationToken);

        //await SendAssignedTransportRequestEmail(updatedTransportRequest, driver, vehicle);
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
                await SendApproveOrRejectEmail(request.Collaborator.Name, request.Id, request.Comment, false);
            }
            else
            {
                request.RequestStatus = RequestStatus.Approved;
                request.Comment = "La solicitud fue completada automáticamente al pasar la fecha de salida.";
            }
            request.StatusChangedDate = currentTime;
        }

        await _transportRepository.UpdateRange(overdueRequests, cancellationToken);
        return "Los estados de las solicitudes han sido actualizados correctamente.";
    }

    private async Task SendApproveOrRejectEmail(string collaboratorEmail, Guid requestId, string comment, bool isApprove)
    {
        if (!isApprove)
        {
            string htmlTemplate = FileExtensions.ReadEmailTemplate(EmailConstants.RejectedRequestTemplate, EmailConstants.TemplateEmailRoute);

            htmlTemplate = htmlTemplate.Replace("{{Email}}", collaboratorEmail)
                                       .Replace("{{RequestId}}", requestId.ToString())
                                       .Replace("{{Comment}}", comment);

            await _emailService.SendEmail(collaboratorEmail, "Notificación de Solicitud Rechazada", htmlTemplate);
        }
        else
        {
            string htmlTemplate = FileExtensions.ReadEmailTemplate(EmailConstants.ApprovedRequestTemplate, EmailConstants.TemplateEmailRoute);

            htmlTemplate = htmlTemplate.Replace("{{Email}}", collaboratorEmail)
                                       .Replace("{{RequestId}}", requestId.ToString())
                                       .Replace("{{Comment}}", comment);

            await _emailService.SendEmail(collaboratorEmail, "Notificación de Solicitud Aprobada", htmlTemplate);
        }

    }
    public async Task<string> ApproveTransportRequest(ApprovalDto approvalDto, CancellationToken cancellationToken)
    {
        var request = await _transportRepository.
             Query()
            .Where(x => x.Id == approvalDto.RequestId)
            .Include(x => x.Collaborator)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (request.RequestStatus != RequestStatus.Pending)
            throw new BadRequestException($"Transport request is already {request.RequestStatus}.");

        if (request.DriverId == null || request.VehicleId == null)
            throw new BadRequestException("Transport request must have both a driver and a vehicle assigned before it can be approved or rejected.");

        var loggedUser = await _graphUserService.Current();

        if (loggedUser.Roles == null || !loggedUser.Roles.Any())
            throw new BadRequestException("Collaborator roles not found.");

        var userRoles = loggedUser.Roles
            .Select(EnumExtensions.MapDbRoleToEnum)
            .Where(role => role != null)
            .Cast<UserRoles>()
            .ToList();

        if (!userRoles.Contains(UserRoles.Supervisor))
            throw new UnauthorizedAccessException("You do not have the required role (Supervisor) to perform this action.");

        if (!approvalDto.IsApproved)
        {
            var updates = new Dictionary<string, object>
            {
                { nameof(request.RequestStatus), RequestStatus.Rejected },
                { nameof(request.PendingApprovalBy), PendingApprovalBy.None },
                { nameof(request.Comment), approvalDto.Comment },
                { nameof(request.StatusChangedDate), DateTime.Now },
                { nameof(request.ApprovedOrRejectedBy), loggedUser.Name },
            };

            await _transportRepository.PatchAsync(request.Id, updates, cancellationToken);
            await SendApproveOrRejectEmail(request.Collaborator.Email, request.Id, approvalDto.Comment, false);
            return $"Transport request {approvalDto.RequestId} has been rejected with comments: {approvalDto.Comment}";
        }

        request.StatusChangedDate = DateTime.Now;
        request.ApprovedOrRejectedBy = loggedUser.Name;
        request.RequestStatus = RequestStatus.Approved;
        request.PendingApprovalBy = PendingApprovalBy.None;

        var approvalUpdates = new Dictionary<string, object>
        {
            { nameof(request.RequestStatus), request.RequestStatus },
            { nameof(request.PendingApprovalBy), request.PendingApprovalBy },
            { nameof(request.StatusChangedDate), request.StatusChangedDate },
            { nameof(request.ApprovedOrRejectedBy), request.ApprovedOrRejectedBy },
            { nameof(request.Comment), approvalDto.Comment }
        };

        await _transportRepository.PatchAsync(request.Id, approvalUpdates, cancellationToken);
        await SendApproveOrRejectEmail(request.Collaborator.Email, request.Id, approvalDto.Comment, true);

        return $"Transport request {approvalDto.RequestId} has been approved with comments: {approvalDto.Comment}";
    }
}
