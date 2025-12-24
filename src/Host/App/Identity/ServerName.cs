using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

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
