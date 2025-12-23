using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Defines server info retrieval. Usage example: Implementation info = item.Info().
/// </summary>
internal interface IServerInfo
{
    /// <summary>
    /// Returns server info. Usage example: Implementation info = item.Info().
    /// </summary>
    Implementation Info();
}

/// <summary>
/// Provides server info values. Usage example: IServerInfo info = new ServerInfo(name, title, version).
/// </summary>
internal sealed class ServerInfo : IServerInfo
{
    private readonly IServerName _name;
    private readonly IApplicationTitle _title;
    private readonly IMcpVersion _version;

    /// <summary>
    /// Creates server info wrapper. Usage example: IServerInfo info = new ServerInfo(name, title, version).
    /// </summary>
    /// <param name="name">Name provider.</param>
    /// <param name="title">Title provider.</param>
    /// <param name="version">Version provider.</param>
    public ServerInfo(IServerName name, IApplicationTitle title, IMcpVersion version)
    {
        _name = name;
        _title = title;
        _version = version;
    }

    /// <summary>
    /// Returns server info. Usage example: Implementation info = item.Info().
    /// </summary>
    public Implementation Info() => new() { Name = _name.Name(), Title = _title.Title(), Version = _version.Version() };
}
