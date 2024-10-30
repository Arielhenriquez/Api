using Api.Domain.Enums;

namespace Api.Domain.Entities.InventoryEntities;

public class InventoryRequest : BaseEntity
{
    public Guid CollaboratorId { get; set; }
    public Collaborator Collaborator { get; set; }
    public DateTime RequestDate { get; set; }
    public RequestStatus RequestStatus { get; set; }

    public ICollection<InventoryRequestItem> InventoryRequestItems { get; set; } = [];
}
