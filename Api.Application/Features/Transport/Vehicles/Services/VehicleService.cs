using Api.Application.Common;
using Api.Application.Common.Extensions;
using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Vehicles.Dtos;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Transport;
using Api.Domain.Constants;
using Api.Domain.Entities.TransportEntities;

namespace Api.Application.Features.Transport.Vehicles.Services;

public class VehicleService : BaseService<Vehicle, VehicleRequestDto, VehicleResponseDto>, IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IEmailService _emailService;
    public VehicleService(IBaseRepository<Vehicle> repository,
        IVehicleRepository vehicleRepository, IEmailService emailService) : base(repository)
    {
        _vehicleRepository = vehicleRepository;
        _emailService = emailService;
    }

    public async override Task<VehicleResponseDto> AddAsync(VehicleRequestDto dto, CancellationToken cancellationToken = default)
    {
        var response = await base.AddAsync(dto, cancellationToken);
        string htmlFile = FileExtensions.ReadEmailTemplate(EmailConstants.CreateDriverTemplate, EmailConstants.TemplateEmailRoute);
        htmlFile = htmlFile.Replace("{{UserName}}", dto.Model);
        await _emailService.SendEmail("supervisorEmail@gmail.com", "Te habla lebron james", htmlFile);

        return response; 
    }

    public async Task<Paged<VehicleResponseDto>> GetPagedVehicles(PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        var result = await _vehicleRepository.SearchAsync(paginationQuery, cancellationToken);
        foreach (var item in result.Items)
        {
            item.VehicleStatusDescription = item.Status.DisplayName();
        }
        return result;
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
        entity.Type = dto.Type;
        entity.Capacity = dto.Capacity;
        entity.Status = dto.Status;
        entity.Brand = dto.Brand;
        entity.Model = dto.Model;
        entity.LicensePlate = dto.LicensePlate;
        entity.InsuranceValidity = dto.InsuranceValidity;
        entity.InsuranceType = dto.InsuranceType;
    }
}