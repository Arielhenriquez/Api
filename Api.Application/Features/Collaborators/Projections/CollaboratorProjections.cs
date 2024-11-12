using Api.Application.Features.Collaborators.Dtos;
using Api.Domain.Entities;
using System.Linq.Expressions;

namespace Api.Application.Features.Collaborators.Projections;

public static class CollaboratorProjections
{
    public static Expression<Func<Collaborator, CollaboratorResponseDto>> Search => (Collaborator collaborator) => new CollaboratorResponseDto()
    {
        Id = collaborator.Id,
        Name = collaborator.Name,
        Supervisor = collaborator.Supervisor,
        Deparment = collaborator.Department,
        Roles = collaborator.Roles,
    };
}
