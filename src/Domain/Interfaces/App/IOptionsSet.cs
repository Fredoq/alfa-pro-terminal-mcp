using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines server options retrieval. Usage example: McpServerOptions options = item.Options().
/// </summary>
public interface IOptionsSet
{
    /// <summary>
    /// Returns server options. Usage example: McpServerOptions options = item.Options().
    /// </summary>
    McpServerOptions Options();
}
