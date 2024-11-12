using Api.Domain.Enums;

namespace Api.Application.Features.Collaborators.Dtos;

public class CollaboratorRequestDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Supervisor { get; set; }
    public required string Deparment { get; set; }
    public UserRoles Role { get; set; } = UserRoles.Applicant;
}
