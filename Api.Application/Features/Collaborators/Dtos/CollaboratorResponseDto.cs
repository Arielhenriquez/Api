using Api.Domain.Entities.InventoryEntities;

namespace Api.Application.Features.Collaborators.Dtos;

public class CollaboratorResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Supervisor { get; set; }
    public required string Deparment { get; set; }

    public static implicit operator CollaboratorResponseDto(Collaborator collaborator)
    {
        return collaborator is null ?
            null :
            new CollaboratorResponseDto
            {
                Id = collaborator.Id,
                Name = collaborator.Name,
                Deparment = collaborator.Department,
                Supervisor = collaborator.Supervisor
            };
    }
}
