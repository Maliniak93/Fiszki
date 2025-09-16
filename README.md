# Fiszki

A web app for learning with flashcards, built on .NET 9 (Blazor Web App with WebAssembly interactivity). This repository contains the server (host) and the client.

## Key features

- Study flashcards (open-ended and multiple choice)
- Browse list and full CRUD for flashcards
- Organize into decks/topics

## Tech stack

- .NET 9 / ASP.NET Core
- Blazor (Razor Components + WebAssembly interactivity)
- Integration tests with xUnit (`tests/Fiszki.Tests.Integration`)
- Docker (multi-stage build)

## Quick start (Linux/WSL - Bash preferred)

Prerequisites: .NET SDK 9.x and (optionally) Docker.

1. Trust the developer HTTPS certificate (optional for local https):

```bash
# one-time
dotnet dev-certs https --trust
```

2. Restore packages and build:

```bash
# from the repo root
dotnet restore Fiszki.sln
dotnet build Fiszki.sln -c Debug
```

3. Run the app locally:

```bash
# runs the host (server) with interactive WASM components
dotnet run --project src/Fiszki/Fiszki/Fiszki.csproj
```

After startup you’ll see the listening address(es) in the console (typically https://localhost:5xxx). Open the app or check the health endpoint:

```bash
# quick smoke test of the health endpoint
curl -k https://localhost:5xxx/health
```

## Tests

Run all tests (including integration tests):

```bash
dotnet test tests/Fiszki.Tests.Integration/Fiszki.Tests.Integration.csproj -c Debug
```

## Run with Docker

Build the image and run a container. The app listens on port 8080 inside the container (see `Dockerfile`).

```bash
# build image
docker build -t fiszki .

# run (map host port to container port)
docker run --rm -p 8080:8080 fiszki
```

Open http://localhost:8080/ or check http://localhost:8080/health.

## Windows (PowerShell) equivalents

If you prefer Windows PowerShell commands, use:

```powershell
# restore & build
dotnet restore .\Fiszki.sln
dotnet build .\Fiszki.sln -c Debug

# run
dotnet run --project .\src\Fiszki\Fiszki\Fiszki.csproj

# health check (replace 5xxx with actual port)
Invoke-WebRequest https://localhost:5xxx/health -UseBasicParsing | Select-Object StatusCode, Content

# docker
docker build -t fiszki .
docker run --rm -p 8080:8080 fiszki
```

## Configuration

- Server configuration: `src/Fiszki/Fiszki/appsettings.json` and `appsettings.Development.json`.
- Client configuration: `src/Fiszki/Fiszki.Client/wwwroot/appsettings.json` (if used).
- Environment variables:
  - `ASPNETCORE_ENVIRONMENT` (e.g., `Development`)
  - `ASPNETCORE_URLS` (Docker uses `http://+:8080` – set in `Dockerfile`)

## Project structure

- `src/Fiszki/Fiszki` — host (ASP.NET Core + Razor Components), exposes `/health`
- `src/Fiszki/Fiszki.Client` — client-side Blazor components running in the browser
- `tests/Fiszki.Tests.Integration` — integration tests
- `Dockerfile` — multi-stage build for .NET 9
- `Fiszki.sln` — solution file

Example minimal HTTP flow:

- GET `/` — application UI
- GET `/health` — returns 200 OK, e.g., `{ "status": "OK" }`

## Troubleshooting

- HTTPS certificates: if your browser warns about a dev certificate, run `dotnet dev-certs https --trust` and restart the app.
- Port conflicts: if a port is in use, set another (e.g., `ASPNETCORE_URLS=http://localhost:5178`) or let `dotnet run` choose an available port.
- Missing .NET 9 SDK: download from https://dotnet.microsoft.com/download/dotnet/9.0.

## Contributing

PRs and issues are welcome. Keep changes focused, maintain code style, and add tests for behavior changes.

## License

MIT
