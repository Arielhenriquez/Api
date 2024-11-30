using Api.Application.Interfaces.Collaborators;
using Api.Domain.Constants;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class SyncCollaboratorFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                var userOid = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == TokenClaimsConstants.UserOid)?.Value!;
                if (!string.IsNullOrEmpty(userOid))
                {
                    using var scope = context.HttpContext.RequestServices.CreateScope();
                    var graphService = scope.ServiceProvider.GetRequiredService<IGraphUserService>();
                    await graphService.SyncUserFromGraph(userOid, CancellationToken.None);
                }
            }
            await next();
        }
    }
}
