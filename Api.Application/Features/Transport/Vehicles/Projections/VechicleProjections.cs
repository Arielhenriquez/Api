using Api.Application.Features.Transport.Vehicles.Dtos;
using Api.Domain.Entities.TransportEntities;
using System.Linq.Expressions;

namespace Api.Application.Features.Transport.Vehicles.Projections;

public static class VechicleProjections
{
    public static Expression<Func<Vehicle, VehicleResponseDto>> Search => (Vehicle vehicle) => new VehicleResponseDto()
    {
        Id = vehicle.Id,
        Type = vehicle.Type,
        Capacity = vehicle.Capacity,
        Status = vehicle.Status,
        Brand = vehicle.Brand,
        Model = vehicle.Model,
        LicensePlate = vehicle.LicensePlate,
        InsuranceType = vehicle.InsuranceType,
        InsuranceValidity = vehicle.InsuranceValidity,
        Chassis = vehicle.Chassis,
        Color = vehicle.Color,
        Year = vehicle.Year,
        DeleteComment = vehicle.DeleteComment,
    };
}
