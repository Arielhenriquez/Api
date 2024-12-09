using Api.Application.Common.Pagination;
using Api.Application.Features.Collaborators.Dtos;
using Api.Domain.Entities;

namespace Api.Application.Interfaces.Collaborators;

public interface ICollaboratorRepository
{
    Task<List<CollaboratorResponseDto>> GetByEmail(string email, CancellationToken cancellationToken = default);
    Task<CollaboratorResponseDto> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Paged<CollaboratorResponseDto>> SearchAsync(PaginationQuery query, CancellationToken cancellationToken = default);
    Task<Collaborator> UpdateAsync(Collaborator updatedCollaborator, CancellationToken cancellationToken = default);
}
