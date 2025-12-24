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
