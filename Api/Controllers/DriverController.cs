﻿using Api.Application.Common.BaseResponse;
using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Drivers.Dtos;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Transport;
using Api.Domain.Entities.TransportEntities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class DriverController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly IDriverService _driverService;

    public DriverController(IEmailService emailService, IDriverService driverService)
    {
        _emailService = emailService;
        _driverService = driverService;
    }
    [HttpGet("paged")]
    [SwaggerOperation(
         Summary = "Gets Paged Drivers in the database")]
    public async Task<IActionResult> GetPagedDrivers([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        var collaborators = await _driverService.GetPagedDrivers(query, cancellationToken);
        return Ok(BaseResponse.Ok(collaborators));
    }


    [HttpGet("search-by-name")]
    [SwaggerOperation(
     Summary = "Get Drivers by partial name match")]
    public async Task<IActionResult> GetDriversByName([FromQuery] string name)
    {
        var inventoryItems = await _driverService.FindDriversByName(name);
        return Ok(BaseResponse.Ok(inventoryItems));
    }


    [HttpPost]
    [SwaggerOperation(
       Summary = "Creates a new Driver")]
    public async Task<IActionResult> AddDriver([FromBody] DriverRequestDto driverRequestDto, CancellationToken cancellationToken)
    {
        var result = await _driverService.AddAsync(driverRequestDto, cancellationToken);
        await _emailService.SendEmail("manoloemail@gmail.com", "Driver", result.Name);
        return CreatedAtRoute(new { id = result.Id }, BaseResponse.Created(result));
    }


    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Updates an existing Driver")]
    public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] DriverRequestDto request, CancellationToken cancellationToken = default)
    {
        await _driverService.UpdateAsync(id, request, cancellationToken);
        return Ok(BaseResponse.Updated(request));
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Deletes a Driver")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _driverService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
