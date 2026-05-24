using AetherAgent.Application.DTOs.Auth;
using AetherAgent.Application.UseCases.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AetherAgent.API.Controllers.Auth;

[ApiController]
[Route("auth")]
public sealed class AuthController : ControllerBase
{
    private readonly LoginUseCase _login;
    private readonly RefreshTokenUseCase _refresh;
    private readonly RevokeTokenUseCase _revoke;

    public AuthController(LoginUseCase login, RefreshTokenUseCase refresh, RevokeTokenUseCase revoke)
    {
        _login = login;
        _refresh = refresh;
        _revoke = revoke;
    }

    [HttpPost("login")]
    public Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
        => throw new NotImplementedException();

    [HttpPost("refresh")]
    public Task<ActionResult<TokenResponse>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken ct)
        => throw new NotImplementedException();

    [HttpPost("revoke")]
    public Task<IActionResult> Revoke([FromBody] RevokeTokenRequest request, CancellationToken ct)
        => throw new NotImplementedException();
}
