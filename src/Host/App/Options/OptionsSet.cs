using ModelContextProtocol.Server;
using ModelContextProtocol.Protocol;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Provides MCP server options. Usage example: IOptionsSet options = new OptionsSet(profile, hooks).
/// </summary>
internal sealed class OptionsSet : IOptionsSet
{
    private readonly IApplicationProfile _profile;
    private readonly IHooksSet _hooks;

    /// <summary>
    /// Creates options wrapper. Usage example: IOptionsSet options = new OptionsSet(profile, hooks).
    /// </summary>
    /// <param name="profile">Application profile.</param>
    /// <param name="hooks">Hooks.</param>
    public OptionsSet(IApplicationProfile profile, IHooksSet hooks)
    {
        _profile = profile;
        _hooks = hooks;
    }

    /// <summary>
    /// Returns server options. Usage example: McpServerOptions options = item.Options().
    /// </summary>
    public McpServerOptions Options() => new() { ServerInfo = new Implementation { Name = _profile.ServerName(), Title = _profile.Title(), Version = _profile.Version() }, Capabilities = new ServerCapabilities { Tools = new ToolsCapability() }, Handlers = _hooks.Hooks() };
}
