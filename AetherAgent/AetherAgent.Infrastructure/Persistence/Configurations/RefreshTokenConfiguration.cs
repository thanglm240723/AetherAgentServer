using AetherAgent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AetherAgent.Infrastructure.Persistence.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // TODO: index trên TokenHash + UserId. HasOne(AppUser) cascade delete.
        // KHÔNG apply soft-delete global filter — token revoked vẫn cần query được.
    }
}
