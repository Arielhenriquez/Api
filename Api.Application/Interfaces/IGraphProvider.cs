using Microsoft.Graph.Models;

namespace Api.Application.Interfaces;

public interface IGraphProvider
{
    Task<User> FindUserAsync(string userOidOrUserPrincipalName);
    Task<DirectoryObject> GetUserManager(string userOid);
    Task<User> UserPrincipalExists(string userPrincipalName);
    Task<List<AppRole>> GetAppRoles(CancellationToken cancellationToken);
    Task<List<AppRoleAssignment>> GetAssignedRolesAsync(string userId, CancellationToken cancellationToken);
    Task<bool> CheckRoleAssignmentExists(string userId, string roleIdOrAssignmentId, CancellationToken cancellationToken, bool validateByAssignmentId = false);
    Task<AppRoleAssignment> AddPermissionsToUser(string userId, string roleId, CancellationToken cancellationToken);
    Task DeletePermissionsFromUser(string userId, string roleAssignmentId, CancellationToken cancellationToken);
}
