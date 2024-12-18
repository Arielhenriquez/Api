using Api.Application.Common.BaseResponse;
using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Vehicles.Dtos;
using Api.Application.Interfaces.Transport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Sudo.All, AdminDeAreaTrans.ReadWrite")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehicleController(IVehicleService vehicleService) =>
        _vehicleService = vehicleService;


    [HttpGet("paged")]
    [SwaggerOperation(
         Summary = "Gets Paged vehicles in the database")]
    public async Task<IActionResult> GetPagedDrivers([FromQuery] PaginationQuery query, [FromQuery] bool isDeleted, CancellationToken cancellationToken)
    {
        var pagedVehicles = await _vehicleService.GetPagedVehicles(query, isDeleted, cancellationToken);
        return Ok(BaseResponse.Ok(pagedVehicles));
    }
    [HttpGet]
    [SwaggerOperation(
        Summary = "List vehicles in the database")]
    public async Task<IActionResult> ListVehicles()
    {
        var vehicle = await _vehicleService.GetAllAsync();
        return Ok(BaseResponse.Ok(vehicle));
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
        Summary = "Deletes a Vehicle and logs a comment explaining the reason for deletion",
        Description = "Deletes a Vehicle resource identified by its ID and associates a provided comment as the reason for the deletion. The comment is logged for audit purposes."
        )]

    public async Task<IActionResult> DeleteWithComment([FromRoute] Guid id, [FromQuery] string comment, CancellationToken cancellationToken)
    {
        var deletedDriver = await _vehicleService.DeleteWithComment(id, comment, cancellationToken);
        return Ok(BaseResponse.Ok(deletedDriver));
    }
}
