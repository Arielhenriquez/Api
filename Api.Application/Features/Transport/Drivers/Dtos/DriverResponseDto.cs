﻿using Api.Domain.Enums;

namespace Api.Application.Features.Transport.Drivers.Dtos;

public class DriverResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DriverStatus Status { get; set; }
    public required DateTime LicenseExpiration { get; set; }
    public string? PhoneNumber { get; set; }
}
