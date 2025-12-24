using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines transport creation. Usage example: StdioServerTransport transport = item.Transport().
/// </summary>
public interface ITransportLink
{
    /// <summary>
    /// Returns transport instance. Usage example: StdioServerTransport transport = item.Transport().
    /// </summary>
    StdioServerTransport Transport();
}
