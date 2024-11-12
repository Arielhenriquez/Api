using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Api.Infrastructure.Persistence.Context;


public interface IDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

}
public abstract class BaseDbContext : DbContext, IDbContext
{
    protected BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    private void SetAuditEntities()
    {
        string email = "Anonymous";

        foreach (var entry in ChangeTracker.Entries<IBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:

                    entry.Entity.IsDeleted = false;
                    entry.Entity.CreatedDate = DateTimeOffset.UtcNow;
                    entry.Entity.CreatedBy = email;
                    break;

                case EntityState.Modified:
                    entry.Property(x => x.CreatedDate).IsModified = false;
                    entry.Property(x => x.CreatedBy).IsModified = false;
                    entry.Entity.UpdatedDate = DateTimeOffset.UtcNow;
                    entry.Entity.UpdatedBy = email;
                    break;

                case EntityState.Deleted:
                    entry.Property(x => x.CreatedDate).IsModified = false;
                    entry.Property(x => x.CreatedBy).IsModified = false;
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedDate = DateTimeOffset.UtcNow;
                    entry.Entity.DeletedBy = email;
                    break;

                default:
                    break;
            }
        }
    }

    public override int SaveChanges()
    {
        SetAuditEntities();
        return base.SaveChanges();
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetAuditEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditEntities();
        return await base.SaveChangesAsync(cancellationToken);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
        foreach (var type in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IBase).IsAssignableFrom(type.ClrType))
            {
                modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }
        }
    }
}
