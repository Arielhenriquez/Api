namespace Api.Application.Features.Collaborators.Dtos.GraphDtos;

public class DeleteRoleFromUserDto
{
    public required string UserId { get; set; }
    public required string[] AppRoleAssignmentIds { get; set; }
}
public class DeleteRoleAssignmentResultDto
{
    public required string RoleId { get; set; }
    public required string UserId { get; set; }
    public required string Message { get; set; }
}