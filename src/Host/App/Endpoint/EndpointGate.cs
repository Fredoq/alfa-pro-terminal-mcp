using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Creates MCP server instances. Usage example: IEndpointGate item = new EndpointGate(options, factory).
/// </summary>
internal sealed class EndpointGate : IEndpointGate
{
    private readonly IOptionsSet _options;
    private readonly ILoggerFactory _factory;

    /// <summary>
    /// Creates endpoint wrapper. Usage example: IEndpointGate item = new EndpointGate(options, factory).
    /// </summary>
    /// <param name="options">Server options provider.</param>
    /// <param name="factory">Logger factory provider.</param>
    public EndpointGate(IOptionsSet options, ILoggerFactory factory)
    {
        _options = options;
        _factory = factory;
    }

    /// <summary>
    /// Returns MCP server instance. Usage example: McpServer server = item.Endpoint(transport).
    /// </summary>
    public McpServer Endpoint(StdioServerTransport transport) => McpServer.Create(transport, _options.Options(), _factory);
}
