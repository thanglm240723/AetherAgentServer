using AetherAgent.Application.Interfaces;
using AetherAgent.Application.UseCases.Auth;
using Moq;

namespace AetherAgent.Application.Tests.UseCases.Auth;

public class LoginUseCaseTests
{
    private readonly Mock<IAppUserRepository> _users = new();
    private readonly Mock<IRefreshTokenRepository> _refreshTokens = new();
    private readonly Mock<IPasswordHasher> _hasher = new();
    private readonly Mock<ITokenService> _tokens = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private LoginUseCase CreateSut() => new(
        _users.Object, _refreshTokens.Object, _hasher.Object, _tokens.Object, _uow.Object);

    [Fact(Skip = "Pending LoginUseCase implementation + AppUser entity.")]
    public void Valid_credentials_should_issue_tokens_and_raise_event()
    {
        // EARS[WHEN credentials are valid THE SYSTEM SHALL issue access + refresh tokens]
    }

    [Fact(Skip = "Pending LoginUseCase implementation.")]
    public void Invalid_password_should_throw_unauthorized()
    {
    }

    [Fact(Skip = "Pending LoginUseCase implementation.")]
    public void Inactive_user_should_be_rejected()
    {
    }
}
