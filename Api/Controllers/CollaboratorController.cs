using Api.Application.Common.BaseResponse;
using Api.Domain.Entities.InventoryEntities;
using Api.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CollaboratorController : ControllerBase
{
    protected readonly IDbContext _context;
    protected readonly DbSet<Collaborator> _db;

    public CollaboratorController(IDbContext context)
    {
        _context = context;
        _db = context.Set<Collaborator>();
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets Collaborators in the database")]
    public async Task<IActionResult> GetCollaborators()
    {
        var collaborators = await _db
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();

        return Ok(BaseResponse.Ok(collaborators));
    }

    [HttpGet("search-by-name")]
    [SwaggerOperation(
     Summary = "Get Collaborators by partial name match")]
    public async Task<IActionResult> GetCollaboratorByName([FromQuery] string name)
    {
        var collaborators = await _db
            .Where(c => EF.Functions.Like(c.Name, $"%{name}%"))
            .ToListAsync();

        if (collaborators == null || collaborators.Count == 0)
        {
            return NotFound(BaseResponse.NotFound($"No collaborators found with name containing '{name}'"));
        }

        return Ok(BaseResponse.Ok(collaborators));
    }
}
