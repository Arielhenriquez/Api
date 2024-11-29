using Api.Application.Features.Collaborators.Dtos.GraphDtos;

namespace Api.Application.Interfaces.Collaborators;

public interface IGraphUserService
{
    Task<LoggedUser> Current();
}
