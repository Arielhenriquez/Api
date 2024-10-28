using Api.Domain.Entities.TransportEntities;

namespace Api.Domain.Entities.InventoryEntities;

public class Collaborator : BaseEntity
{
    public required string Name { get; set; }
    public required string Supervisor { get; set; }
    public required string Department { get; set; }
    public ICollection<InventoryRequest> ArticleRequests { get; set; } = [];
    public ICollection<TransportRequest> TransportRequests { get; set; } = [];
}
