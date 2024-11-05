using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Vehicles.Dtos;

namespace Api.Application.Interfaces.Transport;

public interface IVehicleRepository
{
    Task<Paged<VehicleResponseDto>> SearchAsync(PaginationQuery query, CancellationToken cancellationToken = default);
}
