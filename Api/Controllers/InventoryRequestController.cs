﻿using Api.Application.Common.BaseResponse;
using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Application.Features.Inventory.InventoryRequest.Dtos;
using Api.Application.Interfaces.Inventory;
using Microsoft.AspNetCore.Authorization;
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

    //Todo: Get by department.. Este simplemente lo veran los admin
    [Authorize(Roles = "Sudo.All, AdminDeArea.ReadWrite, Supervisor.Approval, Solicitante.ReadWrite")]
    [HttpGet("paged")]
    [SwaggerOperation(
    Summary = "Get paged Inventory Requests")]
    public async Task<IActionResult> GetPagedInventoryRequests([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        var inventoryRequests = await _inventoryRequestService.GetPagedInventoryRequest(paginationQuery, cancellationToken);
        return Ok(BaseResponse.Ok(inventoryRequests));
    }

    [Authorize(Roles = "Sudo.All, AdminDeArea.ReadWrite")]
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get Inventory Request details")]
    public async Task<IActionResult> GetInventoryRequestDetails([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var inventoryRequests = await _inventoryRequestService.GetInventoryRequestDetails(id, cancellationToken);
        return Ok(BaseResponse.Ok(inventoryRequests));
    }

   // [Authorize(Roles = "Sudo.All, AdminDeArea.ReadWrite, Supervisor.Approval, Solicitante.ReadWrite")]
    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates an Inventory Request")]
    public async Task<IActionResult> AddInventoryRequest([FromBody] InventoryRequestDto inventoryRequestDto, CancellationToken cancellationToken)
    {
        var result = await _inventoryRequestService.AddInventoryRequest(inventoryRequestDto, cancellationToken);
        return CreatedAtRoute(new { id = result.Id }, BaseResponse.Created(result));
    }

   // [Authorize(Roles = "Sudo.All, AdminDeArea.ReadWrite, Supervisor.Approval, Admin.Approval")]
    [HttpPatch("approve")]
    [SwaggerOperation(
    Summary = "Approve or reject an Inventory request",
    Description = "Allows supervisors or administrators to approve or reject a transport request. " +
                  "Only requests in a 'Pending' status can be processed. " +
                  "Supervisors or Sudo roles are required to perform this action.")]
    public async Task<IActionResult> ApproveOrRejectRequest([FromBody] ApprovalDto approvalDto, CancellationToken cancellationToken) 
    {
        var result = await _inventoryRequestService.ApproveInventoryRequest(approvalDto, cancellationToken);
        return Ok(BaseResponse.Ok(result));
    }
}
