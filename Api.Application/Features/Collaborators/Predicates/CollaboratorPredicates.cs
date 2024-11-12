using Api.Domain.Entities;
using System.Linq.Expressions;

namespace Api.Application.Features.Collaborators.Predicates;

public class CollaboratorPredicates
{
    public static Expression<Func<Collaborator, bool>> Search(string criteria)
    {
        return (Collaborator collaborator) =>
            collaborator.Name.Contains(criteria) ||
            collaborator.Supervisor.Contains(criteria) ||
            collaborator.Department.Contains(criteria) ||
            string.IsNullOrWhiteSpace(criteria);
    }
}
