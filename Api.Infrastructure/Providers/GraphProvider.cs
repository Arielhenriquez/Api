using Api.Application.Interfaces;
using Api.Domain.Settings;
using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace Api.Infrastructure.Providers;

public class GraphProvider : IGraphProvider
{
    private readonly GraphServiceClient _graphServiceClient;
    private readonly string _servicePrincipalId;
    public GraphProvider(IOptions<GraphSettings> options)
    {
        var clientSecretCredential = new ClientSecretCredential(
        options.Value.TenantId, options.Value.ClientId, options.Value.ClientSecret);
        var graphServiceClient = new GraphServiceClient(clientSecretCredential);

        _graphServiceClient = graphServiceClient;
        _servicePrincipalId = options.Value.ServicePrincipalId ?? "";
    }

    /// <summary>
    /// Finds a user by object ID or user principal name (UPN).
    /// </summary>
    /// <param name="userOidOrUserPrincipalName">The object ID or user principal name of the user.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    public async Task<User> FindUserAsync(string userOidOrUserPrincipalName)
    {
        var user = await _graphServiceClient.Users[userOidOrUserPrincipalName]
            .GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Select =
                [
                "id",
                "displayName",
                "mail",
                "givenName",
                "surname",
                "userPrincipalName",
                "department"
                ];
            });

        return user;
    }


    public async Task<DirectoryObject> GetUserManager(string userOid)
    {
        return await _graphServiceClient.Users[userOid].Manager.GetAsync();
    }

    /// <summary>
    /// Checks if a user exists by user principal name.
    /// </summary>
    /// <param name="userPrincipalName">The user principal name of the user.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    public async Task<User> UserPrincipalExists(string userPrincipalName)
    {
        var user = await _graphServiceClient.Users.GetAsync((requestConfiguration) =>
        {
            requestConfiguration.QueryParameters.Filter = $"userPrincipalName eq '{userPrincipalName}'";
        });
        return user?.Value?.FirstOrDefault()!;
    }

    public async Task<List<AppRole>> GetAppRoles(CancellationToken cancellationToken)
    {
        var spId = Guid.Parse(_servicePrincipalId);

        var result = await _graphServiceClient.ServicePrincipals[_servicePrincipalId].AppRoleAssignments.GetAsync();
        var servicePrincipal = await _graphServiceClient.ServicePrincipals[spId.ToString()]
            .GetAsync(cancellationToken: cancellationToken);

        return servicePrincipal?.AppRoles ?? [];
    }

    public async Task<List<AppRoleAssignment>> GetAssignedRolesAsync(string userId, CancellationToken cancellationToken)
    {
        var spId = Guid.Parse(_servicePrincipalId);

        var appRoleAssignmentsResponse = await _graphServiceClient.Users[userId]
            .AppRoleAssignments
            .GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Filter = $"resourceId eq {spId}";
            }, cancellationToken);

        return appRoleAssignmentsResponse?.Value ?? [];
    }

    public async Task<bool> CheckRoleAssignmentExists(
        string userId,
        string roleIdOrAssignmentId,
        CancellationToken cancellationToken,
        bool validateByAssignmentId = false)
    {
        try
        {
            if (validateByAssignmentId)
            {
                await _graphServiceClient.Users[userId]
                    .AppRoleAssignments[roleIdOrAssignmentId]
                    .GetAsync(cancellationToken: cancellationToken);

                return true;
            }
            else
            {
                var appRoleAssignments = await _graphServiceClient.Users[userId]
                    .AppRoleAssignments
                    .GetAsync(cancellationToken: cancellationToken);

                return appRoleAssignments?.Value?.Any(assignment => assignment.AppRoleId.ToString() == roleIdOrAssignmentId) ?? false;
            }
        }
        catch (ServiceException ex)
        {
            return false;
        }
    }


    public async Task<AppRoleAssignment> AddPermissionsToUser(string userId, string roleId, CancellationToken cancellationToken)
    {
        var roleAssignment = new AppRoleAssignment
        {
            Id = Guid.NewGuid().ToString(),
            PrincipalId = Guid.Parse(userId),
            ResourceId = Guid.Parse(_servicePrincipalId),
            AppRoleId = Guid.Parse(roleId),
        };

        return await _graphServiceClient.Users[userId]
                     .AppRoleAssignments
                     .PostAsync(roleAssignment, cancellationToken: cancellationToken);
    }

    public async Task DeletePermissionsFromUser(string userId, string roleAssignmentId, CancellationToken cancellationToken)
    {
        await _graphServiceClient.Users[userId]
                     .AppRoleAssignments[roleAssignmentId]
                     .DeleteAsync(cancellationToken: cancellationToken);
    }
}
