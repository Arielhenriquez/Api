using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.TransportRequest.Dtos;

namespace Api.Application.Interfaces.Transport;

public interface ITransportService
{
    Task<Paged<TransportSummaryDto>> GetPagedTransportRequests(PaginationQuery paginationQuery, CancellationToken cancellationToken);
    Task<IEnumerable<TransportSummaryDto>> GetTransportRequestDetails(Guid id, CancellationToken cancellationToken);
    Task<TransportResponseDto> AddTransportRequest(TransportRequestDto transportRequestDto, CancellationToken cancellationToken);
    //Task<TransportResponseDto> AddTransportResponse( CancellationToken cancellationToken);
}
