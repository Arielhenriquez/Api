using Api.Application.Features.Collaborators.Dtos;
using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Domain.Enums;
using InventoryEntity = Api.Domain.Entities.InventoryEntities.InventoryRequest;

namespace Api.Application.Features.Inventory.InventoryRequest.Dtos;

public class InventoryResponseDto
{
    public Guid Id { get; set; }
    public required CollaboratorResponseDto Collaborator { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public RequestStatus RequestStatus { get; set; }
    public IEnumerable<InventoryItemResponseDto> InventoryRequestItems { get; set; } = [];

    public static implicit operator InventoryResponseDto(InventoryEntity inventory)
    {
        return new InventoryResponseDto
        {
            Id = inventory.Id,
            Collaborator = inventory.Collaborator != null ? (CollaboratorResponseDto)inventory.Collaborator : null,
            CreatedDate = inventory.CreatedDate,
            RequestStatus = inventory.RequestStatus,
            InventoryRequestItems = inventory.InventoryRequestItems
                .Select(iri => new InventoryItemResponseDto
                {
                    Id = iri.InventoryItemId,
                    Name = iri.InventoryItem.Name,
                    Quantity = iri.InventoryItem?.Quantity ?? 0,
                    UnitOfMeasure = iri.InventoryItem?.UnitOfMeasure,
                    Value = iri.InventoryItem?.Value
                })
                .ToList()
        };
    }
}