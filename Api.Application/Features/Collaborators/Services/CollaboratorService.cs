using Api.Application.Common.Exceptions;
using Api.Application.Common.Pagination;
using Api.Application.Features.Collaborators.Dtos;
using Api.Application.Features.Collaborators.Dtos.GraphDtos;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Collaborators;
using Microsoft.Graph.Models;

namespace Api.Application.Features.Collaborators.Services;

public class CollaboratorService : ICollaboratorService
{
    private readonly ICollaboratorRepository _collaboratorRepository;
    private readonly IGraphProvider _graphProvider;

    public CollaboratorService(ICollaboratorRepository collaboratorRepository, IGraphProvider graphProvider)
    {
        _collaboratorRepository = collaboratorRepository;
        _graphProvider = graphProvider;
    }
    public Task<Paged<CollaboratorResponseDto>> GetPagedCollaborators(PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        return _collaboratorRepository.SearchAsync(paginationQuery, cancellationToken);
    }

    public async Task<List<CollaboratorResponseDto>> FindCollaboratorByName(string criteria)
    {
        var collaborators = await _collaboratorRepository.GetByName(criteria);

        if (string.IsNullOrWhiteSpace(criteria))
        {
            return [];
        }

        if (collaborators == null || collaborators.Count == 0)
        {
            throw new NotFoundException($"No collaborators found with name containing: {criteria}");
        }

        return collaborators;
    }

    public async Task<CollaboratorResponseDto> GetCollaboratorById(Guid id)
    {
        return await _collaboratorRepository.GetById(id);
    }

    public async Task<GraphUserDto> GetGraphUsers(string userOid)
    {
        return await _graphProvider.FindUserWithManagerAsync(userOid);
    }

    public async Task<DirectoryObject> GetUserManager(string userOid)
    {
        return await _graphProvider.GetUserManager(userOid);
    }

    public async Task<List<RolesResponseDto>> GetAllRoles(CancellationToken cancellationToken)
    {
        var appRoles = await _graphProvider.GetAppRoles(cancellationToken);

        var rolesDto = appRoles
            .Select(role => (RolesResponseDto)role)
            .ToList();

        return rolesDto ?? [];
    }

    public async Task<List<RoleAssignmentDto>> GetUserRoleAssignments(string userId, bool isAssigned, CancellationToken cancellationToken)
    {
        var assignedRoles = await _graphProvider.GetAssignedRolesAsync(userId, cancellationToken);
        var allRoles = await _graphProvider.GetAppRoles(cancellationToken);

        List<RoleAssignmentDto> result;

        if (isAssigned)
        {
            result = assignedRoles
                .Select(assignment =>
                {
                    var appRole = allRoles.FirstOrDefault(role => role.Id == assignment.AppRoleId);
                    return appRole == null ? null : (RoleAssignmentDto)(appRole, assignment);
                })
                .Where(dto => dto != null)
                .ToList();
        }
        else
        {
            var unassignedRoles = allRoles
                .Where(appRole => !assignedRoles.Any(assignment => assignment.AppRoleId == appRole.Id))
                .ToList();

            result = unassignedRoles
                .Select(appRole => new RoleAssignmentDto
                {
                    RoleId = appRole.Id,
                    RoleName = appRole.DisplayName ?? string.Empty,
                    RoleDescription = appRole.Description ?? string.Empty
                })
                .ToList();
        }

        return result;
    }

    public async Task<List<RoleAssignmentResultDto>> AddRolesToUser(AssignRolesToUserDto assignRoleToUserDto, CancellationToken cancellationToken)
    {
        var results = new List<RoleAssignmentResultDto>();

        foreach (var roleId in assignRoleToUserDto.RoleIds)
        {
            var roleAssignmentExists = await _graphProvider.CheckRoleAssignmentExists(assignRoleToUserDto.UserId, roleId, cancellationToken);

            if (roleAssignmentExists)
            {
                results.Add(new RoleAssignmentResultDto
                {
                    RoleId = Guid.Parse(roleId),
                    UserId = Guid.Parse(assignRoleToUserDto.UserId),
                    Message = "Role is already assigned."
                });
                continue;
            }
            try
            {
                var roleAssignment = await _graphProvider.AddPermissionsToUser(assignRoleToUserDto.UserId, roleId, cancellationToken);

                results.Add(new RoleAssignmentResultDto
                {
                    RoleId = Guid.Parse(roleId),
                    UserId = Guid.Parse(assignRoleToUserDto.UserId),
                    Message = "Role assigned successfully."
                });
            }
            catch (Exception ex)
            {
                results.Add(new RoleAssignmentResultDto
                {
                    RoleId = Guid.Parse(roleId),
                    UserId = Guid.Parse(assignRoleToUserDto.UserId),
                    Message = $"Failed to assign role: {ex.Message}"
                });
            }
        }

        return results;
    }


    public async Task DeleteRolesFromUser(DeleteRoleFromUserDto deleteRoleFromUserDto, CancellationToken cancellationToken)
    {
        var tasks = new List<Task>();

        foreach (var roleAssignmentId in deleteRoleFromUserDto.AppRoleAssignmentIds)
        {
            var roleAssignmentExists = await _graphProvider.CheckRoleAssignmentExists(deleteRoleFromUserDto.UserId, roleAssignmentId, cancellationToken, true);

            if (!roleAssignmentExists)
            {
                continue;
            }

            var task = _graphProvider.DeletePermissionsFromUser(deleteRoleFromUserDto.UserId, roleAssignmentId, cancellationToken);

            tasks.Add(task);
        }
        await Task.WhenAll(tasks);
    }
}
