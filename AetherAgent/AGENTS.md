markdown# AGENTS.md — AetherAgent

# Version: 1.0.0 | Updated: 2026-05

# This is the first file every agent reads at the start of each session.

## 1. PROJECT OVERVIEW

Name: AetherAgent — Omnichannel AI Backend
Type: ASP.NET Core 8 REST API + SignalR + Background Services
Domain: AI-powered customer engagement across 5 platforms
Platforms: Zalo, Facebook, TikTok, Instagram, YouTube
Stage: Sprint 1 — Foundation
Arch: Clean Architecture (Domain → Application → Infrastructure → API)

## 2. TECH STACK (DO NOT change without RFC)

Runtime: .NET 8, C# 12 (Nullable + ImplicitUsings enabled on all projects)
API: ASP.NET Core 8 (Microsoft.AspNetCore.OpenApi 8.0.0)
API Docs: Swashbuckle.AspNetCore 8.0.0 (Swagger UI)
ORM: EF Core 8.0.0 (Microsoft.EntityFrameworkCore + .SqlServer + .Tools) + SQL Server 2022 (Windows Auth, local)
Logging: Serilog 4.3.1 (Infrastructure) + Serilog.AspNetCore 8.0.0 (API) + Serilog.Sinks.File 5.0.0
Logging Abstractions: Microsoft.Extensions.Logging[.Abstractions] 10.0.8 (Application)
Messaging: RabbitMQ.Client 6.8.1 (installed; producers/consumers Sprint 2)
Realtime: ASP.NET Core SignalR (in-memory) → Redis backplane (before staging)
Auth: Microsoft.AspNetCore.Authentication.JwtBearer 8.0.0 (API) + BCrypt.Net-Next 4.0.3 (Infrastructure)
Validation: FluentValidation 11.9.0 + FluentValidation.DependencyInjectionExtensions (Application)
Testing: xUnit 2.9.0 + FluentAssertions 6.12.0 + Moq 4.20.70 + Microsoft.NET.Test.Sdk 17.10.0
         + Microsoft.AspNetCore.Mvc.Testing 8.0.0 (API integration) + FluentValidation.TestHelper
Observability: OpenTelemetry SDK 1.8.x — PLANNED, not yet installed
Resilience: Polly v8 — PLANNED (Sprint 2), not yet installed
Scheduling: Hangfire 1.8.x — PLANNED (Sprint 2), not yet installed

## 3. SOLUTION STRUCTURE

AetherAgent.slnx                      # solution file at repo root (.slnx, not .sln)
├── AetherAgent/                      # API project — assembly: AetherAgent.API
│   ├── AetherAgent.API.csproj        # refs Application + Infrastructure
│   ├── Program.cs
│   ├── Controllers/
│   │   └── Webhooks/                 # 5 platform webhook stubs (Zalo, FB, TikTok, IG, YT)
│   ├── Hubs/                         # SignalR hubs
│   ├── Middleware/                   # TraceId, error handling
│   ├── Extensions/                   # DI / service registration
│   ├── Properties/
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── AetherAgent.http
├── AetherAgent.Application/          # refs Domain only
│   ├── DTOs/
│   │   └── Auth/                     # LoginRequest, TokenResponse, RefreshTokenRequest, RevokeTokenRequest
│   ├── Interfaces/                   # ITokenService, IPasswordHasher, IAppUserRepository, IRefreshTokenRepository, IUnitOfWork
│   ├── Services/
│   ├── UseCases/
│   │   └── Auth/                     # LoginUseCase, RefreshTokenUseCase, RevokeTokenUseCase
│   └── Validators/                   # FluentValidation rules (LoginRequestValidator)
├── AetherAgent.Domain/               # zero external deps — entities sẽ tự tạo
│   ├── Common/                       # (sẽ chứa BaseEntity, IDomainEvent)
│   ├── Entities/                     # (sẽ chứa AppUser, RefreshToken)
│   ├── Enums/                        # (sẽ chứa UserRole)
│   ├── Events/                       # (sẽ chứa UserLoggedInEvent, RefreshTokenRevokedEvent)
│   └── Interfaces/
├── AetherAgent.Infrastructure/       # refs Application (implements Domain/App interfaces)
│   ├── Auth/                         # JwtTokenService, BcryptPasswordHasher, JwtOptions
│   ├── Persistence/
│   │   ├── AetherDbContext.cs        # global soft-delete filter (excl. RefreshToken)
│   │   ├── Configurations/           # AppUserConfiguration, RefreshTokenConfiguration
│   │   └── Repositories/             # AppUserRepository, RefreshTokenRepository
│   ├── Messaging/                    # RabbitMQ publisher/consumer (Sprint 2)
│   ├── BackgroundJobs/               # Hangfire jobs (Sprint 2)
│   ├── SignalR/                      # hub services / backplane wiring
│   └── Observability/                # Serilog + OTel wiring
├── tests/                            # xUnit + FluentAssertions + Moq
│   ├── AetherAgent.Domain.Tests/
│   ├── AetherAgent.Application.Tests/   # LoginRequestValidatorTests, LoginUseCaseTests (skipped)
│   └── AetherAgent.API.Tests/           # WebApplicationFactory<Program> integration
├── AGENTS.md
├── CLAUDE.md
└── CONSTITUTION.md


## 4. LAYER RULES (hard boundary — never cross)

- Domain → imports nothing external
- Application → imports Domain only
- Infrastructure → implements interfaces from Domain/Application
- API → imports Application + Infrastructure

## 5. CODING CONVENTIONS

Namespaces: Match folder (AetherAgent.Domain.Entities)
Files: PascalCase, 1 public type per file
Methods: PascalCase, Async suffix for async (GetByIdAsync)
Interfaces: I prefix (IRepository, IUnitOfWork)
DTOs: Request/Response suffix (CreateMessageRequest, MessageResponse)
Enums: String storage in DB, PascalCase values
Soft delete: IsDeleted + DeletedAt — never hard delete business data
Errors: { errorCode, message, requestId } — never expose stack trace
Logging: ILogger<T> injection only — never static Log.\* in controllers
CancellationToken: propagate through entire async call chain

## 6. FORBIDDEN PATTERNS

- Hardcoded connection strings, API keys, secrets in source code
- var for non-obvious types
- Task.Result or .Wait() — deadlock risk
- Raw SQL outside EF Core
- catch (Exception) without re-throw or structured log
- Adding NuGet packages without approval
- Direct IsDeleted = true assignment — must call entity.SoftDelete()
- Logging raw webhook payload — PII risk

## 7. DEFINITION OF DONE (per task)

- [ ] Compiles with zero warnings
- [ ] Unit tests written and passing
- [ ] No nullable reference warnings
- [ ] XML doc comment on all public methods
- [ ] EARS comment on business rules
- [ ] Constitution self-check passed (read CONSTITUTION.md)

## 8. AGENT PERMISSIONS

Allowed:

- Read/write: AetherAgent/, AetherAgent.Application/, AetherAgent.Domain/, AetherAgent.Infrastructure/, tests/
- Execute: dotnet build, dotnet test, dotnet ef migrations add/update

Requires human confirm before executing:

- Deleting or rolling back migration files
- Changing any production connection string
- Committing directly to main or develop
- Adding a new NuGet package (justify first)

## 9. CURRENT SPRINT CONTEXT

Sprint: 1 — Foundation
Done:
  - EF Core + SQL Server (DbContext skeleton — needs entities)
  - Serilog wired in Infrastructure + API
  - SignalR hub folder + webhook controller folder (5 platforms, stubs pending)
  - JWT auth scaffolding (DTOs, interfaces, use cases, JwtTokenService stub, AuthController stub, AuthExtensions DI)
  - Test projects: Domain.Tests, Application.Tests (LoginRequestValidator có test), API.Tests
Pending (Sprint 1 close):
  - User tạo Domain entities: AppUser, RefreshToken, BaseEntity, IDomainEvent, UserRole, 2 domain events
  - Implement bodies cho JwtTokenService, BcryptPasswordHasher, repositories, use cases, controller
  - Wire AddDbContext + AddAetherAuth + UseAuthentication trong Program.cs
  - HMAC-SHA256 webhook validation (SEC-02 violation — fix before Sprint 2)
Blocked: OpenTelemetry, RabbitMQ producers, Polly, Hangfire → Sprint 2
