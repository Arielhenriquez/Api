namespace Api.Application.Features.Collaborators.Dtos;

public class AssignRoleToUserDto
{
    public required string UserId { get; set; }
    //public ActionsTypes? Type { get; set; }
    public required string RoleId { get; set; }
   // public required string AppRoleAssignmentId { get; set; }
}

public class DeleteRoleFromUserDto
{
    public required string UserId { get; set; }
    //public ActionsTypes? Type { get; set; }
    public required string RoleId { get; set; }
    public required string AppRoleAssignmentId { get; set; }
}
