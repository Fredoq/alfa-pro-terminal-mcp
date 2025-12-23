using System.Threading.Tasks;
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
    private readonly IServices _services;

    /// <summary>
    /// Creates MCP session runner. Usage example: IRun run = new EndpointSession(transport, endpoint, token, services).
    /// </summary>
    /// <param name="transport">Transport provider.</param>
    /// <param name="endpoint">Endpoint provider.</param>
    /// <param name="token">Token provider.</param>
    /// <param name="services">Service provider wrapper.</param>
    public EndpointSession(ITransportLink transport, IEndpointGate endpoint, IToken token, IServices services)
    {
        _transport = transport;
        _endpoint = endpoint;
        _token = token;
        _services = services;
    }

    /// <summary>
    /// Runs MCP server session. Usage example: await run.Run().
    /// </summary>
    public async Task Run()
    {
        StdioServerTransport transport = _transport.Transport();
        await using StdioServerTransport frame = transport;
        McpServer server = _endpoint.Endpoint(transport, _services.Provider());
        await using McpServer session = server;
        try
        {
            await server.RunAsync(_token.Token());
        }
        finally
        {
            await _services.Release();
        }
    }
}
