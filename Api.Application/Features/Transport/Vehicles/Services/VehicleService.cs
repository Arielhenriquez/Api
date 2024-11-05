using Api.Application.Common;
using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Vehicles.Dtos;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Transport;
using Api.Domain.Entities.TransportEntities;

namespace Api.Application.Features.Transport.Vehicles.Services;

public class VehicleService : BaseService<Vehicle, VehicleRequestDto, VehicleResponseDto>, IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    public VehicleService(IBaseRepository<Vehicle> repository, IVehicleRepository vehicleRepository) : base(repository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public Task<Paged<VehicleResponseDto>> GetPagedVehicles(PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        return _vehicleRepository.SearchAsync(paginationQuery, cancellationToken);
    }

    protected override VehicleResponseDto MapToDto(Vehicle entity)
    {
        return new VehicleResponseDto
        {
            Id = entity.Id,
            Type = entity.Type,
            Capacity = entity.Capacity,
            Status = entity.Status,
            Brand = entity.Brand,
            Model = entity.Model,
            LicensePlate = entity.LicensePlate,
            InsuranceType = entity.InsuranceType,
            InsuranceValidity = entity.InsuranceValidity,
        };
    }

    protected override Vehicle MapToEntity(VehicleRequestDto dto)
    {
        return new Vehicle
        {
            Id = dto.Id,
            Type = dto.Type,
            Capacity = dto.Capacity,
            Status = dto.Status,
            Brand = dto.Brand,
            Model = dto.Model,
            LicensePlate = dto.LicensePlate,
            InsuranceType = dto.InsuranceType,
            InsuranceValidity = dto.InsuranceValidity,
        };
    }

    protected override void UpdateEntity(Vehicle entity, VehicleRequestDto dto)
    {
        entity.Type = entity.Type;
        entity.Capacity = entity.Capacity;
        entity.Status = dto.Status;
        entity.Brand = dto.Brand;
        entity.Model = dto.Model;
        entity.LicensePlate = dto.LicensePlate;
        entity.InsuranceValidity = dto.InsuranceValidity;
        entity.InsuranceType = dto.InsuranceType;
    }
}