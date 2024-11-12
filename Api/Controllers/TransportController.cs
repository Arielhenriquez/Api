﻿using Api.Application.Common.BaseResponse;
using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.TransportRequest.Dtos;
using Api.Application.Interfaces.Transport;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransportRequestController : ControllerBase
{
    private readonly ITransportService _transportService;

    public TransportRequestController(ITransportService transportService) => _transportService = transportService;

    [HttpGet("paged")]
    [SwaggerOperation(
        Summary = "Get paged Transport Requests")]
    public async Task<IActionResult> GetPagedTransportRequests([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        var inventoryRequests = await _transportService.GetPagedTransportRequests(paginationQuery, cancellationToken);
        return Ok(BaseResponse.Ok(inventoryRequests));
    }


    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get Transport Request details")]
    public async Task<IActionResult> GetTransportRequestDetails([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var inventoryRequests = await _transportService.GetTransportRequestDetails(id, cancellationToken);
        return Ok(BaseResponse.Ok(inventoryRequests));
    }

    [HttpPost]
    [SwaggerOperation(
       Summary = "Creates a Transport Request")]
    public async Task<IActionResult> AddInventoryRequest([FromBody] TransportRequestDto transportRequestDto, CancellationToken cancellationToken)
    {
        var result = await _transportService.AddTransportRequest(transportRequestDto, cancellationToken);
        return CreatedAtRoute(new { id = result.Id }, BaseResponse.Created(result));
    }
}
