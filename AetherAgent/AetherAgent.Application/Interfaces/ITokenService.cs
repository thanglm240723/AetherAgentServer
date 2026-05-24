using System.Security.Claims;
using AetherAgent.Domain.Entities;

namespace AetherAgent.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(AppUser user);
    string GenerateRefreshToken();
    DateTime GetAccessTokenExpiry();
    ClaimsPrincipal? ValidateAccessToken(string token);
}
