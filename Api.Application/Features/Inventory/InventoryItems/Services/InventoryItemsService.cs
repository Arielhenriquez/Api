using Api.Application.Common;
using Api.Application.Common.Exceptions;
using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Inventory;
using Api.Domain.Entities.InventoryEntities;

namespace Api.Application.Features.Inventory.InventoryItems.Services;

public class InventoryItemsService(IBaseRepository<InventoryItem> repository, IInventoryItemRepository inventoryItemRepository) :
    BaseService<InventoryItem, InventoryItemRequestDto, InventoryItemResponseDto>(repository), IInventoryItemsService
{
    private readonly IInventoryItemRepository _inventoryItemRepository = inventoryItemRepository;

    public async Task<IEnumerable<InventoryItemResponseDto>> FindInventoryItemByName(string criteria)
    {
        var inventoryItems = await _inventoryItemRepository.GetByName(criteria);

        if (string.IsNullOrWhiteSpace(criteria))
        {
            var allItems = await repository.GetAll();

            return allItems.Select(item => new InventoryItemResponseDto
            {
                Id = item.Id,
                InstitutionalCode = item.InstitutionalCode,
                Category = item.Category,
                WarehouseObjectAccount = item.WarehouseObjectAccount,
                AcquisitionObjectAccount = item.AcquisitionObjectAccount,
                Name = item.Name,
                Quantity = item.Quantity,
                RequestedQuantity = item.RequestedQuantity,
                UnitOfMeasure = item.UnitOfMeasure,
                Value = item.Value,
                Section = item.Section,
            });
        }

        if (inventoryItems == null || inventoryItems.Count == 0)
        {
            throw new NotFoundException($"No inventory Items found with name containing: {criteria}");
        }

        return inventoryItems;
    }

    public Task<Paged<InventoryItemResponseDto>> GetPagedInventoryItems(PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        return _inventoryItemRepository.SearchAsync(paginationQuery, cancellationToken);
    }

    public async Task UpdateArticleQuantity(Guid id, UpdateArticleQuantityDto updateArticle, CancellationToken cancellationToken)
    {
        var article = await repository.GetById(id, cancellationToken);
        var updates = new Dictionary<string, object>
        {
             { nameof(article.Quantity), updateArticle.Quantity}
        };
        article.Quantity = updateArticle.Quantity;
        await repository.PatchAsync(id, updates, cancellationToken);
    }

    protected override InventoryItemResponseDto MapToDto(InventoryItem entity)
    {
        return new InventoryItemResponseDto
        {
            Id = entity.Id,
            InstitutionalCode = entity.InstitutionalCode,
            Category = entity.Category,
            WarehouseObjectAccount = entity.WarehouseObjectAccount,
            AcquisitionObjectAccount = entity.AcquisitionObjectAccount,
            Name = entity.Name,
            Quantity = entity.Quantity,
            RequestedQuantity = entity.RequestedQuantity,
            UnitOfMeasure = entity.UnitOfMeasure,
            Value = entity.Value,
            Section = entity.Section,
        };
    }

    protected override InventoryItem MapToEntity(InventoryItemRequestDto dto)
    {
        return new InventoryItem
        {
            Id = dto.Id,
            InstitutionalCode = dto.InstitutionalCode,
            Category = dto.Category,
            WarehouseObjectAccount = dto.WarehouseObjectAccount,
            AcquisitionObjectAccount = dto.AcquisitionObjectAccount,
            Name = dto.Name,
            Quantity = dto.Quantity,
            RequestedQuantity = dto.RequestedQuantity,
            UnitOfMeasure = dto.UnitOfMeasure,
            Value = dto.Value,
            Section = dto.Section,
        };
    }

    protected override void UpdateEntity(InventoryItem entity, InventoryItemRequestDto dto)
    {
        entity.InstitutionalCode = dto.InstitutionalCode;
        entity.Category = dto.Category;
        entity.WarehouseObjectAccount = dto.WarehouseObjectAccount;
        entity.AcquisitionObjectAccount = dto.AcquisitionObjectAccount;
        entity.Name = dto.Name;
        entity.Quantity = dto.Quantity;
        entity.RequestedQuantity = dto.RequestedQuantity;
        entity.UnitOfMeasure = dto.UnitOfMeasure;
        entity.Value = dto.Value;
        entity.Section = dto.Section;
    }
}
