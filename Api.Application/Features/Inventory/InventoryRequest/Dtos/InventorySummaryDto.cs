using Api.Application.Features.Collaborators.Dtos;
using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Domain.Enums;

namespace Api.Application.Features.Inventory.InventoryRequest.Dtos;

public class InventorySummaryDto
{
    public Guid Id { get; set; }
    public required CollaboratorResponseDto Collaborator { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public RequestStatus RequestStatus { get; set; }
    public string? RequestStatusDescription { get; set; }
    public PendingApprovalBy? PendingApproval { get; set; }
    public DateTime? StatusChangedDate { get; set; }
    public List<string>? ApprovedOrRejectedBy { get; set; }
    public IEnumerable<InventoryItemResponseDto> InventoryRequestItems { get; set; } = [];
}
