using Api.Application.Features.Collaborators.Dtos;
using Api.Application.Features.Collaborators.Dtos.GraphDtos;
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
    //Move to Service maybe graph service..
    public async Task<GraphUserDto> FindUserWithManagerAsync(string userOid)
    {
        // Fetch user details
        var user = await _graphServiceClient.Users[userOid]
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

        // Fetch the manager details separately
        var manager = await GetManagerDetailsAsync(userOid);

        // Map to GraphUserDto
        return new GraphUserDto
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            UserPrincipalName = user.UserPrincipalName,
            Mail = user.Mail,
            GivenName = user.GivenName,
            SurName = user.Surname,
            Department = user.Department,
            ManagerDto = manager
        };
    }
    //Move to Service maybe graph service..
    private async Task<ManagerDto?> GetManagerDetailsAsync(string userOid)
    {
        try
        {
            var manager = await _graphServiceClient.Users[userOid].Manager.GetAsync();
            if (manager is User managerUser)
            {
                return new ManagerDto
                {
                    Id = managerUser.Id,
                    DisplayName = managerUser.DisplayName,
                    UserPrincipalName = managerUser.UserPrincipalName,
                    Mail = managerUser.Mail
                };
            }
        }
        catch (Exception ex)
        {
            // Handle cases where the manager does not exist
            Console.WriteLine($"Manager not found: {ex.Message}");
        }

        return null;
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

    public async Task<List<AppRole>> GetAppRoles()
    {
        var spId = Guid.Parse(_servicePrincipalId);

        var result = await _graphServiceClient.ServicePrincipals[_servicePrincipalId].AppRoleAssignments.GetAsync();
        var servicePrincipal = await _graphServiceClient.ServicePrincipals[spId.ToString()]
            .GetAsync();

        return servicePrincipal?.AppRoles ?? [];
    }
    public async Task<AppRoleAssignmentCollectionResponse> GetAppRolesAssignments(string userId, Guid servicePrincipalId)
    {
        return await _graphServiceClient.Users[userId]
           .AppRoleAssignments
           .GetAsync(requestConfiguration =>
           {
               requestConfiguration.QueryParameters.Filter = $"resourceId eq {servicePrincipalId}";
           });
    }
    //Todo: Remove unused code.. move mapping to projection or services
    public async Task<List<AppRoleDto>> GetAppRolesAssignedToUser(string userId, Guid servicePrincipalId)
    {
        // Fetch AppRoleAssignments for the user, filtered by the Service Principal ID
        var appRoleAssignmentsResponse = await _graphServiceClient.Users[userId]
            .AppRoleAssignments
            .GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Filter = $"resourceId eq {servicePrincipalId}";
            });

        // Extract AppRoleAssignments from the response
        var appRoleAssignments = appRoleAssignmentsResponse?.Value;
        if (appRoleAssignments == null || appRoleAssignments.Count == 0)
        {
            return new List<AppRoleDto>(); // Return an empty list if no roles are assigned
        }

        // Fetch the Service Principal for the application to get all available AppRoles
        var servicePrincipal = await _graphServiceClient.ServicePrincipals[servicePrincipalId.ToString()]
            .GetAsync();

        var appRoles = servicePrincipal.AppRoles;

        // Map the assigned AppRoles to their corresponding definitions in the Service Principal
        var assignedAppRoles = appRoles
            .Where(appRole => appRoleAssignments.Any(assignment => assignment.AppRoleId == appRole.Id))
            .Select(appRole => new AppRoleDto
            {
                Id = appRole.Id,
                DisplayName = appRole.DisplayName,
                Description = appRole.Description,
                Value = appRole.Value
            })
            .ToList();

        return assignedAppRoles;
    }
    public async Task<AppRoleAssignment> AddPermissionToUser(AssignRoleToUserDto command, CancellationToken cancellationToken)
    {

        var roleAssignment = new AppRoleAssignment
        {
            Id = Guid.NewGuid().ToString(),
            PrincipalId = Guid.Parse(command.UserId),
            ResourceId = Guid.Parse(_servicePrincipalId),
            AppRoleId = Guid.Parse(command.RoleId),
            PrincipalDisplayName = command.UserId,
            ResourceDisplayName = _servicePrincipalId
        };

        return await _graphServiceClient.Users[command.UserId]
                     .AppRoleAssignments
                     .PostAsync(roleAssignment);
    }

    public async Task<AppRoleAssignment> DeletePermission(DeleteRoleFromUserDto command, CancellationToken cancellationToken)
    {
        await _graphServiceClient.Users[command.UserId]
                     .AppRoleAssignments[command.AppRoleAssignmentId]
                     .DeleteAsync();

        return new AppRoleAssignment()
        {
            PrincipalId = Guid.Parse(command.UserId),
            PrincipalDisplayName = command.UserId,
            ResourceId = Guid.Empty,
            AppRoleId = Guid.Empty,
            ResourceDisplayName = string.Empty
        };
    }
}
