using Api.Application.Common.Pagination;
using Api.Application.Features.Collaborators.Dtos;
using Api.Application.Features.Collaborators.Predicates;
using Api.Application.Features.Collaborators.Projections;
using Api.Application.Interfaces.Collaborators;
using Api.Domain.Entities;
using Api.Infrastructure.Extensions;
using Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Persistence.Repositories;

public class CollaboratorRepository : ICollaboratorRepository
{
    protected readonly IDbContext _context;
    protected readonly DbSet<Collaborator> _db;

    public CollaboratorRepository(IDbContext context)
    {
        _context = context;
        _db = context.Set<Collaborator>();
    }

    public Task<CollaboratorResponseDto> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Set<Collaborator>()
            .AsNoTracking()
            .Select(CollaboratorProjections.Search)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<List<CollaboratorResponseDto>> GetByName(string name, CancellationToken cancellationToken = default)
    {
        var collaborators = await _db
            .Where(c => EF.Functions.Like(c.Name, $"%{name}%"))
            .Select(CollaboratorProjections.Search)
            .ToListAsync(cancellationToken);

        return collaborators;
    }

    public Task<Paged<CollaboratorResponseDto>> SearchAsync(PaginationQuery query, CancellationToken cancellationToken = default)
    {
        return _context.Set<Collaborator>()
        .AsNoTracking()
        .Where(CollaboratorPredicates.Search(query.Search))
        .OrderByDescending(p => p.CreatedDate)
        .Select(CollaboratorProjections.Search)
        .Paginate(query.PageSize, query.PageNumber, cancellationToken);
    }
}

