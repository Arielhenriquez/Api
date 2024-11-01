using Api.Application.Common.Pagination;
using Api.Application.Features.Collaborators.Dtos;

namespace Api.Application.Interfaces.Collaborators;

public interface ICollaboratorService
{
    Task<Paged<CollaboratorResponseDto>> GetPagedCollaborators(PaginationQuery paginationQuery, CancellationToken cancellationToken);
    Task<CollaboratorResponseDto> GetCollaboratorById(Guid id);
    Task<List<CollaboratorResponseDto>> FindCollaboratorByName(string criteria);
    //Task<CollaboratorResponseDto> CreateCollaborator(CollaboratorResponseDto request, CancellationToken cancellationToken = default);
    //Task<CollaboratorResponseDto> UpdateCollaborator(CollaboratorResponseDto request, CancellationToken cancellationToken = default);
    //Task<CollaboratorResponseDto> DeleteCollaboratorById(Guid id);
}
