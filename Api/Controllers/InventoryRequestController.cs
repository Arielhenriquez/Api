using Api.Application.Common.BaseResponse;
using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryRequest.Dtos;
using Api.Application.Interfaces.Inventory;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryRequestController : ControllerBase
{
    private readonly IInventoryRequestService _inventoryRequestService;
    public InventoryRequestController(IInventoryRequestService inventoryRequestService) =>
        _inventoryRequestService = inventoryRequestService;


    [HttpGet]
    [SwaggerOperation(
        Summary = "List Inventory Requests")]
    public async Task<IActionResult> ListInventoryRequests()
    {
        var inventoryRequests = await _inventoryRequestService.ListInventoryRequests();
        return Ok(BaseResponse.Ok(inventoryRequests));
    }


    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "List Inventory Requests details")]
    public async Task<IActionResult> GetInventoryRequestDetails([FromQuery] PaginationQuery paginationQuery, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var inventoryRequests = await _inventoryRequestService.GetInventoryRequestDetails(paginationQuery, id, cancellationToken);
        return Ok(BaseResponse.Ok(inventoryRequests));
    }


    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a Inventory Request")]
    public async Task<IActionResult> AddInventoryRequest([FromBody] InventoryRequestDto inventoryRequestDto, CancellationToken cancellationToken)
    {
        await _inventoryRequestService.AddInventoryRequest(inventoryRequestDto, cancellationToken);
        return Ok();
        // return CreatedAtRoute(new { id = result.Id }, BaseResponse.Created(result));
    }

}
