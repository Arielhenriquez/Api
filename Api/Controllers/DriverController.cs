using Api.Application.Common.BaseResponse;
using Api.Application.Interfaces;
using Api.Domain.Entities.TransportEntities;
using Api.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]

//Todo: refactor this
public class DriverController : ControllerBase
{
    protected readonly IDbContext _context;
    protected readonly DbSet<Driver> _db;
    private readonly IEmailService _emailService;

    public DriverController(IDbContext context, IEmailService emailService)
    {
        _context = context;
        _db = context.Set<Driver>();
        _emailService = emailService;
    }

    [HttpGet]
    [SwaggerOperation(
     Summary = "Gets drivers in the database")]
    public async Task<IActionResult> GetDrivers()
    {
        var oli = _db.AsQueryable();
        return Ok(BaseResponse.Ok(oli));
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a new driver")]
    public async Task<IActionResult> AddDriver([FromBody] DriverDto driver)
    {
        Driver driver1 = new() { Name = driver.Name, LicenseExpiration = driver.LicenseExpiration, State = driver.State, PhoneNumber = driver.PhoneNumber };

        var oli = await _db.AddAsync(driver1);
        await _context.SaveChangesAsync();
        await _emailService.SendEmail("manoloemail@gmail.com", "Driver", driver.Name);
        return Ok(BaseResponse.Ok(driver1));
    }

    public class DriverDto
    {
        public required string Name { get; set; }
        public string? State { get; set; }
        public required DateTime LicenseExpiration { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
