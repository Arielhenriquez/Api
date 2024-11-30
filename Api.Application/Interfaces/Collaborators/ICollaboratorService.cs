using Api.Application.Common.Pagination;
using Api.Application.Features.Collaborators.Dtos;
using Api.Application.Features.Collaborators.Dtos.GraphDtos;

namespace Api.Application.Interfaces.Collaborators;

public interface ICollaboratorService
{
    Task<Paged<CollaboratorResponseDto>> GetPagedCollaborators(PaginationQuery paginationQuery, CancellationToken cancellationToken);
    Task<CollaboratorResponseDto> GetCollaboratorById(Guid id);
    Task<List<CollaboratorResponseDto>> FindCollaboratorByName(string criteria);
    Task<List<RolesResponseDto>> GetAllRoles(CancellationToken cancellationToken);
    Task<List<RoleAssignmentDto>> GetUserRoleAssignments(string userOid, bool isAssigned, CancellationToken cancellationToken);
    Task<List<RoleAssignmentResultDto>> AddRolesToUser(AssignRolesToUserDto assignRoleToUserDto, CancellationToken cancellationToken);
    Task DeleteRolesFromUser(DeleteRoleFromUserDto deleteRoleFromUserDto, CancellationToken cancellationToken);
}
