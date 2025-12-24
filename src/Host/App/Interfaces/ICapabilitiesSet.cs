using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Defines capabilities retrieval. Usage example: ServerCapabilities caps = item.Capabilities().
/// </summary>
internal interface ICapabilitiesSet
{
    /// <summary>
    /// Returns capabilities. Usage example: ServerCapabilities caps = item.Capabilities().
    /// </summary>
    ServerCapabilities Capabilities();
}
