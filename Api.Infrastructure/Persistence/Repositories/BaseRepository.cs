using Api.Application.Common.Exceptions;
using Api.Application.Interfaces;
using Api.Domain.Entities;
using Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Api.Infrastructure.Persistence.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity>
  where TEntity : class, IBase
{
    protected readonly IDbContext _context;
    protected readonly DbSet<TEntity> _db;
    public BaseRepository(IDbContext context)
    {
        _context = context;
        _db = context.Set<TEntity>();
    }
    public virtual IQueryable<TEntity> Query()
    {
        return _db.AsQueryable();
    }

    public virtual IQueryable<TEntity> ListOrderedBy()
    {
        return _db.AsQueryable().OrderByDescending(c => c.CreatedDate);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAll()
    {
        return await Query().ToListAsync();
    }

    //public async Task<IEnumerable<TEntity>> GetDeleted()
    //{
    //   var query = ListOrderedBy().Where(x=>x.IsDeleted == );
    //}

    public virtual async Task<TEntity> GetById(Guid id, CancellationToken cancellationToken)
    {
        var entity = await Query().Where(x => x.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);

        if (entity is null) throw new NotFoundException(typeof(TEntity).Name, id);

        return entity;
    }
    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var result = await _db.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public virtual async Task<ICollection<TEntity>> AddRange(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _db.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entities;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var _entity = await GetById(entity.Id, cancellationToken);
        Type type = typeof(TEntity);
        PropertyInfo[] propertyInfo = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var item in propertyInfo)
        {
            var fieldValue = item.GetValue(entity);
            if (fieldValue != null)
            {
                item.SetValue(_entity, fieldValue);
            }
        }
        await _context.SaveChangesAsync(cancellationToken);
        return _entity;
    }
    public async Task<ICollection<TEntity>> UpdateRange(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _db.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
        return entities;
    }

    public virtual async Task PatchAsync(Guid id, Dictionary<string, object> updates, CancellationToken cancellationToken = default)
    {
        var entity = await GetById(id, cancellationToken);

        Type type = typeof(TEntity);
        foreach (var update in updates)
        {
            var property = type.GetProperty(update.Key);
            if (property != null && property.CanWrite)
            {
                var value = update.Value;

                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (value == null || property.PropertyType.GetGenericArguments()[0].IsEnum)
                    {
                        value = value != null ? Enum.Parse(property.PropertyType.GetGenericArguments()[0], value.ToString()) : null;
                    }
                }
                else if (property.PropertyType.IsEnum)
                {
                    value = Enum.Parse(property.PropertyType, value.ToString());
                }

                property.SetValue(entity, value);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<TEntity> Delete(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetById(id, cancellationToken);

        var result = _db.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }

    public virtual async Task RemoveRange(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _db.RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
