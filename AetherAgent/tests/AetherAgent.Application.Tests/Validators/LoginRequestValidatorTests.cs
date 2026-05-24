using AetherAgent.Application.DTOs.Auth;
using AetherAgent.Application.Validators;
using FluentValidation.TestHelper;

namespace AetherAgent.Application.Tests.Validators;

public class LoginRequestValidatorTests
{
    private readonly LoginRequestValidator _sut = new();

    [Fact]
    public void Username_empty_should_fail()
    {
        var result = _sut.TestValidate(new LoginRequest("", "password123"));
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Password_shorter_than_8_should_fail()
    {
        var result = _sut.TestValidate(new LoginRequest("alice", "short"));
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Username_exceeding_64_should_fail()
    {
        var result = _sut.TestValidate(new LoginRequest(new string('a', 65), "password123"));
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Valid_payload_should_pass()
    {
        var result = _sut.TestValidate(new LoginRequest("alice", "password123"));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
