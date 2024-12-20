using System.ComponentModel.DataAnnotations.Schema;
using Api.Domain.Enums;

namespace Api.Domain.Entities.InventoryEntities;

public class InventoryRequest : BaseRequestEntity
{
    public InventoryRequestStatus RequestStatus { get; set; } = InventoryRequestStatus.Pending;
    public ICollection<InventoryRequestItem> InventoryRequestItems { get; set; } = [];
    [Column(TypeName = "json")]
    //Todo validar json jj hacer otra tabla a terror
    public List<ApprovalEntry> ApprovalHistory { get; set; } = [];
}

public class ApprovalEntry
{
    public string? ApproverName { get; set; }
    public string? Status { get; set; }  
}
