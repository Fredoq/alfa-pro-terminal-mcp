using System.Diagnostics;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Provides MCP version from file metadata. Usage example: IMcpVersion version = new McpVersion().
/// </summary>
internal sealed class McpVersion : IMcpVersion
{
    /// <summary>
    /// Creates MCP version wrapper. Usage example: IMcpVersion version = new McpVersion().
    /// </summary>
    public McpVersion()
    {
    }

    /// <summary>
    /// Returns MCP version. Usage example: string version = item.Version().
    /// </summary>
    public string Version()
    {
        const string empty = "0.0.0.0";
        string[] args = Environment.GetCommandLineArgs();
        string path = args.Length > 0 ? args[0] : string.Empty;
        if (path.Length > 0 && !Path.IsPathRooted(path))
        {
            path = Path.Combine(AppContext.BaseDirectory, path);
        }
        if (!File.Exists(path))
        {
            path = Environment.ProcessPath ?? string.Empty;
            if (path.Length == 0)
            {
                path = AppContext.BaseDirectory;
            }
        }
        if (path.Length == 0)
        {
            throw new InvalidOperationException("Process path is missing");
        }
        if (!File.Exists(path))
        {
            return empty;
        }
        FileVersionInfo info = FileVersionInfo.GetVersionInfo(path);
        string text = info.ProductVersion ?? string.Empty;
        if (text.Length == 0 || text == empty)
        {
            text = info.FileVersion ?? string.Empty;
        }
        return text.Length == 0 ? empty : text;
    }
}
