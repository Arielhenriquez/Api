using Api.Application.Interfaces;
using Api.Application.Interfaces.Collaborators;
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
        .AddScoped<IEmailService, EmailService>();
    }
}
