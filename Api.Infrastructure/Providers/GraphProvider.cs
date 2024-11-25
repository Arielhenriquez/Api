using Api.Application.Interfaces;
using Api.Domain.Settings;
using Microsoft.Extensions.Options;

namespace Api.Infrastructure.Providers;

public class GraphProvider : IGraphProvider
{
    //private readonly GraphServiceClient _graphServiceClient;
    //public GraphProvider(IOptions<GraphSettings> options)
    //{
    //    var clientSecretCredential = new ClientSecretCredential(
    //    options.Value.TenantId, options.Value.ClientId, options.Value.ClientSecret);
    //    var graphServiceClient = new GraphServiceClient(clientSecretCredential);

    //    _graphServiceClient = graphServiceClient;
    //}

    /// <summary>
    /// Finds a user by object ID or user principal name (UPN).
    /// </summary>
    /// <param name="userOidOrUserPrincipalName">The object ID or user principal name of the user.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    //public async Task<User> FindUserAsync(string userOidOrUserPrincipalName)
    //{
    //    return await _graphServiceClient.Users[userOidOrUserPrincipalName].GetAsync();
    //}

    /// <summary>
    /// Checks if a user exists by user principal name.
    /// </summary>
    /// <param name="userPrincipalName">The user principal name of the user.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    //public async Task<User> UserPrincipalExists(string userPrincipalName)
    //{
    //    var user = await _graphServiceClient.Users.GetAsync((requestConfiguration) =>
    //    {
    //        requestConfiguration.QueryParameters.Filter = $"userPrincipalName eq '{userPrincipalName}'";
    //    });
    //    return user?.Value?.FirstOrDefault()!;
    //}


    //public async Task<IEnumerable<AppRoles>> GetAppRoles(string appRoleId)
    //{
    //    var assignments = await _graphServiceClient.Users[request.userId]
    //                    .AppRoleAssignments
    //                    .Request()
    //                    .Filter($"resourceId eq {servicePrincipalId}")
    //                    .GetAsync();

    //    var permissions = (await _graphServiceClient.ServicePrincipals[servicePrincipalId]
    //                      .Request()
    //                      .GetAsync())
    //                      .AppRoles
    //                      .Where(x => assignments.Any(y => y.AppRoleId == x.Id));
    //}

    //public async Task<AppRoleAssignment> AddPermissionToUser(AddUserPermissionCommand command, CancellationToken cancellationToken)
    //{

    //    var roleAssignment = new AppRoleAssignment
    //    {
    //        Id = Guid.NewGuid().ToString(),
    //        PrincipalId = Guid.Parse(command.UserId),
    //        ResourceId = Guid.Parse(servicePrincipalId),
    //        AppRoleId = Guid.Parse(command.PermissionId),
    //        PrincipalDisplayName = command.UserId,
    //        ResourceDisplayName = servicePrincipalId
    //    };

    //    return await _graphServiceClient.Users[command.UserId]
    //                 .AppRoleAssignments
    //                 .Request()
    //                 .AddAsync(roleAssignment);
    //}

    //public async Task<AppRoleAssignment> DeletePermission(AddUserPermissionCommand command, CancellationToken cancellationToken)
    //{
    //    await _graphServiceClient.Users[command.UserId]
    //                 .AppRoleAssignments[command.AppRoleAssignmentId]
    //                 .Request()
    //                 .DeleteAsync();

    //    return new AppRoleAssignment()
    //    {
    //        PrincipalId = Guid.Parse(command.UserId),
    //        PrincipalDisplayName = command.UserId,
    //        ResourceId = Guid.Empty,
    //        AppRoleId = Guid.Empty,
    //        ResourceDisplayName = string.Empty
    //    };
    //}


}
