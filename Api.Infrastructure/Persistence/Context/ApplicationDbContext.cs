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

    private static ValueConverter<List<T>, string> CreateJsonConverter<T>() =>
     new ValueConverter<List<T>, string>(
         v => System.Text.Json.JsonSerializer.Serialize(v, new System.Text.Json.JsonSerializerOptions()),
         v => System.Text.Json.JsonSerializer.Deserialize<List<T>>(v, new System.Text.Json.JsonSerializerOptions()) ?? new List<T>()
     );
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Collaborator>()
            .Property(c => c.Roles)
            .HasConversion(CreateJsonConverter<string>())
            .HasColumnType("json");

        modelBuilder.Entity<Collaborator>()
            .Property(c => c.Approvers)
            .HasConversion(CreateJsonConverter<string>())
            .HasColumnType("json");

        modelBuilder.Entity<InventoryRequest>()
            .Property(c => c.ApprovalHistory)
            .HasConversion(CreateJsonConverter<ApprovalEntry>())
            .HasColumnType("json");
    }
}
