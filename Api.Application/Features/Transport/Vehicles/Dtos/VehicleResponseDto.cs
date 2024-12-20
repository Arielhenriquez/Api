using Api.Domain.Entities.TransportEntities;
using Api.Domain.Enums;

namespace Api.Application.Features.Transport.Vehicles.Dtos;

public class VehicleResponseDto
{
    public Guid Id { get; set; }
    public string? Type { get; set; }
    public int Capacity { get; set; }
    public VehicleStatus Status { get; set; }
    public string? VehicleStatusDescription { get; set; }
    public required string Brand { get; set; }
    public required string Model { get; set; }
    public required string LicensePlate { get; set; }
    public required DateTime InsuranceValidity { get; set; }
    public required string InsuranceType { get; set; }
    public required string Color { get; set; }
    public required string Chassis { get; set; }
    public int Year { get; set; }
    public string? DeleteComment { get; set; } 

     public static implicit operator VehicleResponseDto(Vehicle vehicle)
     {
        return vehicle is null
            ? null
            : new VehicleResponseDto
            {
                Id = vehicle.Id,
                Type = vehicle.Type,
                Capacity = vehicle.Capacity,
                Status = vehicle.Status,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                LicensePlate = vehicle.LicensePlate,
                InsuranceValidity = vehicle.InsuranceValidity,
                InsuranceType = vehicle.InsuranceType,
                Color = vehicle.Color,
                Chassis = vehicle.Chassis,
                Year = vehicle.Year,
                DeleteComment = vehicle.DeleteComment,
            };
     }
}
