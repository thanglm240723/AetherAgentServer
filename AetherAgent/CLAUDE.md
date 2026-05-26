# CLAUDE.md — AetherAgent

# Read AGENTS.md first for full project context.

## BEHAVIORAL GUIDELINES (read every session)

**Tradeoff:** These bias toward caution over speed. For trivial edits (rename, typo, single-line fix) use judgment and skip the ceremony.

### 1. Think before coding — surface tradeoffs, don't hide confusion
- State assumptions explicitly. If uncertain about scope (e.g. "should HMAC validation reject or just log?"), ask before coding.
- If multiple interpretations exist, present them — never pick silently.
- If a simpler approach exists (e.g. EF Core global filter vs. manual `Where(!x.IsDeleted)` everywhere), say so and push back when warranted.
- If something is unclear, stop. Name what's confusing. Ask.

### 2. Simplicity first — minimum code that solves the problem
- No features beyond what was asked. No speculative abstractions, no "flexibility" or config knobs that weren't requested.
- No error handling for impossible scenarios. Validate at boundaries only (webhook input, external APIs) — trust internal Clean Architecture layers.
- No premature interfaces for single-use code. Three similar lines beats a premature abstraction.
- Ask: "Would a senior engineer call this overcomplicated?" If yes, simplify.

### 3. Surgical changes — touch only what the task requires
- Don't "improve" adjacent code, comments, or formatting while editing.
- Don't refactor things that aren't broken. Match existing style even if you'd do it differently.
- Respect §6 FORBIDDEN PATTERNS in AGENTS.md — never bypass with `--no-verify`, never delete migrations, never add NuGet packages without approval.
- Clean up only orphans YOUR change created (unused imports/usings/variables). Mention pre-existing dead code; don't delete it unless asked.
- Test: every changed line should trace directly to the user's request.

### 4. Goal-driven execution — define success, then loop until verified
Transform tasks into verifiable goals:
- "Add HMAC validation" → "Write unit test with known signature, then make it pass"
- "Fix the soft-delete bug" → "Write a test that reproduces it (deleted entity appearing in query), then make it pass"
- "Refactor X" → "`dotnet build` + `dotnet test` green before AND after"

For multi-step tasks, state a brief plan up front:
```
1. [Step] → verify: [check, e.g. dotnet build zero warnings]
2. [Step] → verify: [check, e.g. unit test passes]
3. [Step] → verify: [check, e.g. EARS comment present, Constitution self-check]
```

Strong success criteria → independent looping. Weak criteria ("make it work") → constant clarification churn.

**Working if:** fewer unnecessary diffs, fewer rewrites from overcomplication, questions come *before* implementation — not after mistakes.

## MANUAL MEMORY (human-maintained)

### Architecture Decisions (ADR)

# ADR-001: Clean Architecture — 4 separate projects, dependency flows inward only (API → Application → Domain)

# ADR-002: Soft Delete — BaseEntity.IsDeleted + DeletedAt, EF Core global query filter auto-excludes deleted records

# ADR-003: Enums as string in DB — HasConversion<string>(), human-readable, no magic numbers

# ADR-004: SignalR in-memory for dev/sprint-1 — Redis-B backplane required before staging deploy

# ADR-005: TraceId middleware — propagates across all 7 tiers via Serilog LogContext, response header X-Trace-Id

# ADR-006: Windows Auth for local SQL Server 2022 — no password in connection string for dev

# ADR-007: YARP_APIGateway/ là gateway duy nhất đang dùng (có full middleware).
# AetherAgent.Gateway/ là project placeholder rỗng — không có logic.
# Không viết code vào AetherAgent.Gateway/ cho đến khi có quyết định chính thức (RFC cần thiết).

### Lessons Learned

# LESSON-001: EF Core global query filter for soft delete must be in OnModelCreating, not OnConfiguring

# LESSON-002: Webhook POST endpoints must NOT log raw payload — PII risk (message content, sender name, phone)

# LESSON-003: HMAC-SHA256 validation is a HARD RULE (SEC-02) — implement before any real payload processing

### Current Sprint Notes

# Sprint 1 focus: Base infrastructure + Auth module

# ⚠️ TRẠNG THÁI: Solution hiện KHÔNG compile — Domain project rỗng, nhưng DbContext và
#    nhiều file Infrastructure/Application đã reference AppUser, RefreshToken, BaseEntity.
#    Đây là trạng thái có chủ ý từ scaffold. Ưu tiên cao nhất: tạo Domain entities.

# Done (scaffold): EF Core + Serilog + SignalR + JWT auth stack + HmacValidator (SEC-02) +
#    3 test projects (xUnit/FluentAssertions/Moq)

# Pending: Domain entities → implement stubs → wire Program.cs → EF migration → unskip tests

# Blocked (Sprint 2): OpenTelemetry, RabbitMQ producers, Polly, Hangfire

## PATTERNS TO FOLLOW

# Repository pattern: Domain/Interfaces/IRepository<T> → Infrastructure implements

# UoW pattern: always call IUnitOfWork.SaveChangesAsync() after business operations

# Domain events: entity raises event → dispatch after SaveChanges → publish to RabbitMQ

# Error response: always { errorCode, message, requestId } — never leak stack trace

# Logging: use ILogger<T> injection, never static Serilog.Log.\* in controllers

# EARS comment: annotate every business rule with // EARS[WHEN...THE SYSTEM SHALL...]

# JWT auth: AppUser inherits BaseEntity (soft-delete applies). RefreshToken KHÔNG inherit BaseEntity và KHÔNG có global soft-delete query filter — vì revoked tokens vẫn phải query được. Token rotation bắt buộc trên /auth/refresh: revoke old → issue new pair.

# JWT secret: KHÔNG hardcode trong appsettings.json. Dev: dotnet user-secrets. Prod: env var / Key Vault (SEC-01).

# Test naming: Method_scenario_expectedOutcome. Mỗi UseCase ≥ 3 test (happy + 2 error path). [Fact(Skip = "...")] cho test chưa implement — không để empty test pass giả.

## AUTO MEMORY (Claude Code appends here)
