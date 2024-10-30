using Api.Application.Common.BaseResponse;
using Api.Domain.Entities.InventoryEntities;
using Api.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryItemController : ControllerBase
{
    protected readonly IDbContext _context;
    protected readonly DbSet<InventoryItem> _db;

    public InventoryItemController(IDbContext context)
    {
        _context = context;
        _db = context.Set<InventoryItem>();
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets Inventory Items in the database")]
    public async Task<IActionResult> GetInventoryItems()
    {
        var collaborators = await _db
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();

        return Ok(BaseResponse.Ok(collaborators));
    }

    [HttpGet("search-by-name")]
    [SwaggerOperation(
     Summary = "Get InventoryItems by partial name match")]
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


    [HttpPost]
    [SwaggerOperation(
       Summary = "Creates a new inventory item")]
    public async Task<IActionResult> AddInventoryItem([FromBody] InventoryItemDto inventoryItemDto)
    {
        InventoryItem inventoryItem = new()
        {
            Name = inventoryItemDto.Name,
            Quantity = inventoryItemDto.Quantity,
            UnitOfMeasure = inventoryItemDto.UnitOfMeasure,
        };

        await _db.AddAsync(inventoryItem);
        await _context.SaveChangesAsync();
        return Ok(BaseResponse.Ok(inventoryItem));
    }

    public class InventoryItemDto
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public required int Quantity { get; set; }
        public string? UnitOfMeasure { get; set; }
    }


    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Deletes an inventory item")]

    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var entity = await _db.AsQueryable().Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();

        if (entity is null) return NotFound(BaseResponse.NotFound($"No collaborators found with id containing '{id}'"));

        _db.Remove(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
