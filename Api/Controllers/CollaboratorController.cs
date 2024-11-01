using Api.Application.Common.BaseResponse;
using Api.Application.Common.Pagination;
using Api.Application.Interfaces.Collaborators;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CollaboratorController : ControllerBase
{
    private readonly ICollaboratorService _collaboratorService;

    public CollaboratorController(ICollaboratorService collaboratorService)
    {
        _collaboratorService = collaboratorService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets Collaborators in the database")]
    public async Task<IActionResult> GetCollaborators([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        var collaborators = await _collaboratorService.GetPagedCollaborators(query, cancellationToken);
        return Ok(BaseResponse.Ok(collaborators));
    }

    [HttpGet("search-by-name")]
    [SwaggerOperation(
        Summary = "Get Collaborators by partial name match")]
    public async Task<IActionResult> GetCollaboratorByName([FromQuery] string name)
    {
        var collaborators = await _collaboratorService.FindCollaboratorByName(name);
        return Ok(BaseResponse.Ok(collaborators));
    }
}
