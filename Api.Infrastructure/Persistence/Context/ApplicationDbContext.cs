using Api.Domain.Entities;
using Api.Domain.Entities.InventoryEntities;
using Api.Domain.Entities.TransportEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Api.Infrastructure.Persistence.Context;

public interface IApplicationDbContext : IDbContext { }
public class ApplicationDbContext(DbContextOptions options, IHttpContextAccessor context) : BaseDbContext(options, context), IApplicationDbContext
{
    public DbSet<Collaborator>? Collaborators { get; set; }
    public DbSet<InventoryItem>? InventoryItems { get; set; }
    public DbSet<InventoryRequest>? InventoryRequests { get; set; }
    public DbSet<Driver>? Drivers { get; set; }
    public DbSet<Vehicle>? Vehicles { get; set; }
    public DbSet<TransportRequest>? TransportRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var rolesConverter = new ValueConverter<List<string>, string>(
            v => System.Text.Json.JsonSerializer.Serialize(v, new System.Text.Json.JsonSerializerOptions()),
            v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, new System.Text.Json.JsonSerializerOptions()) ?? new List<string>());

        modelBuilder.Entity<Collaborator>()
            .Property(c => c.Roles)
            .HasConversion(rolesConverter) 
            .HasColumnType("json");
    }
}
