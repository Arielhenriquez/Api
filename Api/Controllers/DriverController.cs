using Api.Application.Common.BaseResponse;
using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Drivers.Dtos;
using Api.Application.Features.Transport.Vehicles.Services;
using Api.Application.Interfaces.Transport;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class DriverController : ControllerBase
{
    private readonly IDriverService _driverService;
    public DriverController(IDriverService driverService) => _driverService = driverService;
    
    [HttpGet("paged")]
    [SwaggerOperation(
         Summary = "Gets Paged Drivers in the database")]
    public async Task<IActionResult> GetPagedDrivers([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        var drivers = await _driverService.GetPagedDrivers(query, cancellationToken);
        return Ok(BaseResponse.Ok(drivers));
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "List Drivers in the database")]
    public async Task<IActionResult> ListDrivers()
    {
        var drivers = await _driverService.GetAllAsync();
        return Ok(BaseResponse.Ok(drivers));
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get a single driver by id")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var driver = await _driverService.GetByIdAsync(id, cancellationToken);
        return Ok(BaseResponse.Ok(driver));
    }


    [HttpGet("search-by-name")]
    [SwaggerOperation(
     Summary = "Get Drivers by partial name match")]
    public async Task<IActionResult> GetDriversByName([FromQuery] string? name)
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
