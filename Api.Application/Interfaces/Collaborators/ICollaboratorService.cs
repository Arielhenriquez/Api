using Api.Application.Common.Pagination;
using Api.Application.Features.Collaborators.Dtos;
using Microsoft.Graph.Models;

namespace Api.Application.Interfaces.Collaborators;

public interface ICollaboratorService
{
    Task<Paged<CollaboratorResponseDto>> GetPagedCollaborators(PaginationQuery paginationQuery, CancellationToken cancellationToken);
    Task<CollaboratorResponseDto> GetCollaboratorById(Guid id);
    Task<List<CollaboratorResponseDto>> FindCollaboratorByName(string criteria);
    Task<ServicePrincipal> GetAppRole(string userId);
    Task<List<AppRoleDto?>> GetAppRoles(string userId);
    Task<GraphUserDto> GetGraphUsers(string userOid);
    Task<AppRoleAssignmentCollectionResponse> GetAppRolesAssignments(string userId);
    Task<DirectoryObject> GetUserManager(string userOid);
    Task<AppRoleAssignment> AddPermissionToUser(AssignRoleToUserDto command, CancellationToken cancellationToken);
    Task<AppRoleAssignment> DeleteRoleUser(DeleteRoleFromUserDto assignRoleToUserDto, CancellationToken cancellationToken);
}
