using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Runs MCP server session. Usage example: IRun run = new McpSession(profile, factory, options, token).
/// </summary>
internal sealed class McpSession : IRun
{
    private readonly StdioServerTransport _transport;
    private readonly McpServer _mcpServer;
    private readonly IToken _token;

    /// <summary>
    /// Creates MCP session runner. Usage example: IRun run = new McpSession(profile, factory, options, token).
    /// </summary>
    /// <param name="profile">Application profile.</param>
    /// <param name="factory">Logger factory.</param>
    /// <param name="options">Server options.</param>
    /// <param name="token">Token provider.</param>
    public McpSession(IApplicationProfile profile, ILoggerFactory factory, McpServerOptions options, IToken token)
    {
        _transport = new StdioServerTransport(profile.ServerName(), factory);
        _mcpServer = McpServer.Create(_transport, options, factory);
        _token = token;
    }

    /// <summary>
    /// Creates MCP session runner. Usage example: IRun run = new McpSession(profile, factory, hooks, token).
    /// </summary>
    /// <param name="profile">Application profile.</param>
    /// <param name="factory">Logger factory.</param>
    /// <param name="hooks">Hooks set.</param>
    /// <param name="token">Token provider.</param>
    public McpSession(IApplicationProfile profile, ILoggerFactory factory, IHooksSet hooks, IToken token)
        : this(profile, factory, new McpServerOptions { ServerInfo = new Implementation { Name = profile.ServerName(), Title = profile.Title(), Version = profile.Version() }, Capabilities = new ServerCapabilities { Tools = new ToolsCapability() }, Handlers = hooks.Hooks() }, token)
    {
    }

    /// <summary>
    /// Runs MCP server session. Usage example: await run.Run().
    /// </summary>
    public async Task Run() => await _mcpServer.RunAsync(_token.Token());

    /// <summary>
    /// Disposes MCP server session. Usage example: await run.DisposeAsync().
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await _transport.DisposeAsync();
        await _mcpServer.DisposeAsync();
    }
}
