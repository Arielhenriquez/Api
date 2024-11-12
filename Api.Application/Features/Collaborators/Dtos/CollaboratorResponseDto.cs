using Api.Domain.Entities;
using Api.Domain.Enums;

namespace Api.Application.Features.Collaborators.Dtos;

public class CollaboratorResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Supervisor { get; set; }
    public required string Deparment { get; set; }
    public UserRoles Roles { get; set; } = UserRoles.Applicant;

    public static implicit operator CollaboratorResponseDto(Collaborator collaborator)
    {
        return collaborator is null ?
            null :
            new CollaboratorResponseDto
            {
                Id = collaborator.Id,
                Name = collaborator.Name,
                Deparment = collaborator.Department,
                Supervisor = collaborator.Supervisor,
                Roles = UserRoles.Applicant
            };
    }
}
