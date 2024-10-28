using Api.Domain.Entities.InventoryEntities;
using Api.Domain.Entities.TransportEntities;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Persistence.Context;

public interface IApplicationDbContext : IDbContext { }
public class ApplicationDbContext(DbContextOptions options) : BaseDbContext(options), IApplicationDbContext
{
    public DbSet<Collaborator> Collaborators { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<InventoryRequest> InventoryRequests { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<TransportRequest> TransportRequests { get; set; }

}
