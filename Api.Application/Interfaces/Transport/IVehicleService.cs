using Api.Application.Common.Pagination;
using Api.Application.Features.Transport.Vehicles.Dtos;

namespace Api.Application.Interfaces.Transport;

public interface IVehicleService : IBaseService<VehicleRequestDto, VehicleResponseDto>
{
    Task<Paged<VehicleResponseDto>> GetPagedVehicles(PaginationQuery paginationQuery, bool isDeleted, CancellationToken cancellationToken);
    Task<VehicleResponseDto> DeleteWithComment(Guid id, string comment, CancellationToken cancellationToken);
}
