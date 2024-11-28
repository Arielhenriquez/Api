namespace Api.Application.Features.Collaborators.Dtos.GraphDtos;

public class AssignRolesToUserDto
{
    public required string UserId { get; set; }
    public required string[] RoleIds { get; set; }
}

public class DeleteRoleFromUserDto
{
    public required string UserId { get; set; }
    public required string[] AppRoleAssignmentIds { get; set; }
}

public class RoleAssignmentResultDto
{
    public required Guid RoleId { get; set; }
    public required Guid UserId { get; set; }
    public required string Message { get; set; }
}