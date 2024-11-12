using Api.Domain.Enums;
using System.Text.Json.Serialization;

namespace Api.Application.Features.Transport.Vehicles.Dtos;

public class VehicleRequestDto
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string? Type { get; set; }
    public int Capacity { get; set; }
    public VehicleStatus Status { get; set; }
    public required string Brand { get; set; }
    public required string Model { get; set; }
    public required string LicensePlate { get; set; }
    public required DateTime InsuranceValidity { get; set; }
    public required string InsuranceType { get; set; }
}
