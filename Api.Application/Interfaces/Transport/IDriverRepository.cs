using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Drivers.Dtos;

namespace Api.Application.Interfaces.Transport;

public interface IDriverRepository
{
    Task<IEnumerable<DriverSummaryDto>> GetSummary(Guid id, CancellationToken cancellationToken = default);
    Task<List<DriverResponseDto>> GetByName(string name, CancellationToken cancellationToken = default);
    Task<Paged<DriverResponseDto>> SearchAsync(PaginationQuery query, CancellationToken cancellationToken = default);
}
