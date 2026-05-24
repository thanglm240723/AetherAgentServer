using AetherAgent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AetherAgent.Infrastructure.Persistence.Configurations;

public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        // TODO: cấu hình table, indexes, length, HasConversion<string>() cho enum Role.
        // Unique index trên Username. Soft-delete inherited từ BaseEntity.
    }
}
