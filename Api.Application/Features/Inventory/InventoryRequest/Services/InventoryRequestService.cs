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

    //todo validar porque el approvedby no se esta mapeando.. (no se ta guardando en la BD)
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

    //todo test this more jj, approved or reject by no se estan mapeando
    public async Task<string> ApproveInventoryRequest(ApprovalDto approvalDto, CancellationToken cancellationToken)
    {
        var request = await _inventoryRequestRepository.GetById(approvalDto.RequestId, cancellationToken);

      if (request.RequestStatus != RequestStatus.Pending)
            throw new BadRequestException($"Transport request is already {request.RequestStatus}.");

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

        if (userRoles.Contains((UserRoles)request.PendingApprovalBy) == false)
            throw new UnauthorizedAccessException($"Approval not allowed. Current pending approval is by {request.PendingApprovalBy}.");

        if (!approvalDto.IsApproved)
        {
            request.ApprovedOrRejectedBy.Add(loggedUser.Name);
            var updates = new Dictionary<string, object>
            {
                { nameof(request.RequestStatus), RequestStatus.Rejected },
                { nameof(request.PendingApprovalBy), PendingApprovalBy.None },
                { nameof(request.Comment), approvalDto.Comment },
                { nameof(request.StatusChangedDate), DateTime.Now },
                { nameof(request.ApprovedOrRejectedBy), request.ApprovedOrRejectedBy },
            };

            await _inventoryRequestRepository.PatchAsync(request.Id, updates, cancellationToken);
            return $"Inventory request {approvalDto.RequestId} has been rejected with comments: {approvalDto.Comment}";
        }

        request.Comment = approvalDto.Comment;
        request.ApprovedOrRejectedBy.Add(loggedUser.Name);

        var prioritizedRole = userRoles
            .OrderByDescending(role => (int)role) 
            .First();

        switch (prioritizedRole)
        {
            case UserRoles.Supervisor:
                if (request.PendingApprovalBy != PendingApprovalBy.Supervisor)
                    throw new UnauthorizedAccessException("Supervisor cannot approve this request at this stage.");
                request.PendingApprovalBy = PendingApprovalBy.Administrative;
                request.RequestStatus = RequestStatus.Approved;
                request.StatusChangedDate = DateTime.Now;
                break;

            case UserRoles.Administrative:
                if (request.PendingApprovalBy != PendingApprovalBy.Administrative)
                    throw new UnauthorizedAccessException("Administrative user cannot approve this request at this stage.");
                request.PendingApprovalBy = PendingApprovalBy.AreaAdministrator;
                request.RequestStatus = RequestStatus.ApprovedByAdmin;
                request.StatusChangedDate = DateTime.Now;
                break;

            case UserRoles.AreaAdministrator:
                if (request.PendingApprovalBy != PendingApprovalBy.AreaAdministrator)
                    throw new UnauthorizedAccessException("Area Administrator cannot approve this request at this stage.");
                request.PendingApprovalBy = PendingApprovalBy.None;
                request.RequestStatus = RequestStatus.Dispatched;
                request.StatusChangedDate = DateTime.Now;
                break;

            default:
                throw new UnauthorizedAccessException("Role not authorized for approval.");
        }

        var updatesApproval = new Dictionary<string, object>
        {
            { nameof(request.RequestStatus), request.RequestStatus },
            { nameof(request.PendingApprovalBy), request.PendingApprovalBy },
            { nameof(request.StatusChangedDate), request.StatusChangedDate },
            { nameof(request.Comment), request.Comment },
            { nameof(request.ApprovedOrRejectedBy), request.ApprovedOrRejectedBy }
        };

        await _inventoryRequestRepository.PatchAsync(request.Id, updatesApproval, cancellationToken);

        return $"Inventory request {approvalDto.RequestId} has been approved with comments: {approvalDto.Comment}";
    }
}



