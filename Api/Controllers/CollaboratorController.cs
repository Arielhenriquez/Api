using Api.Application.Common.BaseResponse;
using Api.Application.Common.Pagination;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Collaborators;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CollaboratorController : ControllerBase
{
    private readonly ICollaboratorService _collaboratorService;
    private readonly IEmailService _emailService;

    public CollaboratorController(ICollaboratorService collaboratorService, IEmailService emailService)
    {
        _collaboratorService = collaboratorService;
        _emailService = emailService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets paged Collaborators in the database")]
    public async Task<IActionResult> GetCollaborators([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        var collaborators = await _collaboratorService.GetPagedCollaborators(query, cancellationToken);
        await _emailService.SendTestEmail();
        return Ok(BaseResponse.Ok(collaborators));
    }

    [HttpGet("search-by-name")]
    [SwaggerOperation(
        Summary = "Get Collaborators by name")]
    public async Task<IActionResult> GetCollaboratorByName([FromQuery] string name)
    {
        var collaborators = await _collaboratorService.FindCollaboratorByName(name);
        return Ok(BaseResponse.Ok(collaborators));
    }
}
