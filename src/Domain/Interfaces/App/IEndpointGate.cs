using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines MCP endpoint creation. Usage example: McpServer server = item.Endpoint(transport, services).
/// </summary>
public interface IEndpointGate
{
    /// <summary>
    /// Returns MCP server instance. Usage example: McpServer server = item.Endpoint(transport).
    /// </summary>
    McpServer Endpoint(StdioServerTransport transport);
}
