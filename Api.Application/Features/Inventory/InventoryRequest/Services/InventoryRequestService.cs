using System.Text;
using Api.Application.Common.BaseResponse;
using Api.Application.Common.Exceptions;
using Api.Application.Common.Extensions;
using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Application.Features.Inventory.InventoryRequest.Dtos;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Collaborators;
using Api.Application.Interfaces.Inventory;
using Api.Domain.Constants;
using Api.Domain.Entities;
using Api.Domain.Entities.InventoryEntities;
using Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using InventoryEntity = Api.Domain.Entities.InventoryEntities.InventoryRequest;

namespace Api.Application.Features.Inventory.InventoryRequest.Services;

public class InventoryRequestService : IInventoryRequestService
{
    private readonly ICollaboratorRepository _collaboratorRepository;
    private readonly IBaseRepository<Collaborator> _collaboratorRepository2;
    private readonly IBaseRepository<InventoryEntity> _inventoryRequestRepository;
    private readonly IBaseRepository<InventoryItem> _inventoryItemRepository;
    private readonly IBaseRepository<InventoryRequestItem> _inventoryRequestItemRepository;
    private readonly IInventoryRequestRepository _requestRepository;
    private readonly IEmailService _emailService;

    public InventoryRequestService(ICollaboratorRepository collaboratorRepository,
        IBaseRepository<InventoryEntity> inventoryRequestRepository,
        IBaseRepository<InventoryRequestItem> inventoryRequestItemRepository,
        IBaseRepository<InventoryItem> inventoryItemRepository,
        IEmailService emailService,
        IInventoryRequestRepository requestRepository,
        IBaseRepository<Collaborator> collaboratorRepository2)
    {
        _collaboratorRepository = collaboratorRepository;
        _inventoryRequestRepository = inventoryRequestRepository;
        _inventoryRequestItemRepository = inventoryRequestItemRepository;
        _inventoryItemRepository = inventoryItemRepository;
        _emailService = emailService;
        _requestRepository = requestRepository;
        _collaboratorRepository2 = collaboratorRepository2;
    }

    public async Task<InventoryResponseDto> AddInventoryRequest(InventoryRequestDto inventoryRequestDto, CancellationToken cancellationToken)
    {
        var collaborator = await _collaboratorRepository.GetById(inventoryRequestDto.CollaboratorId, cancellationToken);

        var inventoryRequestEntity = new InventoryEntity
        {
            CollaboratorId = collaborator.Id,
            CreatedDate = DateTime.Now,
            RequestStatus = RequestStatus.Pending,
        };
        var createdInventoryRequest = await _inventoryRequestRepository.AddAsync(inventoryRequestEntity, cancellationToken);

        var inventoryRequestItems = new List<InventoryRequestItem>();
        foreach (var itemDto in inventoryRequestDto.ArticlesIds)
        {
            var article = await _inventoryItemRepository.GetById(itemDto, cancellationToken);
            var inventoryRequestItem = new InventoryRequestItem
            {
                InventoryRequestId = createdInventoryRequest.Id,
                InventoryItemId = article.Id
            };
            inventoryRequestItems.Add(inventoryRequestItem);
        }
        await _inventoryRequestItemRepository.AddRange(inventoryRequestItems, cancellationToken);

        var inventoryRequestWithCollaborator = await _inventoryRequestRepository.Query()
            .Include(ir => ir.Collaborator)
            .Include(ir => ir.InventoryRequestItems)
            .ThenInclude(iri => iri.InventoryItem)
            .FirstOrDefaultAsync(ir => ir.Id == createdInventoryRequest.Id, cancellationToken);

        await SendInventoryRequestEmail(createdInventoryRequest);
        return inventoryRequestWithCollaborator;
    }

    private async Task SendInventoryRequestEmail(InventoryResponseDto inventoryResponseDto)
    {
        string htmlFile = FileExtensions.ReadEmailTemplate(EmailConstants.InventoryRequestTemplate, EmailConstants.TemplateEmailRoute);
        htmlFile = htmlFile.Replace("{{Name}}", inventoryResponseDto.Collaborator.Name);

        var articlesHtmlBuilder = new StringBuilder();
        foreach (var item in inventoryResponseDto.InventoryRequestItems)
        {
            articlesHtmlBuilder.Append("<tr>")
                                 .Append($"<td style='border: 1px solid #ddd; padding: 8px;'>{item.Name}</td>")
                                 .Append($"<td style='border: 1px solid #ddd; padding: 8px;'>{item.Quantity}</td>")
                                 .Append($"<td style='border: 1px solid #ddd; padding: 8px;'>{item.UnitOfMeasure}</td>")
                                 .Append("</tr>");
        }

        htmlFile = htmlFile.Replace("{{Articles}}", articlesHtmlBuilder.ToString());
        await _emailService.SendEmail("supervisorEmail@gmail.com", "Solicitud de Almacén y Suministro", htmlFile);
    }

    public async Task<Paged<InventorySummaryDto>> GetPagedInventoryRequest(PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        var result = await _requestRepository.SearchAsync(paginationQuery, cancellationToken);
        foreach (var item in result.Items)
        {
            item.RequestStatusDescription = item.RequestStatus.DisplayName();
        }
        return result;
    }

    public async Task<IEnumerable<InventorySummaryDto>> GetInventoryRequestDetails(Guid id, CancellationToken cancellationToken)
    {
        var result = await _requestRepository.GetSummary(id, cancellationToken);
        foreach (var item in result)
        {
            item.RequestStatusDescription = item.RequestStatus.DisplayName();
        }
        return result;
    }

    public async Task<string> ApproveInventoryRequest(ApprovalDto approvalDto, CancellationToken cancellationToken)
    {
        var request = await _inventoryRequestRepository.GetById(approvalDto.RequestId, cancellationToken);

        if (request.RequestStatus != RequestStatus.Pending)
            throw new BadRequestException($"Inventory request is already {request.RequestStatus}.");

        // Obtener el rol del colaborador actual
        var collaboratorRoles = await _collaboratorRepository2
            .Query()
            .Where(x => x.Id == request.CollaboratorId)
            .Select(x => x.Roles)
            .FirstOrDefaultAsync(cancellationToken);

        if (collaboratorRoles == null)
            throw new BadRequestException("Collaborator roles not found.");

        var userRoles = collaboratorRoles
            .Select(EnumExtensions.MapDbRoleToEnum)
            .Where(role => role != null)
            .Cast<UserRoles>()
            .ToList();

        // Validar si el rol del usuario coincide con el rol requerido para aprobar la solicitud
        var requiredRole = request.PendingApprovalBy?.ToString();
        if (!userRoles.Any(role => role.ToString() == requiredRole))
        {
            throw new UnauthorizedAccessException("You do not have the required role to perform this action.");
        }

        // Si se rechaza la solicitud
        if (!approvalDto.IsApproved)
        {
            var updates = new Dictionary<string, object>
        {
            { nameof(request.RequestStatus), RequestStatus.Rejected },
            { nameof(request.PendingApprovalBy), PendingApprovalBy.None },
            { nameof(request.Comment), approvalDto.Comment }
        };

            await _inventoryRequestRepository.PatchAsync(request.Id, updates, cancellationToken);

            // Notificar al colaborador (comentado por ahora)
            //await _notificationService.NotifyAsync(
            //    new NotificationDto
            //    {
            //        Recipient = request.Collaborator.Email,
            //        Subject = "Inventory Request Rejected",
            //        Message = $"Your inventory request has been rejected by {userRoles.FirstOrDefault()}."
            //    });

            return $"Inventory request {approvalDto.RequestId} has been rejected with comments: {approvalDto.Comment}";
        }

        // Si se aprueba la solicitud, cambiar el estado de PendingApprovalBy para avanzar al siguiente nivel de aprobación
        request.Comment = approvalDto.Comment;

        // Avanzar al siguiente nivel de aprobación
        switch (userRoles.FirstOrDefault())
        {
            case UserRoles.Supervisor:
                request.PendingApprovalBy = PendingApprovalBy.Administrative;
                break;

            case UserRoles.Administrative:
                request.PendingApprovalBy = PendingApprovalBy.AreaAdministrator;
                break;

            case UserRoles.AreaAdministrator:
                request.PendingApprovalBy = PendingApprovalBy.None; // Ya no hay más aprobaciones pendientes
                request.RequestStatus = RequestStatus.Approved; // Marca la solicitud como aprobada
                request.ApprovedDate = DateTime.Now;
                break;

            default:
                throw new UnauthorizedAccessException("Role not authorized for approval.");
        }

        // Guardar los cambios
        var updatesApproval = new Dictionary<string, object>
    {
        { nameof(request.RequestStatus), request.RequestStatus },
        { nameof(request.PendingApprovalBy), request.PendingApprovalBy },
        { nameof(request.ApprovedDate), request.ApprovedDate },
        { nameof(request.Comment), request.Comment },
    };

        await _inventoryRequestRepository.PatchAsync(request.Id, updatesApproval, cancellationToken);

        // Notificar al colaborador (comentado por ahora)
        //await _notificationService.NotifyAsync(
        //    new NotificationDto
        //    {
        //        Recipient = request.Collaborator.Email,
        //        Subject = "Inventory Request Approved",
        //        Message = $"Your inventory request has been approved by {userRoles.FirstOrDefault()}."
        //    });

        return "";
        //return $"Inventory request {approvalDto.RequestId} has been {approvalDto.IsApproved ? "approved" : "rejected"} with comments: {approvalDto.Comment}";
    }
}

