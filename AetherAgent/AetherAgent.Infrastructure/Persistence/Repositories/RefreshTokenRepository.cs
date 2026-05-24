using AetherAgent.Application.Interfaces;
using AetherAgent.Domain.Entities;

namespace AetherAgent.Infrastructure.Persistence.Repositories;

public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AetherDbContext _db;
    public RefreshTokenRepository(AetherDbContext db) => _db = db;

    public Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task AddAsync(RefreshToken token, CancellationToken ct = default)
        => throw new NotImplementedException();

    public void Update(RefreshToken token)
        => throw new NotImplementedException();

    public Task<int> DeleteExpiredAsync(DateTime olderThan, CancellationToken ct = default)
        => throw new NotImplementedException();
}
