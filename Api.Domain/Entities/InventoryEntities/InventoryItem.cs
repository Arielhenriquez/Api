namespace Api.Domain.Entities.InventoryEntities;

public class InventoryItem : BaseEntity
{
    public required string Name { get; set; }
    public required int Quantity { get; set; }
    public string? UnitOfMeasure { get; set; }
    public decimal? Value { get; set; }
    public ICollection<InventoryRequestItem> InventoryRequestItems { get; set; } = [];
}
