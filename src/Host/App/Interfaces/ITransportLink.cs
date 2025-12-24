using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Defines transport creation. Usage example: StdioServerTransport transport = item.Transport().
/// </summary>
internal interface ITransportLink
{
    /// <summary>
    /// Returns transport instance. Usage example: StdioServerTransport transport = item.Transport().
    /// </summary>
    StdioServerTransport Transport();
}
