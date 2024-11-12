namespace Api.Domain.Entities.InventoryEntities;

public class InventoryRequest : BaseRequestEntity
{
    public ICollection<InventoryRequestItem> InventoryRequestItems { get; set; } = [];
}
