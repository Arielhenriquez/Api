using Api.Domain.Enums;

namespace Api.Domain.Entities.InventoryEntities;

public class InventoryItem : BaseEntity
{
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
    public string? DeleteComment { get; set; }
    public InventoryItemStatus InventoryItemStatus { get; set; }
    public ICollection<InventoryRequestItem> InventoryRequestItems { get; set; } = [];
}
