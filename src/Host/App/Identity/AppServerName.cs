using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Provides server name value. Usage example: IServerName name = new AppServerName("alfa-pro-terminal-mcp").
/// </summary>
internal sealed class AppServerName : IServerName
{
    private readonly string _name;

    /// <summary>
    /// Creates server name wrapper. Usage example: IServerName name = new AppServerName("alfa-pro-terminal-mcp").
    /// </summary>
    /// <param name="name">Server name value.</param>
    public AppServerName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _name = name;
    }

    /// <summary>
    /// Returns server name. Usage example: string name = item.Name().
    /// </summary>
    public string Name() => _name;
}
