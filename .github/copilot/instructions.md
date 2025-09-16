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
- Use Polish in user-facing README/docs; English in code/comments is OK
- Use dependency injection and minimal APIs (Program.cs)

## Testing

- Prefer integration tests with `WebApplicationFactory<Program>` in `tests/Fiszki.Tests.Integration`
- At minimum add tests for new endpoints (2 cases: happy path + 1 error)

## Style

- Keep patches small and focused; avoid noise formatting
- For new features: include acceptance criteria and usage notes in PR description

## Secrets & config

- Never commit secrets
- Use `appsettings.Development.json` locally; bind to POCOs with `IOptions<T>`
- For OAuth (Google), use env vars or user-secrets; expose settings under `Authentication:Google:*`

## Typical tasks

- Add API endpoint: update `Program.cs`, add DTOs (later: `Shared` project), add tests
- Add UI page/component: create in `src/Fiszki.Client/Pages`, wire to NavMenu if needed, use MudBlazor
- Database: use EF Core with SQLite by default; migrations will be added in later milestones

## Review checklist

- Build passes: `dotnet build`
- Tests pass: `dotnet test`
- No hardcoded secrets; configs documented in README
- Dockerfile still builds
