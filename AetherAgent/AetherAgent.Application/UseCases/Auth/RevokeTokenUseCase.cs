using AetherAgent.Application.DTOs.Auth;
using AetherAgent.Application.Interfaces;

namespace AetherAgent.Application.UseCases.Auth;

// EARS[WHEN revoke is requested THE SYSTEM SHALL mark token IsRevoked AND raise RefreshTokenRevokedEvent]
public sealed class RevokeTokenUseCase
{
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IUnitOfWork _uow;

    public RevokeTokenUseCase(IRefreshTokenRepository refreshTokens, IUnitOfWork uow)
    {
        _refreshTokens = refreshTokens;
        _uow = uow;
    }

    public Task ExecuteAsync(RevokeTokenRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
