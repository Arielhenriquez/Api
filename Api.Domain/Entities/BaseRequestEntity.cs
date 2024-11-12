using Api.Domain.Enums;

namespace Api.Domain.Entities;

public interface IBaseRequestEntity : IBase
{
    public Guid CollaboratorId { get; set; }
    public Collaborator Collaborator { get; set; }
    public RequestStatus RequestStatus { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public PendingApprovalBy? PendingApprovalBy { get; set; } 
    public string? Comment { get; set; }
}
public class BaseRequestEntity : BaseEntity, IBaseRequestEntity
{
    public Guid CollaboratorId { get; set; }
    public Collaborator Collaborator { get; set; }
    public RequestStatus RequestStatus { get; set; } = RequestStatus.Pending;
    public DateTime? ApprovedDate { get; set; }
    public PendingApprovalBy? PendingApprovalBy { get; set; }
    public string? Comment { get; set; }
}
