using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Provides MCP server options. Usage example: IOptionsSet options = new OptionsSet(info, hooks).
/// </summary>
internal sealed class OptionsSet : IOptionsSet
{
    private readonly IServerInfo _info;
    private readonly IHooksSet _hooks;

    /// <summary>
    /// Creates options wrapper. Usage example: IOptionsSet options = new OptionsSet(info, hooks).
    /// </summary>
    /// <param name="info">Server info.</param>
    /// <param name="hooks">Hooks.</param>
    public OptionsSet(IServerInfo info, IHooksSet hooks)
    {
        _info = info;
        _hooks = hooks;
    }

    /// <summary>
    /// Returns server options. Usage example: McpServerOptions options = item.Options().
    /// </summary>
    public McpServerOptions Options() => new() { ServerInfo = _info.Info(), Capabilities = new ServerCapabilities { Tools = new ToolsCapability() }, Handlers = _hooks.Hooks() };
}
