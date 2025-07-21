using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tajan.ProductServcie.Domain.Entities;

namespace Tajan.ProductService.Infrastructure.Persistence.Extensions;

public static class ModelBuilderExtensions
{
    /// <summary>
    /// this method gathers all classes marked by [Entity] attribute and register it to db context
    /// </summary>
    /// <param name="modelBuilder">ef core model builder</param>
    /// <param name="assemblies">assemblies to search for entity type</param>
    /// <returns>ef core model builder</returns>
    public static ModelBuilder RegisterAllEntities(this ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        IEnumerable<Type> types = assemblies.SelectMany(a => a.GetExportedTypes())
            .Where(t =>
                t is { IsClass: true, IsAbstract: false, IsPublic: true } &&
                Attribute.IsDefined(t, typeof(EntityAttribute)));

        foreach (Type type in types)
            modelBuilder.Entity(type);

        return modelBuilder;
    }
}
