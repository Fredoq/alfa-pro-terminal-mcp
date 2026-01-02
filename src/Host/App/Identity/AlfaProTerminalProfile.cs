using System.Diagnostics;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Provides application profile values. Usage example: IApplicationProfile profile = new AlfaProTerminalProfile("alfa-pro-terminal-mcp", "Alfa Pro Terminal MCP").
/// </summary>
internal sealed class AlfaProTerminalProfile : IApplicationProfile
{
    private readonly string _name;
    private readonly string _title;

    /// <summary>
    /// Creates application profile wrapper. Usage example: IApplicationProfile profile = new AlfaProTerminalProfile("alfa-pro-terminal-mcp", "Alfa Pro Terminal MCP").
    /// </summary>
    /// <param name="name">Server name.</param>
    /// <param name="title">Application title.</param>
    public AlfaProTerminalProfile(string name, string title)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(title);
        _name = name;
        _title = title;
    }

    /// <summary>
    /// Returns server name. Usage example: string name = item.ServerName().
    /// </summary>
    public string ServerName() => _name;

    /// <summary>
    /// Returns application title. Usage example: string title = item.Title().
    /// </summary>
    public string Title() => _title;

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
