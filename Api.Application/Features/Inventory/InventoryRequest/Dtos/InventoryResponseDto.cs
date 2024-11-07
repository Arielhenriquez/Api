using Api.Application.Features.Collaborators.Dtos;
using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Domain.Enums;

namespace Api.Application.Features.Inventory.InventoryRequest.Dtos;

public class InventoryResponseDto
{
    public Guid Id { get; set; }
    public CollaboratorResponseDto Collaborator { get; set; }
    public DateTime RequestDate { get; set; }
    public RequestStatus RequestStatus { get; set; }

    public IEnumerable<InventoryItemResponseDto> InventoryRequestItems { get; set; } = [];
}
