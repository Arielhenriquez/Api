using Api.Application.Common.Exceptions;
using Api.Application.Common.Extensions;
using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryRequest.Dtos;
using Api.Application.Features.Inventory.InventoryRequest.Projections;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Collaborators;
using Api.Application.Interfaces.Inventory;
using Api.Domain.Constants;
using Api.Domain.Entities.InventoryEntities;
using Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using InventoryEntity = Api.Domain.Entities.InventoryEntities.InventoryRequest;

namespace Api.Application.Features.Inventory.InventoryRequest.Services;

public class InventoryRequestService : IInventoryRequestService
{
    /*Todo: Endpoint para que un Supervisor pueda aprobar o rechazar solicitud  */

    private readonly ICollaboratorRepository _collaboratorRepository;
    private readonly IBaseRepository<InventoryEntity> _inventoryRequestRepository;
    private readonly IBaseRepository<InventoryItem> _inventoryItemRepository;
    private readonly IBaseRepository<InventoryRequestItem> _inventoryRequestItemRepository;
    private readonly IEmailService _emailService;


    public InventoryRequestService(ICollaboratorRepository collaboratorRepository,
        IBaseRepository<InventoryEntity> inventoryRequestRepository,
        IBaseRepository<InventoryRequestItem> inventoryRequestItemRepository,
        IBaseRepository<InventoryItem> inventoryItemRepository,
        IEmailService emailService)
    {
        _collaboratorRepository = collaboratorRepository;
        _inventoryRequestRepository = inventoryRequestRepository;
        _inventoryRequestItemRepository = inventoryRequestItemRepository;
        _inventoryItemRepository = inventoryItemRepository;
        _emailService = emailService;
    }

    public async Task AddInventoryRequest(InventoryRequestDto inventoryRequestDto, CancellationToken cancellationToken)
    {
        var collaborator = await _collaboratorRepository.GetById(inventoryRequestDto.CollaboratorId, cancellationToken);

        var inventoryRequestEntity = new InventoryEntity
        {
            RequestDate = inventoryRequestDto.RequestDate,
            CollaboratorId = collaborator.Id,
            RequestStatus = RequestStatus.Pending,
        };
        var createdInventoryRequest = await _inventoryRequestRepository.AddAsync(inventoryRequestEntity, cancellationToken);

        var invalidArticles = new List<InventoryRequestItemDto>();

        foreach (var itemDto in inventoryRequestDto.Articles)
        {
            try
            {
                var article = await _inventoryItemRepository.GetById(itemDto.Id, cancellationToken);

                var inventoryRequestItem = new InventoryRequestItem
                {
                    InventoryRequestId = createdInventoryRequest.Id,
                    InventoryItemId = article.Id,
                };
                await _inventoryRequestItemRepository.AddAsync(inventoryRequestItem, cancellationToken);
            }
            catch (NotFoundException)
            {
                invalidArticles.Add(itemDto);
            }
        }
        if (invalidArticles.Count != 0)
        {
            throw new NotFoundException($"Los siguientes artículos no existen en el inventario: {string.Join(", ", invalidArticles.Select(a => a.Name))}");
        }

        await SendRequestEmail();
    }

    //Todo: Modificar plantilla y adecuar detalles de solicitud si es posible
    private async Task SendRequestEmail()
    {
        string htmlFile = FileExtensions.ReadEmailTemplate(EmailConstants.CreateDriverTemplate, EmailConstants.TemplateEmailRoute);
        //htmlFile = htmlFile.Replace("{{UserName}}", inventoryRequestDto.);
        await _emailService.SendEmail("supervisorEmail@gmail.com", "Te habla lebron james", htmlFile);
    }

    //TODO add dto 
    public async Task<IEnumerable<InventoryEntity>> ListInventoryRequests()
    {
        return await _inventoryRequestRepository.GetAll();
    }

    public async Task<List<InventoryResponseDto>> GetInventoryRequestDetails(PaginationQuery paginationQuery, Guid id, CancellationToken cancellationToken)
    {
        var query = _inventoryRequestRepository.Query()
         .Include(ir => ir.Collaborator) // Include Collaborator for the InventoryRequest
         .Include(ir => ir.InventoryRequestItems) // Include the collection of InventoryRequestItems
         .ThenInclude(iri => iri.InventoryItem)
         .Where(x => x.Id == id); // Then include each related InventoryItem in InventoryRequestItems

        // Apply the projection using your projection method
        var result = await query
            .Select(InventoryRequestProjections.Summary)
            .ToListAsync(cancellationToken);

        return result;
    }
}
