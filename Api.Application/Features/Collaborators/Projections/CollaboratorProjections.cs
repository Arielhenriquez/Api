using System.Linq.Expressions;
using System;
using Api.Application.Features.Collaborators.Dtos;
using Api.Domain.Entities.InventoryEntities;

namespace Api.Application.Features.Collaborators.Projections;

public class CollaboratorProjections
{
    public static Expression<Func<Collaborator, CollaboratorResponseDto>> Search => (Collaborator collaborator) => new CollaboratorResponseDto()
    {
        Id = collaborator.Id,
        Name = collaborator.Name,
        Supervisor = collaborator.Supervisor,
        Deparment = collaborator.Department,
    };
}
