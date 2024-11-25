using Api.Application.Common.BaseResponse;
using Api.Application.Common.Pagination;
using Api.Application.Features.Collaborators.Dtos;
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

    //[Authorize]
    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets paged Collaborators in the database")]
    public async Task<IActionResult> GetCollaborators([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        var collaborators = await _collaboratorService.GetPagedCollaborators(query, cancellationToken);
        return Ok(BaseResponse.Ok(collaborators));
    }

    [Authorize]
    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        return Ok("login");
    }

    [HttpGet("search-by-name")]
    [SwaggerOperation(
        Summary = "Get Collaborators by name")]
    public async Task<IActionResult> GetCollaboratorByName([FromQuery] string? name)
    {
        var collaborators = await _collaboratorService.FindCollaboratorByName(name);
        return Ok(BaseResponse.Ok(collaborators));
    }

    [HttpGet("{userOid}")]
    [SwaggerOperation(
    Summary = "Gets Graph User by userOid")]
    public async Task<IActionResult> GetGraphUsers([FromRoute] string userOid)
    {
        var graphUser = await _collaboratorService.GetGraphUsers(userOid);
        return Ok(BaseResponse.Ok(graphUser));
    }

    [HttpGet("user-manager/{userOid}")]
    [SwaggerOperation(
        Summary = "Gets Graph User by manager")]
    public async Task<IActionResult> GetUserManager([FromRoute] string userOid)
    {
        var userManager = await _collaboratorService.GetUserManager(userOid);
        return Ok(BaseResponse.Ok(userManager));
    }

    [HttpGet("roles/{userOid}")]
    [SwaggerOperation(
    Summary = "Gets Graph User Roles")]
    public async Task<IActionResult> GetAppRole([FromRoute] string userOid)
    {
        var userManager = await _collaboratorService.GetAppRoles(userOid);
        return Ok(BaseResponse.Ok(userManager));
    }

    [HttpGet("role/{userOid}")]
    [SwaggerOperation(
        Summary = "Gets All Graph User Roles")]
    public async Task<IActionResult> GetAppRoles([FromRoute] string userOid)
    {
        var userManager = await _collaboratorService.GetAppRole(userOid);
        return Ok(BaseResponse.Ok(userManager));
    }

    [HttpGet("roles-assignment/{userOid}")]
    [SwaggerOperation(
        Summary = "Gets Graph User Roles assignments")]
    public async Task<IActionResult> GetAppRolesAssignments([FromRoute] string userOid)
    {
        var userManager = await _collaboratorService.GetAppRolesAssignments(userOid);
        return Ok(BaseResponse.Ok(userManager));
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Add Roles to Users")]
    public async Task<IActionResult> AddRoleToUser([FromBody] AssignRoleToUserDto assignRoleToUserDto, CancellationToken cancellationToken)
    {
        var userManager = await _collaboratorService.AddPermissionToUser(assignRoleToUserDto, cancellationToken);
        return Ok(BaseResponse.Ok(userManager));
    }

    [HttpDelete]
    [SwaggerOperation(
        Summary = "Delete Roles user")]
    public async Task<IActionResult> DeleteRoleToUser([FromBody] DeleteRoleFromUserDto assignRoleToUserDto, CancellationToken cancellationToken)
    {
        var userManager = await _collaboratorService.DeleteRoleUser(assignRoleToUserDto, cancellationToken);
        return Ok(BaseResponse.Ok(userManager));
    }
}


