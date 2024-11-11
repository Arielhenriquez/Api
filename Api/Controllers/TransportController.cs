using Api.Application.Common.BaseResponse;
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

    [HttpPost]
    [SwaggerOperation(
       Summary = "Creates a Transport Request")]
    public async Task<IActionResult> AddInventoryRequest([FromBody] TransportRequestDto transportRequestDto, CancellationToken cancellationToken)
    {
        var result = await _transportService.AddTransportRequest(transportRequestDto, cancellationToken);
        return CreatedAtRoute(new { id = result.Id }, BaseResponse.Created(result));
    }
}
