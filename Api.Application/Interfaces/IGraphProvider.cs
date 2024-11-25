using Api.Application.Features.Collaborators.Dtos;
using Microsoft.Graph.Models;

namespace Api.Application.Interfaces;

public interface IGraphProvider
{
    Task<User> FindUserAsync(string userOidOrUserPrincipalName);
    Task<DirectoryObject> GetUserManager(string userOid);
    Task<GraphUserDto> FindUserWithManagerAsync(string userOid);
    Task<User> UserPrincipalExists(string userPrincipalName);
    //Task<IEnumerable<AppRole>> GetAppRoles(string userId);
    Task<ServicePrincipal> GetAppRoles(string userId);
    Task<List<AppRoleDto>> GetAppRolesAssignedToUser(string userId, Guid servicePrincipalId);
    Task<AppRoleAssignmentCollectionResponse> GetAppRolesAssignments(string userId, Guid servicePrincipalId);
    Task<AppRoleAssignment> AddPermissionToUser(AssignRoleToUserDto assignRoleToUserDto, CancellationToken cancellationToken);
    Task<AppRoleAssignment> DeletePermission(DeleteRoleFromUserDto command, CancellationToken cancellationToken);
}
