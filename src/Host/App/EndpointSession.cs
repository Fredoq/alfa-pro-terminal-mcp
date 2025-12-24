using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;
using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Runs MCP server session. Usage example: IRun run = new EndpointSession(transport, endpoint, token, services).
/// </summary>
internal sealed class EndpointSession : IRun
{
    private readonly ITransportLink _transport;
    private readonly IEndpointGate _endpoint;
    private readonly IToken _token;

    /// <summary>
    /// Creates MCP session runner. Usage example: IRun run = new EndpointSession(transport, endpoint, token, services).
    /// </summary>
    /// <param name="transport">Transport provider.</param>
    /// <param name="endpoint">Endpoint provider.</param>
    /// <param name="token">Token provider.</param>
    public EndpointSession(ITransportLink transport, IEndpointGate endpoint, IToken token)
    {
        _transport = transport;
        _endpoint = endpoint;
        _token = token;
    }

    /// <summary>
    /// Runs MCP server session. Usage example: await run.Run().
    /// </summary>
    public async Task Run()
    {
        StdioServerTransport transport = _transport.Transport();
        await using StdioServerTransport frame = transport;
        McpServer server = _endpoint.Endpoint(transport);
        await using McpServer session = server;
        await server.RunAsync(_token.Token());
    }
}
