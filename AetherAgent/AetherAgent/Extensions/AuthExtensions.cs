using System.Text;
using AetherAgent.Application.Interfaces;
using AetherAgent.Application.UseCases.Auth;
using AetherAgent.Domain.Enums;
using AetherAgent.Infrastructure.Auth;
using AetherAgent.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AetherAgent.API.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddAetherAuth(this IServiceCollection services, IConfiguration config)
    {
        // JWT options binding
        services.Configure<JwtOptions>(config.GetSection(JwtOptions.SectionName));
        var jwt = config.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                  ?? throw new InvalidOperationException("Jwt section missing in configuration");

        // Auth services
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        // Use cases
        services.AddScoped<LoginUseCase>();
        services.AddScoped<RefreshTokenUseCase>();
        services.AddScoped<RevokeTokenUseCase>();

        // JWT bearer
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey)),
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdmin", p => p.RequireRole(nameof(UserRole.Admin)))
            .AddPolicy("RequireSaleStaff", p => p.RequireRole(nameof(UserRole.SaleStaff), nameof(UserRole.Admin)));

        return services;
    }
}
