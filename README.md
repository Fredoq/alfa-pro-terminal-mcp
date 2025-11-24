# Alfa PRO Terminal MCP

Model Context Protocol server for Alfa Investments PRO Terminal running as a console app over stdio with SDK-based containerization.

## Build and run
- Restore, build, and test: `dotnet restore` then `dotnet test Alfa.ProTerminal.Mcp.sln`
- Local stdio run: `dotnet run --project src/Alfa.ProTerminal.Mcp/Alfa.ProTerminal.Mcp.csproj`
- Container publish (SDK): `dotnet publish src/Alfa.ProTerminal.Mcp/Alfa.ProTerminal.Mcp.csproj -c Release /t:PublishContainer -p ContainerRegistry=docker.io`

## Repository layout
- `src/Alfa.ProTerminal.Mcp` — console MCP host composition with SDK container settings
- `tests/Alfa.ProTerminal.Mcp.Tests` — executable specs
- `docs/architecture/overview.md` — Mermaid overview

## Automation
- CI validates restore, build, and tests on pull requests
- Main branch builds and pushes Docker image to Docker Hub
