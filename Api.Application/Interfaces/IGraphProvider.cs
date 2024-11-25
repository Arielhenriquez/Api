namespace Api.Application.Interfaces;

public interface IGraphProvider
{
    //Task<User> FindUserAsync(string userOidOrUserPrincipalName);
    //Task<User> UserPrincipalExists(string userPrincipalName);
    //Task<IEnumerable<AppRoles>> GetAppRoles(string appRoleId);
    //Task<AppRoleAssignment> AddPermissionToUser(string userId, Guid permissionId)
    //Task<AppRoleAssignment> DeletePermission(RoleToUserDto command, CancellationToken cancellationToken);
}

public class RoleToUserDto
{
    public required string UserId { get; set; }
    //public ActionsTypes? Type { get; set; }
    public required string AppRoleAssignmentId { get; set; }
}
