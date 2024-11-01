using Api.Application.Features.Collaborators.Services;
using Api.Application.Features.Inventory.InventoryItems.Services;
using Api.Application.Features.Transport.Drivers.Services;
using Api.Application.Interfaces;
using Api.Application.Interfaces.Collaborators;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Application;

public static class IoC
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
           .AddScoped<ICollaboratorService, CollaboratorService>()
           .AddScoped<IInventoryItemsService, InventoryItemsService>()
           .AddScoped<IDriverService, DriverService>();
    }
}
