using Api.Domain.Enums;

namespace Api.Application.Features.Inventory.InventoryRequest.Dtos;

public class InventoryRequestDto
{
    public Guid CollaboratorId { get; set; }
    public List<Guid> ArticlesIds { get; set; } = [];
}
