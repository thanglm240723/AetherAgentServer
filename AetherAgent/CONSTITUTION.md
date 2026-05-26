# CONSTITUTION.md — AetherAgent
# Version: 1.1.0 | Status: LOCKED
# Owner: Tech Lead
# Rule: Mọi thay đổi file này cần RFC + toàn team sign-off.
# Changelog:
#   1.1.0 — Sync TECH STACK / AGENT PERMISSIONS với cấu trúc repo thực tế (slnx ở root, chưa có tests/.sdd, OTel/Polly/Hangfire chưa cài).

═══════════════════════════════════════════════════
LAYER 1: HARD RULES — KHÔNG BAO GIỜ VI PHẠM
═══════════════════════════════════════════════════

## SEC-01: Secrets
THE system SHALL NOT hardcode secrets, API keys, connection strings trong source code.
Dùng: appsettings.json (dev) → User Secrets (local) → Azure Key Vault / env vars (prod).
Enforcement: pre-commit hook quét pattern key=, secret=, password=.

## SEC-02: Webhook Signature Validation
THE system SHALL validate HMAC-SHA256 signature cho mọi webhook request từ 5 platform.
KHÔNG xử lý payload nếu signature không hợp lệ.
Trả về HTTP 403 nếu validation fail.

## SEC-03: Input Validation
THE system SHALL validate tất cả webhook payload trước khi persist.
KHÔNG có raw SQL string concatenation.
Dùng EF Core parameterized queries.

## DATA-01: Soft Delete
THE system SHALL dùng soft delete (IsDeleted=true, DeletedAt) cho tất cả business entities.
Hard delete chỉ được phép cho: temp files, logs > 90 ngày.

## AUDIT-01: Domain Events
THE system SHALL raise domain event cho mọi state change quan trọng.
Events được publish lên RabbitMQ để trace.

═══════════════════════════════════════════════════
LAYER 2: ARCHITECTURAL CONSTRAINTS
═══════════════════════════════════════════════════

## ARCH-01: Clean Architecture Layer Boundary
Dependency flow: API → Application → Domain (không ngược lại).
Infrastructure implement interfaces từ Domain/Application.
Domain KHÔNG được import bất kỳ package infrastructure nào.

## ARCH-02: Async Operations qua RabbitMQ
Operations từ webhook đến AI processing PHẢI async qua RabbitMQ.
KHÔNG có sync HTTP call từ webhook handler đến AI service.
Timeout > 2 giây → background job.

## ARCH-03: Redis Isolation
Redis-A (Hot Cache), Redis-B (SignalR Backplane), Redis-C (Queue Buffer) là 3 instance riêng biệt.
KHÔNG dùng chung một instance. Down một không ảnh hưởng hai instance còn lại.

## ARCH-04: SignalR Backplane
SignalR PHẢI dùng Redis-B backplane khi deploy multi-instance.
Hiện tại: in-memory (single instance dev). Thêm Redis backplane trước khi deploy staging.

═══════════════════════════════════════════════════
LAYER 3: ENGINEERING STANDARDS
═══════════════════════════════════════════════════

## TECH STACK (không được thay đổi tùy ý)
# [INSTALLED] = đã có trong .csproj   [PLANNED] = thuộc kiến trúc mục tiêu, chưa cài
Runtime:     .NET 8, C# 12, Nullable + ImplicitUsings enabled                      [INSTALLED]
API:         ASP.NET Core 8 (Controllers — Webhooks/*); OpenAPI 8.0.0              [INSTALLED]
API Docs:    Swashbuckle.AspNetCore 8.0.0 (Swagger UI)                             [INSTALLED]
ORM:         EF Core 8.0.0 (+ SqlServer + Tools) + SQL Server 2022 (Windows Auth)  [INSTALLED]
Logging:     Serilog 4.3.1 + Serilog.AspNetCore 8.0.0 + Sinks.File 5.0.0           [INSTALLED]
Messaging:   RabbitMQ.Client 6.8.1 (producers/consumers triển khai Sprint 2)       [INSTALLED]
Realtime:    SignalR in-memory → Redis-B backplane trước khi deploy staging       [INSTALLED — backplane PLANNED]
Tracing:     OpenTelemetry SDK 1.8.x (Console dev, Jaeger prod)                    [PLANNED]
Resilience:  Polly v8 (Retry, Circuit Breaker, Timeout)                            [PLANNED — Sprint 2]
Scheduling:  Hangfire 1.8.x (SQL Server storage)                                   [PLANNED — Sprint 2]
Testing:     xUnit + FluentAssertions + Moq                                        [PLANNED — chưa có test project]

## CODING STANDARDS
- Nullable reference types: enabled
- Implicit usings: enabled
- C# version: 12 (latest)
- Không dùng var cho non-obvious types
- Async/await bắt buộc cho mọi I/O operation
- CancellationToken propagate xuyên suốt call chain
- XML doc comment cho mọi public API

## TESTING REQUIREMENTS
# 3 test projects đã tồn tại trong /tests/ (Domain.Tests, Application.Tests, API.Tests).
# Phần lớn test đang [Fact(Skip=...)] do pending implementation. Unskip khi implement xong.
- Unit test coverage ≥ 80% cho Application layer
- Integration test cho mọi API endpoint (happy + error path)
- Không mock DbContext — dùng SQL Server LocalDB
- Definition of Done: tests pass + no linting errors + coverage đạt

## GIT CONVENTIONS
Branch:  feat/{feature} | fix/{issue} | spec/{feature} | chore/{task}
Commit:  [type](scope): description
PR:      Min 1 approval + CI pass trước khi merge

═══════════════════════════════════════════════════
AI AGENT POLICY
═══════════════════════════════════════════════════

## AGENT PERMISSIONS
Được phép:
- Read/write trong các project hiện có:
    AetherAgent/                  (API — assembly AetherAgent.API)
    AetherAgent.Application/
    AetherAgent.Domain/
    AetherAgent.Infrastructure/
- Tạo mới khi cần: tests/ (test projects), .sdd/ (specs/skills/constraints) — báo trước khi tạo
- Execute: dotnet build, dotnet test, dotnet ef migrations add/update
- Git: add, commit (không push, không force, không --no-verify)

Cấm không có human confirm:
- Xóa hoặc rollback migration files
- Thay đổi ConnectionString prod
- Commit vào main/develop trực tiếp
- Thêm NuGet package mới mà không có justification
- Sửa file solution AetherAgent.slnx (cấu trúc project)

## AGENT MUST-DO
- Đọc AGENTS.md + CLAUDE.md trước mỗi session
- Chạy Constitution Self-Check trước khi submit code
- Update plan.md sau mỗi completed step
- Báo cáo khi gặp edge case không có trong spec
- Append lessons/decisions quan trọng vào section AUTO MEMORY của CLAUDE.md sau mỗi session
