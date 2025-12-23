using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Defines server options retrieval. Usage example: McpServerOptions options = item.Options().
/// </summary>
internal interface IOptionsSet
{
    /// <summary>
    /// Returns server options. Usage example: McpServerOptions options = item.Options().
    /// </summary>
    McpServerOptions Options();
}

/// <summary>
/// Provides MCP server options. Usage example: IOptionsSet options = new OptionsSet(info, caps, hooks).
/// </summary>
internal sealed class OptionsSet : IOptionsSet
{
    private readonly IServerInfo _info;
    private readonly ICapabilitiesSet _caps;
    private readonly IHooksSet _hooks;

    /// <summary>
    /// Creates options wrapper. Usage example: IOptionsSet options = new OptionsSet(info, caps, hooks).
    /// </summary>
    /// <param name="info">Server info.</param>
    /// <param name="caps">Capabilities.</param>
    /// <param name="hooks">Hooks.</param>
    public OptionsSet(IServerInfo info, ICapabilitiesSet caps, IHooksSet hooks)
    {
        _info = info;
        _caps = caps;
        _hooks = hooks;
    }

    /// <summary>
    /// Returns server options. Usage example: McpServerOptions options = item.Options().
    /// </summary>
    public McpServerOptions Options() => new() { ServerInfo = _info.Info(), Capabilities = _caps.Capabilities(), Handlers = _hooks.Hooks() };
}
