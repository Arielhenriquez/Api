using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Drivers.Dtos;

namespace Api.Application.Interfaces.Transport;

public interface IDriverService : IBaseService<DriverRequestDto, DriverResponseDto>
{
    Task<Paged<DriverResponseDto>> GetPagedDrivers(PaginationQuery paginationQuery, bool isDeleted, CancellationToken cancellationToken);
    Task<List<DriverResponseDto>> FindDriversByName(string criteria);
    Task<IEnumerable<DriverSummaryDto>> GetDriversRequests(Guid id, CancellationToken cancellationToken = default);
    Task<DriverResponseDto> DeleteWithComment(Guid id, string comment, CancellationToken cancellationToken);
}
