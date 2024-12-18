using Api.Application.Common.BaseResponse;
using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Application.Interfaces.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class InventoryItemController : ControllerBase
{
    private readonly IInventoryItemsService _inventoryItemsService;

    public InventoryItemController(IInventoryItemsService inventoryItemsService) =>
       _inventoryItemsService = inventoryItemsService;

    [Authorize(Roles = "Sudo.All, AdminDeArea.ReadWrite")]
    [HttpGet("paged")]
    [SwaggerOperation(
      Summary = "Gets Paged Inventory items in the database")]
    public async Task<IActionResult> GetInventoryItems([FromQuery] PaginationQuery query, bool isDeleted, CancellationToken cancellationToken)
    {
        var collaborators = await _inventoryItemsService.GetPagedInventoryItems(query, isDeleted, cancellationToken);
        return Ok(BaseResponse.Ok(collaborators));
    }

    [Authorize(Roles = "Sudo.All, AdminDeArea.ReadWrite")]
    [HttpGet]
    [SwaggerOperation(
     Summary = "List Inventory Items in the database")]
    public async Task<IActionResult> GetInventoryItems()
    {
        var inventoryItems = await _inventoryItemsService.GetAllAsync();
        return Ok(BaseResponse.Ok(inventoryItems));
    }

    [Authorize(Roles = "Sudo.All, AdminDeArea.ReadWrite, Solicitante.ReadWrite, Supervisor.Approval")]
    [HttpGet("search-by-name")]
    [SwaggerOperation(
     Summary = "Get Inventory Items by partial name match")]
    public async Task<IActionResult> GetInventoryItemByName([FromQuery] string? name)
    {
        var inventoryItems = await _inventoryItemsService.FindInventoryItemByName(name);
        return Ok(BaseResponse.Ok(inventoryItems));
    }

    [Authorize(Roles = "Sudo.All, AdminDeArea.ReadWrite, Solicitante.ReadWrite, Supervisor.Approval")]
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get a single inventory by id")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _inventoryItemsService.GetByIdAsync(id, cancellationToken);
        return Ok(BaseResponse.Ok(result));
    }

    [Authorize(Roles = "Sudo.All, AdminDeArea.ReadWrite")]
    [HttpPost]
    [SwaggerOperation(
       Summary = "Creates a new inventory item")]
    public async Task<IActionResult> AddInventoryItem([FromBody] InventoryItemRequestDto inventoryItemDto, CancellationToken cancellationToken)
    {
        var result = await _inventoryItemsService.AddAsync(inventoryItemDto, cancellationToken);
        return CreatedAtRoute(new { id = result.Id }, BaseResponse.Created(result));
    }

    [Authorize(Roles = "Sudo.All, AdminDeArea.ReadWrite")]
    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Updates existing Inventory item")]
    public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] InventoryItemRequestDto request, CancellationToken cancellationToken = default)
    {
        await _inventoryItemsService.UpdateAsync(id, request, cancellationToken);
        return Ok(BaseResponse.Updated(request));
    }

    [Authorize(Roles = "Sudo.All, AdminDeArea.ReadWrite, Solicitante.ReadWrite")]
    [HttpPatch("{id}")]
    [SwaggerOperation(
    Summary = "Update the quantity of an inventory item")]
    public async Task<IActionResult> Patch([FromRoute] Guid id, [FromBody] UpdateArticleQuantityDto request, CancellationToken cancellationToken = default)
    {
        await _inventoryItemsService.UpdateArticleQuantity(id, request, cancellationToken);
        return Ok(BaseResponse.Updated(request));
    }

    [Authorize(Roles = "Sudo.All, AdminDeArea.ReadWrite")]
    [HttpDelete("{id}")]
    [SwaggerOperation(
         Summary = "Deletes an inventory item and logs a comment explaining the reason for deletion",
         Description = "Deletes an inventory item resource identified by its ID and associates a provided comment as the reason for the deletion. The comment is logged for audit purposes."
         )]

    public async Task<IActionResult> DeleteWithComment([FromRoute] Guid id, [FromQuery] string comment, CancellationToken cancellationToken)
    {
        var deletedDriver = await _inventoryItemsService.DeleteItemWithComment(id, comment, cancellationToken);
        return Ok(BaseResponse.Ok(deletedDriver));
    }
}
