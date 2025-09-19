using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Tajan.Standard.Domain.Abstractions;
using MediatR;
using Tajan.Standard.Application.Abstractions;

namespace Tajan.Standard.Infrastructure.Persistence.Contexts;

public class SharedDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;
    public SharedDbContext(DbContextOptions options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().HaveMaxLength(500);
        configurationBuilder.Properties<string>().AreUnicode(false);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // Gather domain events from tracked entities BEFORE saving
        var domainEvents = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .SelectMany(e => e.DomainEvents)
            .ToList();

        OnBeforeSaving();
        var result = await base.SaveChangesAsync(ct);

        // Dispatch after save (or use outbox pattern for reliability)
        foreach (var domainEvent in domainEvents)
        {
            var notification = CreateNotification(domainEvent);
            if (notification is not null)
            {
                await _mediator.Publish(notification, ct);
            }
        }

        // Clear events
        foreach (var entity in ChangeTracker.Entries<Entity>())
        {
            entity.Entity.ClearDomainEvents();
        }


        return result;
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

    private static INotification? CreateNotification(IDomainEvent domainEvent)
    {
        var domainEventType = domainEvent.GetType();
        var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEventType);
        return (INotification?)Activator.CreateInstance(notificationType, domainEvent);
    }

}