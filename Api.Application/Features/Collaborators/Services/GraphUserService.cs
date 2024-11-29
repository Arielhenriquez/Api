using System.Security.Claims;
using Api.Application.Features.Collaborators.Dtos.GraphDtos;
using Api.Application.Interfaces.Collaborators;
using Microsoft.AspNetCore.Http;

namespace Api.Application.Features.Collaborators.Services;

public class GraphUserService : IGraphUserService
{
    private readonly IHttpContextAccessor _accessor;

    public GraphUserService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public async Task<LoggedUser> Current()
    {
        var userDto = new LoggedUser();
        if (_accessor.HttpContext == null) return userDto;

        var user = _accessor.HttpContext.User;
        var claims = user.Claims.ToList();

        var adUser = new LoggedUser()
        {
            Oid = user.Claims
                .FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value!,
            Name = user.Claims.FirstOrDefault(x => x.Type == "name")?.Value,
            Email = user.FindFirst(ClaimTypes.Upn)?.Value!,
            Roles = user.FindAll(ClaimTypes.Role)
                   .Select(claim => claim.Value)
                   .ToList(),
        };

        if (adUser.Oid is null)
            throw new ArgumentException("User is not autheticated");

        return adUser;
    }
}
