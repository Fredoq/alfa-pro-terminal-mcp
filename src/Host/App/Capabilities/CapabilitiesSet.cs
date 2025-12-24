using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Provides tools capability set. Usage example: ICapabilitiesSet caps = new CapabilitiesSet().
/// </summary>
internal sealed class CapabilitiesSet : ICapabilitiesSet
{
    /// <summary>
    /// Creates capability wrapper. Usage example: ICapabilitiesSet caps = new CapabilitiesSet().
    /// </summary>
    public CapabilitiesSet()
    {
    }

    /// <summary>
    /// Returns capabilities. Usage example: ServerCapabilities caps = item.Capabilities().
    /// </summary>
    public ServerCapabilities Capabilities() => new() { Tools = new ToolsCapability() };
}
