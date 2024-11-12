using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.TransportRequest.Dtos;

namespace Api.Application.Interfaces.Transport;

public interface ITransportRequestRepository
{
    Task<IEnumerable<TransportSummaryDto>> GetSummary(Guid id, CancellationToken cancellationToken = default);
    Task<Paged<TransportSummaryDto>> SearchAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default);
}
