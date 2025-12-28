using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Runs MCP server session. Usage example: IRun run = new EndpointSession(name, factory, options, token).
/// </summary>
internal sealed class EndpointSession : IRun
{
    private readonly IServerName _name;
    private readonly ILoggerFactory _factory;
    private readonly IOptionsSet _options;
    private readonly IToken _token;

    /// <summary>
    /// Creates MCP session runner. Usage example: IRun run = new EndpointSession(name, factory, options, token).
    /// </summary>
    /// <param name="name">Server name.</param>
    /// <param name="factory">Logger factory.</param>
    /// <param name="options">Options provider.</param>
    /// <param name="token">Token provider.</param>
    public EndpointSession(IServerName name, ILoggerFactory factory, IOptionsSet options, IToken token)
    {
        _name = name;
        _factory = factory;
        _options = options;
        _token = token;
    }

    /// <summary>
    /// Runs MCP server session. Usage example: await run.Run().
    /// </summary>
    public async Task Run()
    {
        await using StdioServerTransport transport = new(_name.Name(), _factory);
        McpServer server = McpServer.Create(transport, _options.Options(), _factory);
        await using McpServer session = server;
        await server.RunAsync(_token.Token());
    }
}
