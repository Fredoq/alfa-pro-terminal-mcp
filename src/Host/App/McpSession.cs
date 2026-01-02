using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

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
    /// <param name="options">Options provider.</param>
    /// <param name="token">Token provider.</param>
    public McpSession(IApplicationProfile profile, ILoggerFactory factory, IOptionsSet options, IToken token)
    {
        _transport = new StdioServerTransport(profile.ServerName(), factory);
        _mcpServer = McpServer.Create(_transport, options.Options(), factory);
        _token = token;
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
