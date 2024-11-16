using Api.Application.Common.Extensions;
using Api.Domain.Entities;
using Api.Domain.Enums;
using System.Linq.Expressions;

namespace Api.Application.Features.Collaborators.Predicates;

public static class CollaboratorPredicates
{
    public static Expression<Func<Collaborator, bool>> Search(string criteria)
    {
        var matchingEnum = Enum.GetValues(typeof(UserRoles))
            .Cast<Enum>()
            .FirstOrDefault(e => e.DisplayName().Contains(criteria, StringComparison.OrdinalIgnoreCase));

        if (matchingEnum != null)
        {
            var parsedStatus = (UserRoles)matchingEnum;

            return transportEntity =>
                transportEntity.Roles == parsedStatus;
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
