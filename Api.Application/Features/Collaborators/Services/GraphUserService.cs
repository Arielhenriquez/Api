using System.Security.Claims;
using Api.Application.Features.Collaborators.Dtos.GraphDtos;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Collaborators;
using Api.Domain.Constants;
using Api.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;

namespace Api.Application.Features.Collaborators.Services;

public class GraphUserService : IGraphUserService
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IGraphProvider _provider;
    private readonly IBaseRepository<Collaborator> _collaboratorRepository;

    public GraphUserService(IHttpContextAccessor accessor, IGraphProvider provider, IBaseRepository<Collaborator> collaboratorRepository)
    {
        _accessor = accessor;
        _provider = provider;
        _collaboratorRepository = collaboratorRepository;
    }
    public async Task<LoggedUser> Current()
    {
        var userDto = new LoggedUser();
        if (_accessor.HttpContext == null) return userDto;

        var user = _accessor.HttpContext.User;
        if (!user.Identity!.IsAuthenticated)
        {
            throw new ArgumentException("El usuario no está autenticado");
        }
        var claims = user.Claims.ToList();

        var adUser = new LoggedUser()
        {
            Oid = claims
                .FirstOrDefault(x => x.Type == TokenClaimsConstants.UserOid)?.Value!,
            Name = claims.FirstOrDefault(x => x.Type == TokenClaimsConstants.UserName)?.Value,
            Email = claims.FirstOrDefault(x => x.Type == TokenClaimsConstants.Email)?.Value!,
            Roles = user.FindAll(ClaimTypes.Role)
                   .Select(claim => claim.Value)
                   .ToList(),
        };

        return adUser;
    }

    public async Task<GraphUserDto> FindUserWithManagerAsync(string userOid)
    {
        var user = await _provider.FindUserAsync(userOid);
        var manager = await GetManagerDetailsAsync(userOid);

        return new GraphUserDto
        {
            Id = user.Id!,
            Name = user.DisplayName!,
            Email = user.UserPrincipalName!,
            FirstName = user.GivenName,
            LastName = user.Surname,
            Department = user.Department,
            Manager = manager
        };
    }
    private async Task<ManagerDto?> GetManagerDetailsAsync(string userOid)
    {
        try
        {
            var manager = await _provider.GetUserManager(userOid);
            if (manager is User managerUser)
            {
                return new ManagerDto
                {
                    Id = managerUser.Id,
                    Name = managerUser.DisplayName,
                    Email = managerUser.Mail
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Manager not found: {ex.Message}");
        }

        return null;
    }


    public async Task<Collaborator> SyncUserFromGraph(string userOid, CancellationToken cancellationToken)
    {
        var userClaims = _accessor?.HttpContext?.User;

        var azureRoles = userClaims!.FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value)
            .ToList() ?? [];

        var azureRolesAvailable = await _provider.GetAppRoles(cancellationToken);

        var applicantRole = azureRolesAvailable
            .FirstOrDefault(x => x.Value == "Solicitante.ReadWrite");

        if (azureRoles.Count == 0)
        {
            azureRoles.Add(applicantRole.Value);

            await _provider.AddPermissionsToUser(userOid, applicantRole.Id.ToString(), cancellationToken);
        }

        var graphUser = await FindUserWithManagerAsync(userOid);

        var collaborator = await _collaboratorRepository
            .Query()
            .FirstOrDefaultAsync(x => x.UserOid == userOid, cancellationToken);

        if (collaborator == null)
        {
            collaborator = new Collaborator
            {
                Name = graphUser.Name,
                Email = graphUser.Email,
                Department = graphUser.Department ?? "No Department",
                Supervisor = graphUser.Manager?.Email ?? "No manager",
                UserOid = userOid,
                Roles = [applicantRole?.Value],
                CreatedBy = "System"
            };

            await _collaboratorRepository.AddAsync(collaborator, cancellationToken);
        }
        else
        {
            collaborator.Name = graphUser.Name;
            collaborator.Email = graphUser.Email;
            collaborator.Department = graphUser.Department ?? "No Department";
            collaborator.Supervisor = graphUser.Manager?.Email ?? "No manager";
            collaborator.Roles = azureRoles;
            collaborator.UpdatedBy = "System";

            await _collaboratorRepository.UpdateAsync(collaborator, cancellationToken);
        }

        return collaborator;
    }
}
