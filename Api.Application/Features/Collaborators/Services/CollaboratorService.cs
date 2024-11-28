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

    public async Task<List<RolesResponseDto>> GetAllRoles()
    {
        var appRoles = await _graphProvider.GetAppRoles();

        var rolesDto = appRoles
            .Select(role => (RolesResponseDto)role)
            .ToList();

        return rolesDto ?? [];
    }

    //Todo add service principal from settings
    public async Task<List<AppRoleDto?>> GetAppRoles(string userId)
    {
        var servicePrincipalId = Guid.Parse("96b41073-8601-4827-be54-d52994080767");

        var assignedRoles = await _graphProvider.GetAppRolesAssignedToUser(userId, servicePrincipalId);

        return assignedRoles;
    }

    //Add Response Dto
    public async Task<AppRoleAssignment> AddPermissionToUser(AssignRoleToUserDto command, CancellationToken cancellationToken)
    {
        return await _graphProvider.AddPermissionToUser(command, cancellationToken);
    }

    //Add Response Dto
    public async Task<AppRoleAssignment> DeleteRoleUser(DeleteRoleFromUserDto assignRoleToUserDto, CancellationToken cancellationToken)
    {
        return await _graphProvider.DeletePermission(assignRoleToUserDto, cancellationToken);
    }

    //Todo add service principal from settings, add response dto
    public async Task<AppRoleAssignmentCollectionResponse> GetAppRolesAssignments(string userId)
    {
        var servicePrincipalId = Guid.Parse("96b41073-8601-4827-be54-d52994080767");

        return await _graphProvider.GetAppRolesAssignments(userId, servicePrincipalId);
    }
}
