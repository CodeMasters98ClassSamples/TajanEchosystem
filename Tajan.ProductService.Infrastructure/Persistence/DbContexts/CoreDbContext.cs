using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using Tajan.ProductService.API.Entities;

namespace Tajan.ProductService.Infrastructure.DbContexts;

public class CoreDbContext : DbContext
{
    public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().HaveMaxLength(500);
        configurationBuilder.Properties<string>().AreUnicode(false);
    }
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.RegisterAllEntities(typeof(AppDbContext).Assembly);
        modelBuilder.HasDefaultSchema("myschema");

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.NoAction;

        // Configure the Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0.0m);

            // Add an index on Name
            entity.HasIndex(p => p.Name).IsUnique(false);
        });

        base.OnModelCreating(builder);
    }
}