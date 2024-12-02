namespace Api.Application.Features.Inventory.InventoryItems.Dtos;

public class ApprovalDto
{
    public required Guid RequestId { get; set; }
    public Guid CollaboratorId { get; set; }
    public bool IsApproved { get; set; }
    public string? Comment { get; set; }
}
