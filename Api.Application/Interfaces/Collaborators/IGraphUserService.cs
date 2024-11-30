using Api.Application.Features.Collaborators.Dtos.GraphDtos;
using Api.Domain.Entities;

namespace Api.Application.Interfaces.Collaborators;

public interface IGraphUserService
{
    Task<LoggedUser> Current();
    Task<GraphUserDto> FindUserWithManagerAsync(string userOid);
    Task<Collaborator> SyncUserFromGraph(string userOid, CancellationToken cancellationToken);
}
