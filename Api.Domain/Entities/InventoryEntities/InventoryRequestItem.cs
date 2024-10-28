namespace Api.Domain.Entities.InventoryEntities;

public class InventoryRequestItem : BaseEntity
{
    public Guid InventoryRequestId { get; set; }
    public InventoryRequest InventoryRequest { get; set; }

    public Guid InventoryItemId { get; set; }
    public InventoryItem InventoryItem { get; set; }
}
