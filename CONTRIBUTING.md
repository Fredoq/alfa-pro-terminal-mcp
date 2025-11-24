# Contributing

- Use feature branches `feat/*`, fixes `fix/*`, documentation `docs/*`; target `main` via pull requests.
- Follow Conventional Commits (e.g., `feat: add mcp host`).
- Keep changes small, with tests and docs updated.
- Run `dotnet restore` and `dotnet test Alfa.ProTerminal.Mcp.sln` before opening a pull request.
- For container output, use SDK publish: `dotnet publish src/Alfa.ProTerminal.Mcp/Alfa.ProTerminal.Mcp.csproj -c Release /t:PublishContainer -p ContainerRegistry=docker.io`.
