using System.Security.Claims;
using AetherAgent.Application.Interfaces;
using AetherAgent.Domain.Entities;

namespace AetherAgent.Infrastructure.Auth;

public sealed class JwtTokenService : ITokenService
{
    // Inject IConfiguration / JwtOptions ở đây — không hardcode SecretKey (SEC-01).
    public JwtTokenService() { }

    public string GenerateAccessToken(AppUser user)
        => throw new NotImplementedException();

    public string GenerateRefreshToken()
        => throw new NotImplementedException();

    public DateTime GetAccessTokenExpiry()
        => throw new NotImplementedException();

    public ClaimsPrincipal? ValidateAccessToken(string token)
        => throw new NotImplementedException();
}
