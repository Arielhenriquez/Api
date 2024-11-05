using Api.Application.Common.BaseResponse;
using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Application.Interfaces.Inventory;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryItemController : ControllerBase
{
    private readonly IInventoryItemsService _inventoryItemsService;

    public InventoryItemController(IInventoryItemsService inventoryItemsService)
    {
        _inventoryItemsService = inventoryItemsService;
    }

    [HttpGet("paged")]
    [SwaggerOperation(
      Summary = "Gets Paged Inventory items in the database")]
    public async Task<IActionResult> GetCollaborators([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        var collaborators = await _inventoryItemsService.GetPagedInventoryItems(query, cancellationToken);
        return Ok(BaseResponse.Ok(collaborators));
    }

    [HttpGet]
    [SwaggerOperation(
     Summary = "List Inventory Items in the database")]
    public async Task<IActionResult> GetInventoryItems()
    {
        var inventoryItems = await _inventoryItemsService.GetAllAsync();
        return Ok(BaseResponse.Ok(inventoryItems));
    }

    [HttpGet("search-by-name")]
    [SwaggerOperation(
     Summary = "Get Inventory Items by partial name match")]
    public async Task<IActionResult> GetInventoryItemByName([FromQuery] string name)
    {
        var inventoryItems = await _inventoryItemsService.FindInventoryItemByName(name);
        return Ok(BaseResponse.Ok(inventoryItems));
    }


    [HttpPost]
    [SwaggerOperation(
       Summary = "Creates a new inventory item")]
    public async Task<IActionResult> AddInventoryItem([FromBody] InventoryItemRequestDto inventoryItemDto, CancellationToken cancellationToken)
    {
        var result = await _inventoryItemsService.AddAsync(inventoryItemDto, cancellationToken);
        return CreatedAtRoute(new { id = result.Id }, BaseResponse.Created(result));
    }


    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Updates existing Inventory item")]
    public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] InventoryItemRequestDto request, CancellationToken cancellationToken = default)
    {
        await _inventoryItemsService.UpdateAsync(id, request, cancellationToken);
        return Ok(BaseResponse.Updated(request));
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Deletes an inventory item")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _inventoryItemsService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
