using Api.Domain.Enums;

namespace Api.Application.Features.Inventory.InventoryRequest.Dtos;

public class InventoryRequestDto
{
    public Guid CollaboratorId { get; set; }
    public RequestStatus RequestStatus { get; set; } = RequestStatus.Pending;
    public DateTime RequestDate { get; set; }
    public List<InventoryRequestItemDto> Articles { get; set; } = [];
}
