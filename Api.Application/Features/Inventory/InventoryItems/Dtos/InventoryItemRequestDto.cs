using System.Text.Json.Serialization;

namespace Api.Application.Features.Inventory.InventoryItems.Dtos;

public class InventoryItemRequestDto
{
    [JsonIgnore]
    public Guid Id { get; set; } = Guid.NewGuid();
    public int InstitutionalCode { get; set; }
    public required string Category { get; set; }
    public required string WarehouseObjectAccount { get; set; }
    public required string AcquisitionObjectAccount { get; set; }
    public required string Name { get; set; }
    public required int Quantity { get; set; }
    public required int RequestedQuantity { get; set; }
    public string? UnitOfMeasure { get; set; }
    public decimal? Value { get; set; }
    public required string Section { get; set; }
}
