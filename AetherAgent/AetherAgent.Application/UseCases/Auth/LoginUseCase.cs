using AetherAgent.Application.DTOs.Auth;
using AetherAgent.Application.Interfaces;

namespace AetherAgent.Application.UseCases.Auth;

// EARS[WHEN credentials are valid THE SYSTEM SHALL issue access + refresh tokens AND raise UserLoggedInEvent]
public sealed class LoginUseCase
{
    private readonly IAppUserRepository _users;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokens;
    private readonly IUnitOfWork _uow;

    public LoginUseCase(
        IAppUserRepository users,
        IRefreshTokenRepository refreshTokens,
        IPasswordHasher hasher,
        ITokenService tokens,
        IUnitOfWork uow)
    {
        _users = users;
        _refreshTokens = refreshTokens;
        _hasher = hasher;
        _tokens = tokens;
        _uow = uow;
    }

    public Task<TokenResponse> ExecuteAsync(LoginRequest request, string? ipAddress, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
