using AetherAgent.Application.DTOs.Auth;
using AetherAgent.Application.Interfaces;

namespace AetherAgent.Application.UseCases.Auth;

// EARS[WHEN refresh token is presented THE SYSTEM SHALL rotate: revoke old AND issue new pair]
public sealed class RefreshTokenUseCase
{
    private readonly IAppUserRepository _users;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly ITokenService _tokens;
    private readonly IUnitOfWork _uow;

    public RefreshTokenUseCase(
        IAppUserRepository users,
        IRefreshTokenRepository refreshTokens,
        ITokenService tokens,
        IUnitOfWork uow)
    {
        _users = users;
        _refreshTokens = refreshTokens;
        _tokens = tokens;
        _uow = uow;
    }

    public Task<TokenResponse> ExecuteAsync(RefreshTokenRequest request, string? ipAddress, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
