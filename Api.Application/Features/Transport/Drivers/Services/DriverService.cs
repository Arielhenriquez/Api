using Api.Application.Common;
using Api.Application.Common.Exceptions;
using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Drivers.Dtos;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Transport;
using Api.Domain.Entities.TransportEntities;

namespace Api.Application.Features.Transport.Drivers.Services;

public class DriverService : BaseService<Driver, DriverRequestDto, DriverResponseDto>, IDriverService
{
    private readonly IDriverRepository _driverRepository;
    public DriverService(IBaseRepository<Driver> repository, IDriverRepository driverRepository) : base(repository)
    {
        _driverRepository = driverRepository;
    }

    public async Task<List<DriverResponseDto>> FindDriversByName(string criteria)
    {
        var drivers = await _driverRepository.GetByName(criteria);

        if (drivers == null || drivers.Count == 0)
        {
            throw new NotFoundException($"No inventory Items found with name containing: {criteria}");
        }

        return drivers;
    }

    public Task<Paged<DriverResponseDto>> GetPagedDrivers(PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        return _driverRepository.SearchAsync(paginationQuery, cancellationToken);
    }


    protected override DriverResponseDto MapToDto(Driver entity)
    {
        return new DriverResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Status = entity.Status,
            LicenseExpiration = entity.LicenseExpiration,
            PhoneNumber = entity.PhoneNumber
        };
    }

    protected override Driver MapToEntity(DriverRequestDto dto)
    {
        return new Driver
        {
            Id = dto.Id,
            Name = dto.Name,
            Status = dto.Status,
            LicenseExpiration = dto.LicenseExpiration,
            PhoneNumber = dto.PhoneNumber
        };
    }

    protected override void UpdateEntity(Driver entity, DriverRequestDto dto)
    {
        entity.Name = dto.Name;
        entity.Status = dto.Status;
        entity.LicenseExpiration = dto.LicenseExpiration;
        entity.PhoneNumber = dto.PhoneNumber;
    }
}
