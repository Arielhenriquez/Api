using Api.Application.Common.Exceptions;
using Api.Application.Common.Extensions;
using Api.Application.Common.Pagination;
using Api.Application.Features.Collaborators.Dtos;
using Api.Application.Features.Collaborators.Dtos.GraphDtos;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Collaborators;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Features.Collaborators.Services;

public class CollaboratorService : ICollaboratorService
{
    private readonly ICollaboratorRepository _collaboratorRepository;
    private readonly IBaseRepository<Collaborator> _baseRepository;
    private readonly IGraphProvider _graphProvider;

    public CollaboratorService(ICollaboratorRepository collaboratorRepository, IGraphProvider graphProvider, IBaseRepository<Collaborator> baseRepository)
    {
        _collaboratorRepository = collaboratorRepository;
        _graphProvider = graphProvider;
        _baseRepository = baseRepository;
    }
    public Task<Paged<CollaboratorResponseDto>> GetPagedCollaborators(PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        return _collaboratorRepository.SearchAsync(paginationQuery, cancellationToken);
    }

    public async Task<List<CollaboratorResponseDto>> FindCollaboratorByEmail(string criteria)
    {
        var collaborators = await _collaboratorRepository.GetByEmail(criteria);

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

    public async Task UpdateRoles(CollaboratorRequest collaboratorRequest)
    {
        var appRoles = collaboratorRequest.RolesDescriptions
            .Select(role => Enum.TryParse<UserRoles>(role, out var parsedRole) ? parsedRole : (UserRoles?)null)
            .Where(role => role != null)
            .Cast<UserRoles>()
            .ToList();

        var existingCollaborator = await _baseRepository.GetById(collaboratorRequest.Id, CancellationToken.None);

        var updatedRoles = appRoles.Select(EnumExtensions.MapEnumToDbRole).ToList();

        if (!updatedRoles.SequenceEqual(existingCollaborator.Roles ?? []))
        {
            existingCollaborator.Roles = updatedRoles;
            await _collaboratorRepository.UpdateAsync(existingCollaborator);
        }
    }
    public async Task<CollaboratorResponseDto> GetCollaboratorById(Guid id)
    {
        return await _collaboratorRepository.GetById(id);
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

        bool hasMaxRolesInAzure = await CheckIfUserHasMaxRoles(assignRoleToUserDto.UserId, cancellationToken);
        if (hasMaxRolesInAzure)
            throw new BadRequestException("The user already has 2 roles Azure, cannot assign more.");

        var collaborator = await _baseRepository.Query()
            .FirstOrDefaultAsync(c => c.UserOid == assignRoleToUserDto.UserId, cancellationToken);

        if (collaborator != null && collaborator.Roles.Count >= 2)
            throw new BadRequestException("The user already has 2 roles in the database, cannot assign more.");

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

                var assignedRoles = await GetUserRoleAssignments(assignRoleToUserDto.UserId, true, cancellationToken);
                var roleValues = assignedRoles.Select(x => x.RoleValue);
                await AddRolesInDatabase(assignRoleToUserDto.UserId, roleValues);

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

    public async Task<bool> CheckIfUserHasMaxRoles(string userId, CancellationToken cancellationToken)
    {
        var userRoles = await GetUserRoleAssignments(userId, true, cancellationToken);

        if (userRoles.Count >= 2)
        {
            return true;
        }

        return false;
    }

    private async Task AddRolesInDatabase(string userId, IEnumerable<string> graphRoleValues)
    {
        var appRoles = graphRoleValues
            .Select(EnumExtensions.MapDbRoleToEnum)
            .Where(role => role != null) 
            .Cast<UserRoles>()
            .ToList();

        var existingCollaborator = await _baseRepository.Query()
            .AsTracking()
            .FirstOrDefaultAsync(c => c.UserOid == userId)
            ?? throw new NotFoundException("Collaborator not found");

        var updatedRoles = appRoles
            .Select(role => EnumExtensions.MapEnumToDbRole(role)!)
            .ToList();

        if (!updatedRoles.SequenceEqual(existingCollaborator.Roles ?? []))
        {
            existingCollaborator.Roles = updatedRoles;
            await _collaboratorRepository.UpdateAsync(existingCollaborator);
        }
    }

    public async Task<List<DeleteRoleAssignmentResultDto>> DeleteRolesFromUser(DeleteRoleFromUserDto deleteRoleFromUserDto, CancellationToken cancellationToken)
    {
        var results = new List<DeleteRoleAssignmentResultDto>();

        var assignedRoles = await GetUserRoleAssignments(deleteRoleFromUserDto.UserId, true, cancellationToken);

        foreach (var roleId in deleteRoleFromUserDto.AppRoleAssignmentIds)
        {
            try
            {
                var roleToRemove = assignedRoles
                    .FirstOrDefault(r => r.RoleAssignmentId == roleId);

                if (roleToRemove == null)
                {
                    results.Add(new DeleteRoleAssignmentResultDto
                    {
                        RoleId = roleId,
                        UserId = deleteRoleFromUserDto.UserId,
                        Message = "Role not found for user."
                    });
                    continue;
                }

                await _graphProvider.DeletePermissionsFromUser(deleteRoleFromUserDto.UserId, roleToRemove.RoleAssignmentId, cancellationToken);

                await RemoveRoleFromDatabase(deleteRoleFromUserDto.UserId, roleToRemove.RoleValue, cancellationToken);

                results.Add(new DeleteRoleAssignmentResultDto
                {
                    RoleId = roleId,
                    UserId = deleteRoleFromUserDto.UserId,
                    Message = "Role removed successfully."
                });
            }
            catch (BadRequestException ex)
            {
                results.Add(new DeleteRoleAssignmentResultDto
                {
                    RoleId = roleId,
                    UserId = deleteRoleFromUserDto.UserId,
                    Message = $"Failed to remove role: {ex.Message}"
                });
            }
        }
        return results;
    }

    private async Task RemoveRoleFromDatabase(string userId, string graphRoleValue, CancellationToken cancellationToken)
    {
        var collaborator = await _baseRepository
            .Query()
            .FirstOrDefaultAsync(c => c.UserOid == userId, cancellationToken)
            ?? throw new NotFoundException($"Collaborator with UserOid {userId} not found.");

        var dbRole = EnumExtensions.MapEnumToDbRole(EnumExtensions.MapDbRoleToEnum(graphRoleValue)
            ?? throw new ArgumentException($"Invalid role value: {graphRoleValue}"));

        if (collaborator.Roles.Contains(dbRole))
        {
            collaborator.Roles.Remove(dbRole);
            await _collaboratorRepository.UpdateAsync(collaborator, cancellationToken);
        }
        else
        {
            throw new BadRequestException($"Role {dbRole} does not exist for the collaborator.");
        }
    }
}
