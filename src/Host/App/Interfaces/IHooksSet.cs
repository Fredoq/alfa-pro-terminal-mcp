using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Defines hook retrieval. Usage example: McpServerHandlers data = item.Hooks().
/// </summary>
internal interface IHooksSet
{
    /// <summary>
    /// Returns hooks. Usage example: McpServerHandlers data = item.Hooks().
    /// </summary>
    McpServerHandlers Hooks();
}
