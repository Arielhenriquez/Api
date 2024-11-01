namespace Api.Application.Features.Collaborators.Dtos;

public class CollaboratorResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Supervisor { get; set; }
    public required string Deparment { get; set; }
}
