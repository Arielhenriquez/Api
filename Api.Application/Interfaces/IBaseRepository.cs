using Api.Domain.Entities;

namespace Api.Application.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class, IBase
{
    IQueryable<TEntity> Query();
    Task<TEntity> GetById(Guid id);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<ICollection<TEntity>> AddRange(ICollection<TEntity> entities, CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<ICollection<TEntity>> UpdateRange(ICollection<TEntity> entities, CancellationToken cancellationToken = default);
    Task<TEntity> Delete(Guid id);
    Task RemoveRange(ICollection<TEntity> entities, CancellationToken cancellationToken = default);
}