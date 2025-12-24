using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines hook retrieval. Usage example: McpServerHandlers data = item.Hooks().
/// </summary>
public interface IHooksSet
{
    /// <summary>
    /// Returns hooks. Usage example: McpServerHandlers data = item.Hooks().
    /// </summary>
    McpServerHandlers Hooks();
}
