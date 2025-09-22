using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tajan.Identity.Infrastructure.Models;
using Tajan.Standard.Application.Abstractions;

namespace Tajan.Identity.Infrastructure.Contexts;


public class IdentityContext(DbContextOptions<IdentityContext> options) : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
{

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .HasDefaultSchema("Identity")
            .MapModels()
            .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        => base.Set<TEntity>();

    public new async Task SaveChangesAsync(CancellationToken ct = default)
    {
        OnBeforeSaving();
        await base.SaveChangesAsync(ct);
    }

    private void OnBeforeSaving()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.CurrentValues["IsDeleted"] = false;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["IsDeleted"] = true;
                    break;
            }
        }
    }

    
}

public static class ConfigModels
{
    public static ModelBuilder MapModels(this ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable(name: "User");
        });

        return builder;
    }
}