using Api.Application.Common.BaseResponse;
using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Vehicles.Dtos;
using Api.Application.Interfaces.Transport;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehicleController(IVehicleService vehicleService) =>
        _vehicleService = vehicleService;


    [HttpGet("paged")]
    [SwaggerOperation(
         Summary = "Gets Paged vehicles in the database")]
    public async Task<IActionResult> GetPagedDrivers([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        var pagedVehicles = await _vehicleService.GetPagedVehicles(query, cancellationToken);
        return Ok(BaseResponse.Ok(pagedVehicles));
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get a single vehicle by id")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id, cancellationToken);
        return Ok(BaseResponse.Ok(vehicle));
    }


    [HttpPost]
    [SwaggerOperation(
       Summary = "Creates a new Vehicle")]
    public async Task<IActionResult> Post([FromBody] VehicleRequestDto vehicleRequestDto, CancellationToken cancellationToken)
    {
        var result = await _vehicleService.AddAsync(vehicleRequestDto, cancellationToken);
        return CreatedAtRoute(new { id = result.Id }, BaseResponse.Created(result));
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Updates an existing Vehicle")]
    public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] VehicleRequestDto request, CancellationToken cancellationToken = default)
    {
        await _vehicleService.UpdateAsync(id, request, cancellationToken);
        return Ok(BaseResponse.Updated(request));
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Deletes a Vehicle")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _vehicleService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
