using Api.Application.Features.Transport.TransportRequest.Dtos;

namespace Api.Application.Interfaces.Transport;

public interface ITransportService
{
    //Task<Paged<InventorySummaryDto>> GetPagedInventoryRequest(PaginationQuery paginationQuery, CancellationToken cancellationToken);
    //Task<IEnumerable<InventorySummaryDto>> GetInventoryRequestDetails(Guid id, CancellationToken cancellationToken);
    Task<TransportResponseDto> AddTransportRequest(TransportRequestDto transportRequestDto, CancellationToken cancellationToken);
}
