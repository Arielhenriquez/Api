using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Drivers.Dtos;

namespace Api.Application.Interfaces.Transport;

public interface IDriverService : IBaseService<DriverRequestDto, DriverResponseDto>
{
    Task<Paged<DriverResponseDto>> GetPagedDrivers(PaginationQuery paginationQuery, CancellationToken cancellationToken);
    Task<List<DriverResponseDto>> FindDriversByName(string criteria);
}
