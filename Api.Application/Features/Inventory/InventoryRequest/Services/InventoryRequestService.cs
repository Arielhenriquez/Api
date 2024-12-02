using Api.Application.Common.BaseResponse;
using Api.Application.Common.Extensions;
using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Application.Features.Inventory.InventoryRequest.Dtos;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Collaborators;
using Api.Application.Interfaces.Inventory;
using Api.Domain.Constants;
using Api.Domain.Entities.InventoryEntities;
using Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text;
using InventoryEntity = Api.Domain.Entities.InventoryEntities.InventoryRequest;

namespace Api.Application.Features.Inventory.InventoryRequest.Services;

public class InventoryRequestService : IInventoryRequestService
{
    private readonly ICollaboratorRepository _collaboratorRepository;
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
        IInventoryRequestRepository requestRepository)
    {
        _collaboratorRepository = collaboratorRepository;
        _inventoryRequestRepository = inventoryRequestRepository;
        _inventoryRequestItemRepository = inventoryRequestItemRepository;
        _inventoryItemRepository = inventoryItemRepository;
        _emailService = emailService;
        _requestRepository = requestRepository;
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
        //var request = await _inventoryRepository.GetById(id, cancellationToken);
        //if (request == null)
        //    return NotFound(BaseResponse.NotFound($"Inventory request with ID {id} not found."));

        //// Verificar si ya está completada o rechazada
        //if (request.RequestStatus == RequestStatus.Completed || request.RequestStatus == RequestStatus.Rejected)
        //    return BadRequest(BaseResponse.BadRequest($"Inventory request is already {request.RequestStatus}."));

        //// Obtener rol del usuario actual
        //var userRole = GetUserRoleFromToken();

        //// Validar flujo de aprobación
        //var requiredRole = request.PendingApprovalBy?.ToString();
        //if (requiredRole != userRole)
        //    return Forbid(BaseResponse.Unauthorized($"Approval by {requiredRole} is required before your approval."));

        //if (!approvalDto.IsApproved)
        //{
        //    // Manejar el rechazo
        //    request.RequestStatus = RequestStatus.Rejected;
        //    request.PendingApprovalBy = null;
        //    request.Comment = approvalDto.Comment;

        //    await _inventoryRepository.UpdateAsync(request, cancellationToken);

        //    // Notificar al colaborador
        //    await _notificationService.NotifyAsync(
        //        new NotificationDto
        //        {
        //            Recipient = request.Collaborator.Email,
        //            Subject = "Inventory Request Rejected",
        //            Message = $"Your inventory request has been rejected by {userRole}."
        //        });

        //    return Ok(BaseResponse.Ok($"Inventory request {id} has been rejected."));
        //}

        //// Manejar la aprobación
        //request.ApprovedBy.Add(userRole);
        //request.Comment = approvalDto.Comment;

        //// Avanzar al siguiente nivel de aprobación
        //switch (userRole)
        //{
        //    case "Supervisor":
        //        request.PendingApprovalBy = PendingApprovalBy.Admin;
        //        break;
        //    case "Admin":
        //        request.PendingApprovalBy = PendingApprovalBy.AreaAdministrator;
        //        break;
        //    case "AreaAdministrator":
        //        request.PendingApprovalBy = null; // Completa el flujo
        //        request.RequestStatus = RequestStatus.Approved; // Marca como completada
        //        break;
        //}

        //await _inventoryRepository.UpdateAsync(request, cancellationToken);

        //// Notificar al colaborador
        //await _notificationService.NotifyAsync(
        //    new NotificationDto
        //    {
        //        Recipient = request.Collaborator.Email,
        //        Subject = "Inventory Request Approved",
        //        Message = $"Your inventory request has been approved by {userRole}."
        //    });

        //return Ok(BaseResponse.Ok($"Inventory request {id} has been approved."));
        return "";
    }
}
