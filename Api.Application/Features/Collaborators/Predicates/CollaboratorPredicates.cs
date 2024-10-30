using Api.Domain.Entities.InventoryEntities;
using System.Linq.Expressions;

namespace Api.Application.Features.Collaborators.Predicates;

public class CollaboratorPredicates
{
    public static Expression<Func<Collaborator, bool>> Search(string criteria)
    {
        return (Collaborator person) =>
            person.Name.Contains(criteria) ||
            person.Supervisor.Contains(criteria) ||
            person.Department.Contains(criteria) ||
            string.IsNullOrWhiteSpace(criteria);
    }
}
