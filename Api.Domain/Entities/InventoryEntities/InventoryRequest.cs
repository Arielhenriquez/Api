using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Domain.Entities.InventoryEntities;

public class InventoryRequest : BaseRequestEntity
{
    public ICollection<InventoryRequestItem> InventoryRequestItems { get; set; } = [];
    [Column(TypeName = "json")]
    public List<string> ApprovedOrRejectedBy { get; set; } = [];
}
