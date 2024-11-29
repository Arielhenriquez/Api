using System.Reflection;
using Api.Application.Features.Collaborators.Services;
using Api.Application.Features.Inventory.InventoryItems.Services;
using Api.Application.Features.Inventory.InventoryRequest.Services;
using Api.Application.Features.Transport.Drivers.Services;
using Api.Application.Features.Transport.TransportRequest.Services;
using Api.Application.Features.Transport.Vehicles.Services;
using Api.Application.Interfaces.Collaborators;
using Api.Application.Interfaces.Inventory;
using Api.Application.Interfaces.Transport;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Application;

public static class IoC
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
           .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
           .AddFluentValidationAutoValidation()
           .AddScoped<ICollaboratorService, CollaboratorService>()
           .AddScoped<IInventoryItemsService, InventoryItemsService>()
           .AddScoped<IDriverService, DriverService>()
           .AddScoped<IVehicleService, VehicleService>()
           .AddScoped<IInventoryRequestService, InventoryRequestService>()
           .AddScoped<ITransportService, TransportService>()
           .AddScoped<IGraphUserService, GraphUserService>();
    }
}
