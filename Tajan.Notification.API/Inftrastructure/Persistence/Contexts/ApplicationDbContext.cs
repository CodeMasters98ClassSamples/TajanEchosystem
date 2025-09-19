using MediatR;
using Microsoft.EntityFrameworkCore;
using Tajan.Notification.API.Models;
using Tajan.Standard.Infrastructure.Persistence.Contexts;

namespace Tajan.Notification.API.Persistence.Contexts;

public class ApplicationDbContext : SharedDbContext
{
    public DbSet<Sms> Sms { get; set; }
    public DbSet<SmsOutbox> SmsOutbox { get; set; }

    public ApplicationDbContext(DbContextOptions options, IMediator mediator) : base(options, mediator)
    {
    }
}
