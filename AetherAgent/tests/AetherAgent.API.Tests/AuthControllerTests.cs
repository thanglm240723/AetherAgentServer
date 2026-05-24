using Microsoft.AspNetCore.Mvc.Testing;

namespace AetherAgent.API.Tests;

public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthControllerTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact(Skip = "Pending AuthController implementation + DbContext registration.")]
    public async Task POST_auth_login_with_valid_credentials_returns_200_and_token()
    {
        await Task.CompletedTask;
    }

    [Fact(Skip = "Pending AuthController implementation.")]
    public async Task POST_auth_login_with_invalid_credentials_returns_401()
    {
        await Task.CompletedTask;
    }

    [Fact(Skip = "Pending AuthController implementation.")]
    public async Task POST_auth_refresh_rotates_token_pair()
    {
        await Task.CompletedTask;
    }
}
