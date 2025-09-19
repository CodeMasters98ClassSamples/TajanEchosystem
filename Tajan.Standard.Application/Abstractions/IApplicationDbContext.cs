using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Tajan.Standard.Application.Abstractions;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);

    DbSet<TEntity> Set<TEntity>() where TEntity : class;

}
