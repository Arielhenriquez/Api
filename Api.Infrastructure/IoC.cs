using Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Infrastructure;

public static class IoC
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;
        services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(connectionString));
        services.AddTransient<IDbContext, ApplicationDbContext>();
        // services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        return services;
    }
}
