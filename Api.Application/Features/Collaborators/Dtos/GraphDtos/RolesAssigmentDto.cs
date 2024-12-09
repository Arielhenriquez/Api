using Microsoft.Graph.Models;

namespace Api.Application.Features.Collaborators.Dtos.GraphDtos;

public class RoleAssignmentDto
{
    public Guid? RoleId { get; set; }
    public  Guid? UserId { get; set; }
    public string? RoleAssignmentId { get; set; }
    public string? UserName { get; set; }
    public string? RoleName { get; set; }
    public string? RoleValue { get; set; }
    public string? RoleDescription { get; set; }

    public static implicit operator RoleAssignmentDto((AppRole appRole, AppRoleAssignment appRoleAssignment) source)
    {
        if (source.appRole == null || source.appRoleAssignment == null)
        {
            throw new ArgumentNullException("Both appRole and appRoleAssignment must be non-null.");
        }

        return new RoleAssignmentDto
        {
            RoleId = source.appRole.Id,
            UserId = source.appRoleAssignment.PrincipalId ?? Guid.Empty,
            RoleAssignmentId = source.appRoleAssignment.Id,
            UserName = source.appRoleAssignment.PrincipalDisplayName ?? string.Empty,
            RoleName = source.appRole.DisplayName ?? string.Empty,
            RoleValue = source.appRole.Value ?? string.Empty,
            RoleDescription = source.appRole.Description ?? string.Empty
        };
    }
}