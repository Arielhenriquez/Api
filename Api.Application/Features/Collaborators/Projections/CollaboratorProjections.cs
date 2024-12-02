using System.Linq.Expressions;
using Api.Application.Common.Extensions;
using Api.Application.Features.Collaborators.Dtos;
using Api.Domain.Entities;
using Api.Domain.Enums;

namespace Api.Application.Features.Collaborators.Projections;

public static class CollaboratorProjections
{
    public static Expression<Func<Collaborator, CollaboratorResponseDto>> Search => (Collaborator collaborator) => new CollaboratorResponseDto
    {
        Id = collaborator.Id,
        UserOid = collaborator.UserOid,
        Name = collaborator.Name,
        Supervisor = collaborator.Supervisor,
        Department = collaborator.Department,

        Roles = new List<UserRoles>(),
        RolesDescriptions = collaborator.Roles
    };

    public static CollaboratorResponseDto ToResponseDto(Collaborator collaborator)
    {
        var azureRoles = collaborator.Roles
            .Select(EnumExtensions.MapDbRoleToEnum)
            .Where(role => role != null)
            .Cast<UserRoles>()
            .ToList();

        return new CollaboratorResponseDto
        {
            Id = collaborator.Id,
            UserOid = collaborator.UserOid,
            Name = collaborator.Name,
            Supervisor = collaborator.Supervisor,
            Department = collaborator.Department,
            Roles = azureRoles,
            RolesDescriptions = azureRoles.Select(role => role.DisplayName()).ToList()
        };
    }
}