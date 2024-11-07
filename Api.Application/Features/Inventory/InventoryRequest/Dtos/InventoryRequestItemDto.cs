namespace Api.Application.Features.Inventory.InventoryRequest.Dtos;

public class InventoryRequestItemDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required int Quantity { get; set; }
    public string? UnitOfMeasure { get; set; }
}
