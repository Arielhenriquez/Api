namespace Api.Domain.Entities.InventoryEntities;

public class InventoryRequest : BaseEntity
{
    public Guid CollaboratorId { get; set; }
    public Collaborator Collaborator { get; set; }
    public DateTime RequestDate { get; set; }
    public string ApprovalStatus { get; set; }

    public ICollection<InventoryRequestItem> ArticleRequestArticles { get; set; } = [];
}
