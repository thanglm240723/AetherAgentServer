using AetherAgent.Application.Interfaces;
using AetherAgent.Domain.Entities;

namespace AetherAgent.Infrastructure.Persistence.Repositories;

public sealed class AppUserRepository : IAppUserRepository
{
    private readonly AetherDbContext _db;
    public AppUserRepository(AetherDbContext db) => _db = db;

    public Task<AppUser?> GetByUsernameAsync(string username, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<AppUser?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task AddAsync(AppUser user, CancellationToken ct = default)
        => throw new NotImplementedException();

    public void Update(AppUser user)
        => throw new NotImplementedException();
}
