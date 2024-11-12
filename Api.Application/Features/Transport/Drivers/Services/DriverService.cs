using Api.Application.Common;
using Api.Application.Common.Exceptions;
using Api.Application.Common.Extensions;
using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Drivers.Dtos;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Transport;
using Api.Domain.Constants;
using Api.Domain.Entities.TransportEntities;

namespace Api.Application.Features.Transport.Drivers.Services;

public class DriverService : BaseService<Driver, DriverRequestDto, DriverResponseDto>, IDriverService
{
    private readonly IDriverRepository _driverRepository;
    private readonly IEmailService _emailService;
    public DriverService(IBaseRepository<Driver> repository, 
        IDriverRepository driverRepository, IEmailService emailService) : base(repository)
    {
        _driverRepository = driverRepository;
        _emailService = emailService;
    }

    public async override Task<DriverResponseDto> AddAsync(DriverRequestDto dto, CancellationToken cancellationToken = default)
    {
        var response = await base.AddAsync(dto, cancellationToken);
        string htmlFile = FileExtensions.ReadEmailTemplate(EmailConstants.CreateDriverTemplate, EmailConstants.TemplateEmailRoute);
        htmlFile = htmlFile.Replace("{{UserName}}", dto.Name);
        await _emailService.SendEmail("supervisorEmail@gmail.com", "Te habla lebron james", htmlFile);

        return response;
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

    public async Task<Paged<DriverResponseDto>> GetPagedDrivers(PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        var result = await _driverRepository.SearchAsync(paginationQuery, cancellationToken);
        foreach (var item in result.Items)
        {
            item.DriveStatusDescription = item.Status.DisplayName();
        }
        return result;
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
