using Api.Application.Common.Extensions;
using Api.Domain.Entities;
using Api.Domain.Enums;

namespace Api.Application.Features.Collaborators.Dtos;

public class CollaboratorResponseDto
{
    public Guid Id { get; set; }
    public required string UserOid { get; set; }
    public required string Name { get; set; }
    public required string Supervisor { get; set; }
    public required string Department { get; set; }

    public List<UserRoles> Roles { get; set; } = []; 
    public List<string> RolesDescriptions { get; set; } = []; 

    public static implicit operator CollaboratorResponseDto(Collaborator collaborator)
    {
        if (collaborator == null)
        {
            return null;
        }

        var appRoles = collaborator.Roles
            .Select(role => Enum.TryParse<UserRoles>(role, out var parsedRole) ? parsedRole : (UserRoles?)null)
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
            Roles = appRoles,
            RolesDescriptions = appRoles.Select(role => role.DisplayName()).ToList()
        };
    }
}

public class CollaboratorRequest 
{
    public Guid Id { get; set; }
    public required string UserOid { get; set; }
    public required string Name { get; set; }
    public required string Supervisor { get; set; }
    public required string Department { get; set; }
    public List<UserRoles> Roles { get; set; } = [];
    public List<string> RolesDescriptions { get; set; } = [];

    public static implicit operator CollaboratorRequest(Collaborator collaborator) 
    {
        var appRoles = collaborator.Roles
           .Select(role => Enum.TryParse<UserRoles>(role, out var parsedRole) ? parsedRole : (UserRoles?)null)
           .Where(role => role != null)
           .Cast<UserRoles>()
           .ToList();

        return new CollaboratorRequest
        {
            Id = collaborator.Id,
            UserOid = collaborator.UserOid,
            Name = collaborator.Name,
            Supervisor = collaborator.Supervisor,
            Department = collaborator.Department,
            Roles = appRoles,
            RolesDescriptions = appRoles.Select(role => role.DisplayName()).ToList()
        };
    }
}