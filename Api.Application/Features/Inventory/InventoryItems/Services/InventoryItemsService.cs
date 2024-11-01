﻿using Api.Application.Common;
using Api.Application.Common.Exceptions;
using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Application.Interfaces;
using Api.Domain.Entities.InventoryEntities;

namespace Api.Application.Features.Inventory.InventoryItems.Services;

public class InventoryItemsService(IBaseRepository<InventoryItem> repository, IInventoryItemRepository inventoryItemRepository) :
    BaseService<InventoryItem, InventoryItemRequestDto, InventoryItemResponseDto>(repository), IInventoryItemsService
{
    private readonly IInventoryItemRepository _inventoryItemRepository = inventoryItemRepository;

    public async Task<List<InventoryItemResponseDto>> FindInventoryItemByName(string criteria)
    {
        var inventoryItems = await _inventoryItemRepository.GetByName(criteria);

        if (inventoryItems == null || inventoryItems.Count == 0)
        {
            throw new NotFoundException($"No collaborators found with name containing: {criteria}");
        }

        return inventoryItems;
    }

    public Task<Paged<InventoryItemResponseDto>> GetPagedInventoryItems(PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        return _inventoryItemRepository.SearchAsync(paginationQuery, cancellationToken);
    }

    protected override InventoryItemResponseDto MapToDto(InventoryItem entity)
    {
        return new InventoryItemResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Quantity = entity.Quantity,
            UnitOfMeasure = entity.UnitOfMeasure,
            Value = entity.Value
        };
    }

    protected override InventoryItem MapToEntity(InventoryItemRequestDto dto)
    {
        return new InventoryItem
        {
            Id = dto.Id,
            Name = dto.Name,
            Quantity = dto.Quantity,
            UnitOfMeasure = dto.UnitOfMeasure,
        };
    }

    protected override void UpdateEntity(InventoryItem entity, InventoryItemRequestDto dto)
    {
        entity.Name = dto.Name;
        entity.Quantity = dto.Quantity;
        entity.UnitOfMeasure = dto.UnitOfMeasure;
    }
}
