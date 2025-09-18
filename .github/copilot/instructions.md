# Copilot project instructions

These instructions help Copilot Chat and inline completions produce consistent, high-quality changes in this repo.

## Project context

- App: Fiszki — Blazor (.NET 9) with WebAssembly interactivity
- Structure:
  - Server: `src/Fiszki` (hosts client, minimal APIs)
  - Client: `src/Fiszki.Client` (Blazor WASM UI)
  - Tests: `tests/Fiszki.Tests.Integration`
- Health check: GET `/health` → 200 OK
- CI: GitHub Actions in `.github/workflows/ci.yml`
- Docker: Multi-stage `Dockerfile` builds and runs ASP.NET app on port 8080

## Conventions

- TargetFramework: `net9.0`
- Nullable and implicit usings enabled
- Keep public API changes minimal; prefer additive changes
- Language: Use English for all generated content, documentation, commit messages, PR descriptions, and user-facing text. Code comments may also be in English.
- Shell: Prefer Bash (Linux/WSL) in documentation and examples. Provide Windows PowerShell equivalents only when requested or when essential.
- Use dependency injection and minimal APIs (Program.cs)

## Testing

- Prefer integration tests with `WebApplicationFactory<Program>` in `tests/Fiszki.Tests.Integration`
- At minimum add tests for new endpoints (2 cases: happy path + 1 error)
- Use NSubstitute for mocking/stubbing collaborators
- Use Testcontainers for external dependencies (e.g., databases, message brokers) in integration tests

## Validation

- Use FluentValidation for input/request/DTO validation when validation is required
- Register validators via DI (e.g., `services.AddValidatorsFromAssemblyContaining<Program>()`)
- In minimal APIs, inject `IValidator<T>` and return `Results.ValidationProblem(...)` or 400 with errors on failure
- Add at least one integration test for invalid input to assert proper 400 responses and error payload shape

## Style

- Keep patches small and focused; avoid noise formatting
- For new features: include acceptance criteria and usage notes in PR description (in English)

## Secrets & config

- Never commit secrets
- Use `appsettings.Development.json` locally; bind to POCOs with `IOptions<T>`
- For OAuth (Google), use env vars or user-secrets; expose settings under `Authentication:Google:*`

## Local notes & setup guides

- Store developer-only technical notes and setup guides under `/Local` (root-level) or append to existing files there.
- The `/Local` directory is ignored by Git (see `.gitignore`) so files are visible in Explorer but never committed.
- Examples:
  - `/Local/TECH_NOTES.md` — architecture/auth/JWT notes
  - `/Local/GOOGLE_OAUTH_SETUP.md` — Google OAuth step-by-step
  - Prefer English for documentation; avoid secrets in committed files. Use user-secrets or env vars.

## Typical tasks

- Add API endpoint: update `Program.cs`, add DTOs (later: `Shared` project), add tests
- Add UI page/component: create in `src/Fiszki.Client/Pages`, wire to NavMenu if needed, use MudBlazor
- Database: use EF Core with SQLite by default; migrations will be added in later milestones

## Review checklist

- Build passes: `dotnet build`
- Tests pass: `dotnet test`
- No hardcoded secrets; configs documented in README
- Dockerfile still builds

## Maintenance & updates

Whenever you add or change something in the project that affects these instructions (this file), update this file in the same pull request. Examples include:

- New testing conventions or frameworks
- Changes to project structure or paths
- New CI/CD workflows or important changes to existing ones
- Updates to language/shell conventions or coding standards
- New typical tasks or review checklist items

PR checklist additions:

- [ ] I reviewed `.github/copilot/instructions.md` and updated it if needed
- [ ] README and docs reflect the latest conventions
