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

    //Todo Test this.
    public async Task UpdateRoles(CollaboratorRequest collaborator) 
    {
        var appRoles = collaborator.RolesDescriptions
          .Select(role => Enum.TryParse<UserRoles>(role, out var parsedRole) ? parsedRole : (UserRoles?)null)
          .Where(role => role != null)
          .Cast<UserRoles>()
          .ToList();

        var collaboratorEntity = new Collaborator()
        {
            Email = "",
            Department = collaborator.Department,
            Name = collaborator.Name,
            Supervisor = collaborator.Supervisor,
            UserOid = collaborator.UserOid,
            Id = collaborator.Id
        };
        var oal = await _baseRepository.UpdateAsync(collaboratorEntity);
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
                var roleNames = assignedRoles.Select(x => x.RoleName);
                await AddRolesInDatabase(assignRoleToUserDto.UserId, roleNames);

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
        var userRoles = await _graphProvider.GetAssignedRolesAsync(userId, cancellationToken);

        if (userRoles.Count >= 2)
        {
            return true; 
        }

        return false; 
    }

    //Todo Test this.
    private async Task AddRolesInDatabase(string userId, IEnumerable<string> graphRoleValues)
    {
        // Obtén el colaborador y asegúrate de rastrearlo
        var collaborator = await _baseRepository.Query()
            .AsTracking() // Asegura que EF Core rastree cambios
            .FirstOrDefaultAsync(c => c.UserOid == userId) ?? throw new NotFoundException("Collaborator not found");

        // Itera sobre los valores de los roles
        foreach (var graphRoleValue in graphRoleValues)
        {
            // Mapea cada rol al enum de tu aplicación
            var appRole = EnumExtensions.MapDbRoleToEnum(graphRoleValue)?.ToString();

            if (appRole != null && !collaborator.Roles.Contains(graphRoleValue))
            {
                collaborator.Roles.Add(graphRoleValue);
            }

        }

        // Actualiza el colaborador en la base de datos
        await _baseRepository.UpdateAsync(collaborator, CancellationToken.None);
    }

    public async Task DeleteRolesFromUser(DeleteRoleFromUserDto deleteRoleFromUserDto, CancellationToken cancellationToken)
    {
        // Obtén las asignaciones de roles actuales del usuario en Azure
        var assignedRoles = await GetUserRoleAssignments(deleteRoleFromUserDto.UserId, true, cancellationToken);

        foreach (var roleId in deleteRoleFromUserDto.AppRoleAssignmentIds)
        {
            try
            {
                // Encuentra el nombre del rol asociado al roleId en Azure
                var roleToRemove = assignedRoles
                    .FirstOrDefault(r => r.RoleAssignmentId == roleId);

                if (roleToRemove == null)
                {
                    // Si no se encuentra la asignación, continúa con el siguiente rol
                    continue;
                }


               await _graphProvider.DeletePermissionsFromUser(deleteRoleFromUserDto.UserId, roleToRemove.RoleAssignmentId, cancellationToken);

                // Elimina el rol de la base de datos
                //await RemoveRoleFromDatabase(deleteRoleFromUserDto.UserId, roleToRemove.RoleName, cancellationToken);
            }
            catch (Exception ex)
            {
                // Opcional: Log o manejo de errores
                Console.WriteLine($"Error removing role {roleId} from user {deleteRoleFromUserDto.UserId}: {ex.Message}");
            }
        }
    }

    //Todo Test this.
    private async Task RemoveRoleFromDatabase(string userId, string graphRoleValue, CancellationToken cancellationToken)
    {
        // Obtén el colaborador desde la base de datos
        var collaborator = await _baseRepository.Query()
            .FirstOrDefaultAsync(c => c.UserOid == userId, cancellationToken);

        if (collaborator != null)
        {
            // Mapea el valor del rol al enum de tu aplicación
            var appRole = EnumExtensions.MapDbRoleToEnum(graphRoleValue)?.ToString();

            if (appRole != null && collaborator.Roles.Contains(appRole))
            {
                collaborator.Roles.Remove(appRole);

                // Actualiza el colaborador en la base de datos
                await _baseRepository.UpdateAsync(collaborator, cancellationToken);
            }
        }
    }

}
