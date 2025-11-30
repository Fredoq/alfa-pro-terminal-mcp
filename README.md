# Alfa PRO Terminal MCP

Model Context Protocol server that bridges LLM clients with the Alfa Investments PRO Terminal API (see `Alfa-Investments-Pro-API.pdf`). The host runs over stdio, connects to the terminal router (`Router:Endpoint`, default `ws://127.0.0.1:3366/router/`, override with `ROUTER__ENDPOINT`), and exposes terminal actions as MCP tools. Ensure the Alfa Investments PRO Terminal desktop app is running before starting the MCP server.

## Capabilities
- Serve MCP tools for PRO Terminal data flows such as account snapshots, positions, portfolio metrics, and order pre-checks
- Let LLMs ground reasoning on live terminal data: portfolio diagnostics, ticker lookup, dividend and cash flow checks, risk alerts
- Return JSON envelopes ready for downstream orchestration without custom glue code

## Quick start
- Restore, build, test: `dotnet restore` then `dotnet test Alfa.ProTerminal.Mcp.slnx`
- Run locally: `dnx Fredoqw.Alfa.ProTerminal.Mcp@0.3.0 --yes`
- Router endpoint: default `ws://127.0.0.1:3366/router/`, override with `ROUTER__ENDPOINT`

## MCP client configuration
- Codex CLI (`~/.codex/config.toml`):
  ```toml
  [mcp_servers.pro-terminal]
  type = "stdio"
  command = "dnx"
  args = ["Fredoqw.Alfa.ProTerminal.Mcp@0.3.0", "--yes"]
  ```
- Visual Studio Code MCP:
  ```json
  {
    "servers": {
      "Fredoqw.Alfa.ProTerminal.Mcp": {
        "type": "stdio",
        "command": "dnx",
        "args": ["Fredoqw.Alfa.ProTerminal.Mcp@0.3.0", "--yes"]
      }
    }
  }
  ```

## Example LLM prompts
- “List client accounts with IIA type and summarize exposure by currency”
- “Fetch positions for ticker AAPL, compute dividend yield, and flag concentration risk”
- “Compare portfolio performance against MOEX index and propose a rebalance plan”
- “Simulate order instructions with random lot sizes and validate margin impact”

## Repository layout
- `src/Host` — MCP host wiring, tool discovery, stdio transport
- `src/Infrastructure` — router socket, PRO Terminal integration, options
- `src/Domain` — terminal abstractions and message contracts
- `tests` — executable specs for messaging, routing, and terminal adapters
- `docs/architecture/overview.md` — Mermaid overview
