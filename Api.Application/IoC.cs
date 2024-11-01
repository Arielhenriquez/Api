using Api.Application.Features.Collaborators.Services;
using Api.Application.Interfaces.Collaborators;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Application;

public static class IoC
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
           .AddScoped<ICollaboratorService, CollaboratorService>();
    }
}
