namespace Api.Domain.Entities.InventoryEntities;

public class InventoryRequestItem : BaseEntity
{
    public Guid ArticleRequestId { get; set; }
    public InventoryRequest ArticleRequest { get; set; }

    public Guid ArticleId { get; set; }
    public InventoryItem Article { get; set; }
}
