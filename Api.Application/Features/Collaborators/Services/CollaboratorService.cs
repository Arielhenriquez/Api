using Api.Application.Common.Exceptions;
using Api.Application.Common.Pagination;
using Api.Application.Features.Collaborators.Dtos;
using Api.Application.Interfaces.Collaborators;

namespace Api.Application.Features.Collaborators.Services;

public class CollaboratorService : ICollaboratorService
{
    private readonly ICollaboratorRepository _collaboratorRepository;

    public CollaboratorService(ICollaboratorRepository collaboratorRepository)
    {
        _collaboratorRepository = collaboratorRepository;
    }
    public Task<Paged<CollaboratorResponseDto>> GetPagedCollaborators(PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        return _collaboratorRepository.SearchAsync(paginationQuery, cancellationToken);
    }

    public async Task<List<CollaboratorResponseDto>> FindCollaboratorByName(string criteria)
    {
        var collaborators = await _collaboratorRepository.GetByName(criteria);

        if (collaborators == null || collaborators.Count == 0)
        {
            throw new NotFoundException($"No collaborators found with name containing: {criteria}");
        }

        return collaborators;
    }

    public async Task<CollaboratorResponseDto> GetCollaboratorById(Guid id)
    {
        return await _collaboratorRepository.GetById(id);
    }

}
