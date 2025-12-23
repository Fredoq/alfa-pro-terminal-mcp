using System.Diagnostics;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Defines server name retrieval. Usage example: string name = item.Name().
/// </summary>
internal interface IServerName
{
    /// <summary>
    /// Returns server name. Usage example: string name = item.Name().
    /// </summary>
    string Name();
}

/// <summary>
/// Provides server name value. Usage example: IServerName name = new ServerName("alfa-pro-terminal-mcp").
/// </summary>
internal sealed class ServerName : IServerName
{
    private readonly string _name;

    /// <summary>
    /// Creates server name wrapper. Usage example: IServerName name = new ServerName("alfa-pro-terminal-mcp").
    /// </summary>
    /// <param name="name">Server name value.</param>
    public ServerName(string name)
    {
        _name = name;
    }

    /// <summary>
    /// Returns server name. Usage example: string name = item.Name().
    /// </summary>
    public string Name()
    {
        string name = _name;
        if (name.Length == 0)
        {
            throw new InvalidOperationException("Server name is missing");
        }
        return name;
    }
}

/// <summary>
/// Defines application title retrieval. Usage example: string title = item.Title().
/// </summary>
internal interface IApplicationTitle
{
    /// <summary>
    /// Returns application title. Usage example: string title = item.Title().
    /// </summary>
    string Title();
}

/// <summary>
/// Provides application title value. Usage example: IApplicationTitle title = new ApplicationTitle("Alfa Pro Terminal MCP").
/// </summary>
internal sealed class ApplicationTitle : IApplicationTitle
{
    private readonly string _title;

    /// <summary>
    /// Creates application title wrapper. Usage example: IApplicationTitle title = new ApplicationTitle("Alfa Pro Terminal MCP").
    /// </summary>
    /// <param name="title">Title value.</param>
    public ApplicationTitle(string title)
    {
        _title = title;
    }

    /// <summary>
    /// Returns application title. Usage example: string title = item.Title().
    /// </summary>
    public string Title()
    {
        string title = _title;
        if (title.Length == 0)
        {
            throw new InvalidOperationException("Server title is missing");
        }
        return title;
    }
}

/// <summary>
/// Defines environment name retrieval. Usage example: string name = item.Name().
/// </summary>
internal interface IEnvironmentName
{
    /// <summary>
    /// Returns environment name. Usage example: string name = item.Name().
    /// </summary>
    string Name();
}

/// <summary>
/// Provides environment name from variables. Usage example: IEnvironmentName name = new EnvironmentName("DOTNET_ENVIRONMENT", "ASPNETCORE_ENVIRONMENT", "Production").
/// </summary>
internal sealed class EnvironmentName : IEnvironmentName
{
    private readonly string _key;
    private readonly string _alias;
    private readonly string _fallback;

    /// <summary>
    /// Creates environment name wrapper. Usage example: IEnvironmentName name = new EnvironmentName("DOTNET_ENVIRONMENT", "ASPNETCORE_ENVIRONMENT", "Production").
    /// </summary>
    /// <param name="key">Primary environment key.</param>
    /// <param name="alias">Secondary environment key.</param>
    /// <param name="fallback">Fallback value.</param>
    public EnvironmentName(string key, string alias, string fallback)
    {
        _key = key;
        _alias = alias;
        _fallback = fallback;
    }

    /// <summary>
    /// Returns environment name. Usage example: string name = item.Name().
    /// </summary>
    public string Name()
    {
        string text = Environment.GetEnvironmentVariable(_key) ?? string.Empty;
        if (text.Length > 0)
        {
            return text;
        }
        text = Environment.GetEnvironmentVariable(_alias) ?? string.Empty;
        if (text.Length > 0)
        {
            return text;
        }
        if (_fallback.Length == 0)
        {
            throw new InvalidOperationException("Environment name is missing");
        }
        return _fallback;
    }
}

/// <summary>
/// Defines base path retrieval. Usage example: string path = item.Path().
/// </summary>
internal interface IBasePath
{
    /// <summary>
    /// Returns base path. Usage example: string path = item.Path().
    /// </summary>
    string Path();
}

/// <summary>
/// Provides base path value. Usage example: IBasePath path = new BasePath().
/// </summary>
internal sealed class BasePath : IBasePath
{
    /// <summary>
    /// Creates base path wrapper. Usage example: IBasePath path = new BasePath().
    /// </summary>
    public BasePath()
    {
    }

    /// <summary>
    /// Returns base path. Usage example: string path = item.Path().
    /// </summary>
    public string Path()
    {
        string path = AppContext.BaseDirectory;
        if (path.Length == 0)
        {
            throw new InvalidOperationException("Base path is missing");
        }
        return path;
    }
}

/// <summary>
/// Defines process path retrieval. Usage example: string path = item.Path().
/// </summary>
internal interface IProcessPath
{
    /// <summary>
    /// Returns process path. Usage example: string path = item.Path().
    /// </summary>
    string Path();
}

/// <summary>
/// Provides process path value. Usage example: IProcessPath path = new ProcessPath().
/// </summary>
internal sealed class ProcessPath : IProcessPath
{
    /// <summary>
    /// Creates process path wrapper. Usage example: IProcessPath path = new ProcessPath().
    /// </summary>
    public ProcessPath()
    {
    }

    /// <summary>
    /// Returns process path. Usage example: string path = item.Path().
    /// </summary>
    public string Path()
    {
        string path = Environment.ProcessPath ?? string.Empty;
        if (path.Length > 0)
        {
            return path;
        }
        path = AppContext.BaseDirectory;
        if (path.Length == 0)
        {
            throw new InvalidOperationException("Process path is missing");
        }
        return path;
    }
}

/// <summary>
/// Defines MCP version retrieval. Usage example: string version = item.Version().
/// </summary>
internal interface IMcpVersion
{
    /// <summary>
    /// Returns MCP version. Usage example: string version = item.Version().
    /// </summary>
    string Version();
}

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
        string path = _path.Path();
        FileVersionInfo info = FileVersionInfo.GetVersionInfo(path);
        string text = info.ProductVersion ?? string.Empty;
        if (text.Length == 0)
        {
            throw new InvalidOperationException("Version is missing");
        }
        return text;
    }
}
