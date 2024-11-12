using Api.Domain.Entities;
using Api.Domain.Enums;
using System.Linq.Expressions;

namespace Api.Application.Features.Collaborators.Predicates;

public static class CollaboratorPredicates
{
    public static Expression<Func<Collaborator, bool>> Search(string criteria)
    {
        if (Enum.TryParse<UserRoles>(criteria, ignoreCase: true, out var parsedRole))
        {
            return collaborator =>
                collaborator.Roles == parsedRole ||
                string.IsNullOrWhiteSpace(criteria);
        }
        else
        {
            return collaborator =>
                collaborator.Name.Contains(criteria) ||
                collaborator.Supervisor.Contains(criteria) ||
                collaborator.Department.Contains(criteria) ||
                string.IsNullOrWhiteSpace(criteria);
        }
    }
}
