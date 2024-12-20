using Api.Application.Common.BaseResponse;
using Api.Application.Common.Pagination;
using Api.Application.Features.Collaborators.Dtos;
using Api.Application.Features.Collaborators.Dtos.GraphDtos;
using Api.Application.Interfaces.Collaborators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CollaboratorController : ControllerBase
{
    private readonly ICollaboratorService _collaboratorService;
    public CollaboratorController(ICollaboratorService collaboratorService) =>
        _collaboratorService = collaboratorService;

    //Todo hacer endpoint get solicitudes por department no importa cual sea jj
    [Authorize(Roles = "Sudo.All")]
    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets paged Collaborators in the database")]
    public async Task<IActionResult> GetCollaborators([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        var collaborators = await _collaboratorService.GetPagedCollaborators(query, cancellationToken);
        return Ok(BaseResponse.Ok(collaborators));
    }

    [Authorize]
    [HttpGet("search-by-email")]
    [SwaggerOperation(
        Summary = "Get Collaborators by email")]
    public async Task<IActionResult> GetCollaboratorByEmail([FromQuery] string? name)
    {
        var collaborators = await _collaboratorService.FindCollaboratorByEmail(name);
        return Ok(BaseResponse.Ok(collaborators));
    }

    [Authorize(Roles = "Sudo.All")]
    [HttpGet("roles")]
    [SwaggerOperation(
        Summary = "Gets All Graph User Roles")]
    public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
    {
        var roles = await _collaboratorService.GetAllRoles(cancellationToken);
        return Ok(BaseResponse.Ok(roles));
    }

    [Authorize(Roles = "Sudo.All")]
    //Todo: Add validation of userOid
    [SwaggerOperation(
        Summary = "Gets user roles (assigned or unassigned) based on the isAssigned flag.")]
    [HttpGet("role-assignments/{userOid}")]
    public async Task<IActionResult> GetUserRoles(string userOid, [FromQuery] bool isAssigned, CancellationToken cancellationToken)
    {
        var rolesAssignments = await _collaboratorService.GetUserRoleAssignments(userOid, isAssigned, cancellationToken);
        return Ok(BaseResponse.Ok(rolesAssignments));
    }

    [Authorize(Roles = "Sudo.All")]
    [HttpPost("roles")]
    [SwaggerOperation(
        Summary = "Add Roles to Users")]
    public async Task<IActionResult> AddRolesToUser([FromBody] AssignRolesToUserDto assignRoleToUserDto, CancellationToken cancellationToken)
    {
        var addedRoles = await _collaboratorService.AddRolesToUser(assignRoleToUserDto, cancellationToken);
        return Ok(BaseResponse.Ok(addedRoles));
    }

    [Authorize(Roles = "Sudo.All")]
    [HttpPut("roles")]
    [SwaggerOperation(
    Summary = "Add Roles to Users")]
    public async Task<IActionResult> UpdateUserRoles([FromBody] CollaboratorRequest collaborator, CancellationToken cancellationToken)
    {
        await _collaboratorService.UpdateRoles(collaborator);
        return NoContent();
    }

    [Authorize(Roles = "Sudo.All")]
    [HttpDelete("roles")]
    [SwaggerOperation(
        Summary = "Delete Roles from user")]
    public async Task<IActionResult> DeleteRolesFromUser([FromBody] DeleteRoleFromUserDto assignRoleToUserDto, CancellationToken cancellationToken)
    {
        var deletedRoles = await _collaboratorService.DeleteRolesFromUser(assignRoleToUserDto, cancellationToken);
        return Ok(BaseResponse.Ok(deletedRoles));
    }
}


