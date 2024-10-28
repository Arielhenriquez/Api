namespace Api.Domain.Entities.InventoryEntities;

public class InventoryItem : BaseEntity
{
    public required string Name { get; set; }
    public required int Quantity { get; set; }
    public string? UnitOfMeasure { get; set; }
    public ICollection<InventoryRequestItem> ArticleRequestArticles { get; set; } = [];
}
