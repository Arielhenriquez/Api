using Api.Application.Common.Pagination;
using Api.Application.Features.Collaborators.Dtos;

namespace Api.Application.Interfaces.Collaborators;

public interface ICollaboratorRepository
{
    Task<List<CollaboratorResponseDto>> GetByName(string name, CancellationToken cancellationToken = default);
    Task<CollaboratorResponseDto> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Paged<CollaboratorResponseDto>> SearchAsync(PaginationQuery query, CancellationToken cancellationToken = default);
}
