using Api.Application.Interfaces;
using Api.Application.Interfaces.Collaborators;
using Api.Application.Interfaces.Inventory;
using Api.Application.Interfaces.Transport;
using Api.Infrastructure.Persistence.Context;
using Api.Infrastructure.Persistence.Repositories;
using Api.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Infrastructure;

public static class IoC
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;
        return services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(connectionString))
        .AddTransient<IDbContext, ApplicationDbContext>()
        .AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>))
        .AddScoped<ICollaboratorRepository, CollaboratorRepository>()
        .AddScoped<IInventoryItemRepository, InventoryItemRepository>()
        .AddScoped<IDriverRepository, DriverRepository>()
        .AddScoped<IVehicleRepository, VehicleRepository>()
        .AddScoped<IInventoryRequestRepository, InventoryRequestRepository>()
        .AddScoped<ITransportRequestRepository, TransportRequestRepository>()
        .AddTransient<IEmailService, EmailService>();
    }
}
