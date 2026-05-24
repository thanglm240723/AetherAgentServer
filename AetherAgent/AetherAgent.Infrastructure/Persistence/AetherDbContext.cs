using AetherAgent.Application.Interfaces;
using AetherAgent.Domain.Common;
using AetherAgent.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AetherAgent.Infrastructure.Persistence;

public sealed class AetherDbContext : DbContext, IUnitOfWork
{
    public AetherDbContext(DbContextOptions<AetherDbContext> options) : base(options) { }

    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AetherDbContext).Assembly);

        // LESSON-001: Global soft-delete filter — đặt trong OnModelCreating, KHÔNG OnConfiguring.
        // RefreshToken KHÔNG apply filter này (xem RefreshTokenConfiguration).
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType) &&
                entityType.ClrType != typeof(RefreshToken))
            {
                var param = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                var prop = System.Linq.Expressions.Expression.Property(param, nameof(BaseEntity.IsDeleted));
                var notDeleted = System.Linq.Expressions.Expression.Not(prop);
                var lambda = System.Linq.Expressions.Expression.Lambda(notDeleted, param);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}
