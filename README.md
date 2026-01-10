# Alfa PRO Terminal MCP

Model Context Protocol server that exposes the Alfa Investments PRO Terminal API to LLM clients over stdio. The host connects to the local terminal router via WebSocket; keep the Alfa Investments PRO Terminal desktop app running while the MCP server is active.

## Capabilities
- Accounts list and balances
- Positions by account
- Assets info by ids and tickers
- Object types, object groups, and market boards
- Fin info params by instrument
- Archive candles (OHLCV/MPV)

## Quick start
- Restore and test: `dotnet restore` then `dotnet test Alfa.ProTerminal.Mcp.slnx`
- Run locally: `dnx Fredoqw.Alfa.ProTerminal.Mcp@1.0.0 --yes`

## Configuration
- Endpoint and timeout: `Terminal:Endpoint`, `Terminal:Timeout` (env `TERMINAL__ENDPOINT`, `TERMINAL__TIMEOUT`)
- Default endpoint: `ws://127.0.0.1:3366/router/`

## MCP client configuration
- Example (stdio):
  ```toml
  [mcp_servers.pro-terminal]
  type = "stdio"
  command = "dnx"
  args = ["Fredoqw.Alfa.ProTerminal.Mcp@1.0.0", "--yes"]
  ```

## Repository layout
- `src/Host` — MCP stdio host and tool catalog
- `src/Infrastructure` — router WebSocket adapter, schemas, entries
- `src/Domain` — contracts and terminal abstractions
- `tests` — executable specs for messaging, routing, and adapters
- `docs/architecture/overview.md` — Mermaid overview
