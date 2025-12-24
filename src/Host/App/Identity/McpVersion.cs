using System.Diagnostics;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Provides MCP version from file metadata. Usage example: IMcpVersion version = new McpVersion(new ProcessPath()).
/// </summary>
internal sealed class McpVersion : IMcpVersion
{
    private readonly IProcessPath _path;

    /// <summary>
    /// Creates MCP version wrapper. Usage example: IMcpVersion version = new McpVersion(new ProcessPath()).
    /// </summary>
    /// <param name="path">Process path source.</param>
    public McpVersion(IProcessPath path)
    {
        _path = path;
    }

    /// <summary>
    /// Returns MCP version. Usage example: string version = item.Version().
    /// </summary>
    public string Version()
    {
        string[] args = Environment.GetCommandLineArgs();
        string path = args.Length > 0 ? args[0] : string.Empty;
        if (path.Length > 0 && !Path.IsPathRooted(path))
        {
            path = Path.Combine(AppContext.BaseDirectory, path);
        }
        if (!File.Exists(path))
        {
            path = _path.Path();
        }
        if (!File.Exists(path))
        {
            return "0.0.0.0";
        }
        FileVersionInfo info = FileVersionInfo.GetVersionInfo(path);
        string text = info.ProductVersion ?? string.Empty;
        if (text.Length == 0 || text == "0.0.0.0")
        {
            text = info.FileVersion ?? string.Empty;
        }
        return text.Length == 0 ? "0.0.0.0" : text;
    }
}
