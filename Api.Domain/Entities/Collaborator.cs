using Api.Domain.Entities.InventoryEntities;
using Api.Domain.Entities.TransportEntities;
using Api.Domain.Enums;

namespace Api.Domain.Entities;

public class Collaborator : BaseEntity
{
    public required string UserOid { get; set; }
    public required string Name { get; set; }
    public required string Supervisor { get; set; }
    public required string Department { get; set; }
    public ICollection<InventoryRequest> InventoryRequest { get; set; } = [];
    public ICollection<TransportRequest> TransportRequests { get; set; } = [];
    public UserRoles Roles { get; set; } = UserRoles.Applicant;
}
