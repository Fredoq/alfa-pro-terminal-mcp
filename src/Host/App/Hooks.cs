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

/// <summary>
/// Provides MCP tool hooks. Usage example: IHooksSet data = new HooksSet(catalog, calls).
/// </summary>
internal sealed class HooksSet : IHooksSet
{
    private readonly ICatalog _catalog;
    private readonly ICalls _calls;

    /// <summary>
    /// Creates hook wrapper. Usage example: IHooksSet data = new HooksSet(catalog, calls).
    /// </summary>
    /// <param name="catalog">Tool catalog.</param>
    /// <param name="calls">Call router.</param>
    public HooksSet(ICatalog catalog, ICalls calls)
    {
        _catalog = catalog;
        _calls = calls;
    }

    /// <summary>
    /// Returns hooks. Usage example: McpServerHandlers data = item.Hooks().
    /// </summary>
    public McpServerHandlers Hooks() => new() { ListToolsHandler = _catalog.Tools, CallToolHandler = _calls.Result };
}
