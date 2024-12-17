using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Vehicles.Dtos;

namespace Api.Application.Interfaces.Transport;

public interface IVehicleRepository
{
    Task<Paged<VehicleResponseDto>> SearchAsync(PaginationQuery query, bool isDeleted, CancellationToken cancellationToken = default);
    Task<VehicleResponseDto> DeleteWithComment(Guid id, string comment, CancellationToken cancellationToken);
}
