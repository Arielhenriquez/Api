using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Domain.Entities.InventoryEntities;

public class InventoryRequest : BaseRequestEntity
{
    public ICollection<InventoryRequestItem> InventoryRequestItems { get; set; } = [];
    [Column(TypeName = "json")]
    public List<ApprovalEntry> ApprovalHistory { get; set; } = [];
}

public class ApprovalEntry
{
    public string? ApproverName { get; set; }
    public string? Status { get; set; }  
}
