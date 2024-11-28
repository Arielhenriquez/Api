using Microsoft.Graph.Models;

namespace Api.Application.Features.Collaborators.Dtos.GraphDtos;

public class RolesResponseDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public static implicit operator RolesResponseDto(AppRole role)
    {
        return role is null ?
            null :
            new RolesResponseDto
            {
                Id = role.Id.ToString(),
                Name = role.DisplayName,
                Description = role.Description,
            };
    }
}
