using Api.Application.Common.Exceptions;
using Api.Application.Common.Extensions;
using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Application.Features.Transport.TransportRequest.Dtos;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Collaborators;
using Api.Application.Interfaces.Transport;
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
    private readonly IBaseRepository<TransportEntity> _transportRepository;
    private readonly ITransportRequestRepository _transportRequestRepository;
    private readonly IBaseRepository<Driver> _driverRepository;
    private readonly IBaseRepository<Vehicle> _vehicleRepository;
    private readonly IGraphUserService _graphUserService;
    private readonly IEmailService _emailService;

    public TransportService(ICollaboratorRepository collaboratorRepository, IBaseRepository<TransportEntity> transportRepository, ITransportRequestRepository transportRequestRepository,
        IBaseRepository<Driver> driverRepository, IBaseRepository<Vehicle> vehicleRepository, IBaseRepository<Collaborator> collaboratorRepository2, IGraphUserService graphUserService, IEmailService emailService)
    {
        _collaboratorRepository = collaboratorRepository;
        _transportRepository = transportRepository;
        _transportRequestRepository = transportRequestRepository;
        _driverRepository = driverRepository;
        _vehicleRepository = vehicleRepository;
        _collaboratorRepository2 = collaboratorRepository2;
        _graphUserService = graphUserService;
        _emailService = emailService;
    }

    public async Task<Paged<TransportSummaryDto>> GetPagedTransportRequests(PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        var result = await _transportRequestRepository.SearchAsync(paginationQuery, cancellationToken);

        if (!string.IsNullOrWhiteSpace(paginationQuery.Search))
        {
            var statusMap = EnumExtensions.GetTransportRequestStatusMap();
            result.Items = result.Items
                .Where(item =>
                    statusMap[item.RequestStatus.DisplayName()]
                        .DisplayName()
                        .Contains(paginationQuery.Search, StringComparison.OrdinalIgnoreCase) ||
                    item.DeparturePoint.Contains(paginationQuery.Search, StringComparison.OrdinalIgnoreCase) ||
                    item.Destination.Contains(paginationQuery.Search, StringComparison.OrdinalIgnoreCase) ||
                    item.PhoneNumber.Contains(paginationQuery.Search, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

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
        var collaborator = await _collaboratorRepository.GetById(transportRequestDto.CollaboratorId, cancellationToken);
        var transportRequestEntity = MapTransportEntity(transportRequestDto, collaborator.Department);
        var createdInventoryRequest = await _transportRepository.AddAsync(transportRequestEntity, cancellationToken);

        var inventoryRequestWithCollaborator = await _transportRepository.Query()
        .Include(ir => ir.Collaborator)
        .FirstOrDefaultAsync(ir => ir.Id == createdInventoryRequest.Id, cancellationToken);

        //Todo: poner los correos que van aqui jjj
        var destinataries = new List<string>()
        {
            "waldis.henriquez@cultura.gob.do",
            "manuel.medina@cultura.gob.do",
            collaborator.Email,
        };
        await _emailService.SendCreatedTransportRequestEmail(createdInventoryRequest, destinataries);

        return inventoryRequestWithCollaborator;
    }

    private TransportEntity MapTransportEntity(TransportRequestDto transportRequestDto, string collaboratorDepartment)
    {
        return new TransportEntity
        {
            CollaboratorId = transportRequestDto.CollaboratorId,
            CreatedDate = DateTime.Now,
            TransportRequestStatus = TransportRequestStatus.Pending,
            DeparturePoint = transportRequestDto.DeparturePoint,
            Destination = transportRequestDto.Destination,
            NumberOfPeople = transportRequestDto.NumberOfPeople,
            DepartureDateTime = transportRequestDto.DepartureDateTime,
            PhoneNumber = transportRequestDto.PhoneNumber,
            PendingApprovalBy = PendingApprovalBy.Supervisor,
            LocationType = transportRequestDto.LocationType,
            TravelType = transportRequestDto.TravelType,
            RequestCode = RequestCodeHelper.GenerateRequestCode("Transporte", GetLastCodeNumber)
        };

    }
    private int? GetLastCodeNumber(string prefix)
    {
        return _transportRepository.Query()
            .Where(r => r.RequestCode.StartsWith(prefix))
            .OrderByDescending(r => r.RequestCode)
            .Select(r => int.Parse(r.RequestCode.Substring(1)))
            .FirstOrDefault();
    }

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
        transportRequest.TransportRequestStatus = TransportRequestStatus.InProcess;

        var updatedTransportRequest = await _transportRepository.UpdateAsync(transportRequest, cancellationToken);

        await _emailService.SendAssignedTransportRequestEmail(updatedTransportRequest, driver, vehicle);
    }

    public async Task<string> UpdateExpiredTransportRequestsStatus(CancellationToken cancellationToken = default)
    {
        var currentTime = DateTime.Now; // validar tiempo ejemplo si tiene mas de 1 dia sin asignar vehiculo ni chofer y esta pendiente.

        var overdueRequests = await _transportRepository.Query()
            .Where(tr => tr.TransportRequestStatus == TransportRequestStatus.Pending && tr.DepartureDateTime <= currentTime)
            .Include(x => x.Collaborator)
            .ToListAsync(cancellationToken);

        if (overdueRequests.Count == 0) return "No hay solicitudes con fecha de salida vencidas";

        foreach (var request in overdueRequests)
        {
            if (request.VehicleId == null || request.DriverId == null)
            {
                request.TransportRequestStatus = TransportRequestStatus.Rejected;
                request.Comment = "La solicitud fue rechazada porque pasó la fecha de salida sin asignar vehículo y chofer.";
                await _emailService.SendCompletedOrRejectedEmail(request.Collaborator.Name, request.Id, request.Comment, false);
            }
            else
            {
                request.TransportRequestStatus = TransportRequestStatus.Completed;
                request.Comment = "La solicitud fue completada automáticamente al pasar la fecha de salida.";
            }
            request.StatusChangedDate = currentTime;
        }

        await _transportRepository.UpdateRange(overdueRequests, cancellationToken);
        return "Los estados de las solicitudes han sido actualizados correctamente.";
    }
    public async Task<string> ApproveTransportRequest(ApprovalDto approvalDto, CancellationToken cancellationToken)
    {
        var request = await _transportRepository.
             Query()
            .Where(x => x.Id == approvalDto.RequestId)
            .Include(x => x.Collaborator)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (request.TransportRequestStatus != TransportRequestStatus.InProcess)
            throw new BadRequestException($"Transport request is already {request.TransportRequestStatus}.");

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
                { nameof(request.TransportRequestStatus), TransportRequestStatus.Rejected },
                { nameof(request.PendingApprovalBy), PendingApprovalBy.None },
                { nameof(request.Comment), approvalDto.Comment },
                { nameof(request.StatusChangedDate), DateTime.Now },
                { nameof(request.ApprovedOrRejectedBy), loggedUser.Name },
            };

            await _transportRepository.PatchAsync(request.Id, updates, cancellationToken);
            await _emailService.SendCompletedOrRejectedEmail(request.Collaborator.Email, request.Id, approvalDto.Comment, false);
            return $"Transport request {approvalDto.RequestId} has been rejected with comments: {approvalDto.Comment}";
        }

        if (request.DriverId == null || request.VehicleId == null)
            throw new BadRequestException("Transport request must have both a driver and a vehicle assigned before it can be approved");

        request.StatusChangedDate = DateTime.Now;
        request.ApprovedOrRejectedBy = loggedUser.Name;
        request.TransportRequestStatus = TransportRequestStatus.Completed;
        request.PendingApprovalBy = PendingApprovalBy.None;

        var approvalUpdates = new Dictionary<string, object>
        {
            { nameof(request.TransportRequestStatus), request.TransportRequestStatus },
            { nameof(request.PendingApprovalBy), request.PendingApprovalBy },
            { nameof(request.StatusChangedDate), request.StatusChangedDate },
            { nameof(request.ApprovedOrRejectedBy), request.ApprovedOrRejectedBy },
            { nameof(request.Comment), approvalDto.Comment }
        };

        await _transportRepository.PatchAsync(request.Id, approvalUpdates, cancellationToken);
        await _emailService.SendCompletedOrRejectedEmail(request.Collaborator.Email, request.Id, approvalDto.Comment, true);

        return $"Transport request {approvalDto.RequestId} has been approved with comments: {approvalDto.Comment}";
    }
}
