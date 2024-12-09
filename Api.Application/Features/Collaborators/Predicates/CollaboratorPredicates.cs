using System.Linq.Expressions;
using Api.Domain.Entities;

namespace Api.Application.Features.Collaborators.Predicates;
public static class CollaboratorPredicates
{
    public static Expression<Func<Collaborator, bool>> Search(string criteria)
    {
        return collaborator =>
            collaborator.Name.Contains(criteria) ||
            collaborator.Supervisor.Contains(criteria) ||
            collaborator.Department.Contains(criteria) ||
            string.IsNullOrWhiteSpace(criteria);
    }
}
