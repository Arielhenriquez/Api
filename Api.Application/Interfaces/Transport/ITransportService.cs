using Api.Application.Common.Pagination;
using Api.Application.Features.Inventory.InventoryItems.Dtos;
using Api.Application.Features.Transport.TransportRequest.Dtos;

namespace Api.Application.Interfaces.Transport;

public interface ITransportService
{
    Task<Paged<TransportSummaryDto>> GetPagedTransportRequests(PaginationQuery paginationQuery, CancellationToken cancellationToken);
    Task<IEnumerable<TransportSummaryDto>> GetTransportRequestDetails(Guid id, CancellationToken cancellationToken);
    Task<TransportResponseDto> AddTransportRequest(TransportRequestDto transportRequestDto, CancellationToken cancellationToken);
    Task AssignDriverAndVehicle(Guid transportRequestId, AssignDriverVehicleDto driverVehicleDto, CancellationToken cancellationToken);
    Task<string> UpdateExpiredTransportRequestsStatus(CancellationToken cancellationToken = default);
    Task<string> ApproveTransportRequest(ApprovalDto approvalDto, CancellationToken cancellationToken);
}
