using System.Text.Json.Serialization;

namespace Api.Application.Features.Inventory.InventoryItems.Dtos;

public class InventoryItemResponseDto
{
    public Guid Id { get; set; } 
    public required string Name { get; set; }
    public required int Quantity { get; set; }
    public string? UnitOfMeasure { get; set; }
    public decimal? Value { get; set; }
}
