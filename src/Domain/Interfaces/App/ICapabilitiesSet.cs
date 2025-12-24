using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines capabilities retrieval. Usage example: ServerCapabilities caps = item.Capabilities().
/// </summary>
public interface ICapabilitiesSet
{
    /// <summary>
    /// Returns capabilities. Usage example: ServerCapabilities caps = item.Capabilities().
    /// </summary>
    ServerCapabilities Capabilities();
}
