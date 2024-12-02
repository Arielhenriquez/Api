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

    public async Task<CollaboratorResponseDto> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var collaborator = await _context.Set<Collaborator>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        return collaborator != null ? CollaboratorProjections.ToResponseDto(collaborator) : null;
    }

    public async Task<List<CollaboratorResponseDto>> GetByEmail(string email, CancellationToken cancellationToken = default)
    {
        var collaborators = await _db
            .Where(c => EF.Functions.Like(c.Email, $"%{email}%"))
            .ToListAsync(cancellationToken);

        return collaborators.Select(CollaboratorProjections.ToResponseDto).ToList();
    }

    public async Task<Paged<CollaboratorResponseDto>> SearchAsync(PaginationQuery query, CancellationToken cancellationToken = default)
    {
        var collaborators = await _context.Set<Collaborator>()
            .AsNoTracking()
            .Where(CollaboratorPredicates.Search(query.Search))
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync(cancellationToken);

        var processedResults = collaborators.Select(CollaboratorProjections.ToResponseDto).ToList();

        return processedResults.Paginate(query.PageNumber, query.PageSize);
    }
}
