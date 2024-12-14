using System.ComponentModel.DataAnnotations.Schema;
using Api.Domain.Entities.InventoryEntities;
using Api.Domain.Entities.TransportEntities;

namespace Api.Domain.Entities;

public class Collaborator : BaseEntity
{
    public required string UserOid { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    //Todo acota ete tiguere u,u
    public required string Supervisor { get; set; }
    public required string Department { get; set; }
    [Column(TypeName = "json")] 
    public List<string> Roles { get; set; } = [];

    [Column(TypeName = "json")]
    public List<string> Approvers { get; set; } = [];
    public ICollection<InventoryRequest> InventoryRequest { get; set; } = [];
    public ICollection<TransportRequest> TransportRequests { get; set; } = [];
}
