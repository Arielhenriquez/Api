using Api.Domain.Entities;
using Api.Domain.Enums;

namespace Api.Application.Features.Collaborators.Dtos;

public class CollaboratorResponseDto
{
    public Guid Id { get; set; }
    public required string UserOid { get; set; }
    public required string Name { get; set; }
    public required string Supervisor { get; set; }
    public required string Deparment { get; set; }
    public UserRoles Roles { get; set; }

    public static implicit operator CollaboratorResponseDto(Collaborator collaborator)
    {
        return collaborator is null ?
            null :
            new CollaboratorResponseDto
            {
                Id = collaborator.Id,
                UserOid = collaborator.UserOid,
                Name = collaborator.Name,
                Deparment = collaborator.Department,
                Supervisor = collaborator.Supervisor,
                Roles = collaborator.Roles
            };
    }
}
