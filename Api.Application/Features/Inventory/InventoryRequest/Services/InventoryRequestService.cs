using System.Text;
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
    private readonly IGraphUserService _graphUserService;

    public InventoryRequestService(ICollaboratorRepository collaboratorRepository,
        IBaseRepository<InventoryEntity> inventoryRequestRepository,
        IBaseRepository<InventoryRequestItem> inventoryRequestItemRepository,
        IBaseRepository<InventoryItem> inventoryItemRepository,
        IEmailService emailService,
        IInventoryRequestRepository requestRepository,
        IBaseRepository<Collaborator> collaboratorRepository2,
        IGraphUserService graphUserService)
    {
        _collaboratorRepository = collaboratorRepository;
        _inventoryRequestRepository = inventoryRequestRepository;
        _inventoryRequestItemRepository = inventoryRequestItemRepository;
        _inventoryItemRepository = inventoryItemRepository;
        _emailService = emailService;
        _requestRepository = requestRepository;
        _collaboratorRepository2 = collaboratorRepository2;
        _graphUserService = graphUserService;
    }

    public async Task<InventoryResponseDto> AddInventoryRequest(InventoryRequestDto inventoryRequestDto, CancellationToken cancellationToken)
    {
        var collaborator = await _collaboratorRepository.GetById(inventoryRequestDto.CollaboratorId, cancellationToken);

        var inventoryRequestEntity = new InventoryEntity
        {
            CollaboratorId = collaborator.Id,
            CreatedDate = DateTime.Now,
            RequestStatus = RequestStatus.Pending,
            PendingApprovalBy = PendingApprovalBy.Supervisor,
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
        await _emailService.SendEmail(inventoryResponseDto.Collaborator.Supervisor, "Solicitud de Almacén y Suministro", htmlFile);
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

    public async Task<string> ApproveInventoryRequest(ApprovalDto approvalDto, CancellationToken cancellationToken)
    {
        var request = await _inventoryRequestRepository
            .Query()
            .Where(x => x.Id == approvalDto.RequestId)
            .Include(x => x.Collaborator)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) 
            ?? throw new NotFoundException($"Inventory request with ID {approvalDto.RequestId} not found.");
        
        var loggedUser = await _graphUserService.Current();

        if (loggedUser.Roles == null)
            throw new BadRequestException("Collaborator roles not found.");

        var userRoles = loggedUser.Roles
            .Select(EnumExtensions.MapDbRoleToEnum)
            .Where(role => role != null)
            .Cast<UserRoles>()
            .ToList();

        if (!userRoles.Any())
            throw new UnauthorizedAccessException("User does not have valid roles for this action.");

        if (!userRoles.Contains((UserRoles)request.PendingApprovalBy))
            throw new UnauthorizedAccessException($"Approval not allowed. Current pending approval is by {request.PendingApprovalBy}.");

        request.ApprovedOrRejectedBy.Add(loggedUser.Name);

        if (!approvalDto.IsApproved)
        {
            request.RequestStatus = RequestStatus.Rejected;
            request.PendingApprovalBy = PendingApprovalBy.None;
            request.Comment = approvalDto.Comment;
            request.StatusChangedDate = DateTime.Now;

            await _requestRepository.UpdateRequestAsync(request.Id, request, cancellationToken);

            await SendApproveOrRejectEmail(request.Collaborator.Email, request.Id, approvalDto.Comment, false);
            return $"Inventory request {approvalDto.RequestId} has been rejected with comments: {approvalDto.Comment}";
        }

        request.Comment = approvalDto.Comment;

        if (userRoles.Contains(UserRoles.Supervisor) && request.PendingApprovalBy == PendingApprovalBy.Supervisor)
        {
            request.PendingApprovalBy = PendingApprovalBy.Administrative;
            request.RequestStatus = RequestStatus.Approved;
        }
        else if (userRoles.Contains(UserRoles.Administrative) && request.PendingApprovalBy == PendingApprovalBy.Administrative)
        {
            request.PendingApprovalBy = PendingApprovalBy.AreaAdministrator;
            request.RequestStatus = RequestStatus.ApprovedByAdmin;
        }
        else if (userRoles.Contains(UserRoles.AreaAdministrator) && request.PendingApprovalBy == PendingApprovalBy.AreaAdministrator)
        {
            request.PendingApprovalBy = PendingApprovalBy.None;
            request.RequestStatus = RequestStatus.Dispatched;
        }
        else
        {
            throw new UnauthorizedAccessException("User roles are not authorized for approval at this stage.");
        }

        request.StatusChangedDate = DateTime.Now;

        await _requestRepository.UpdateRequestAsync(request.Id, request, cancellationToken);

        await SendApproveOrRejectEmail(request.Collaborator.Email, request.Id, approvalDto.Comment, true);
        return $"Inventory request {approvalDto.RequestId} has been approved with comments: {approvalDto.Comment}";
    }
}



