using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Creates stdio transport. Usage example: ITransportLink item = new TransportLink(name, factory).
/// </summary>
internal sealed class TransportLink : ITransportLink
{
    private readonly IServerName _name;
    private readonly ILoggerFactory _factory;

    /// <summary>
    /// Creates transport wrapper. Usage example: ITransportLink item = new TransportLink(name, journal).
    /// </summary>
    /// <param name="name">Server name.</param>
    /// <param name="journal">Journal instance.</param>
    public TransportLink(IServerName name, ILog journal) : this(name, journal.Factory())
    {
    }

    /// <summary>
    /// Creates transport wrapper. Usage example: ITransportLink item = new TransportLink(name, factory).
    /// </summary>
    /// <param name="name">Server name.</param>
    /// <param name="factory">Logger factory.</param>
    public TransportLink(IServerName name, ILoggerFactory factory)
    {
        _name = name;
        _factory = factory;
    }

    /// <summary>
    /// Returns transport instance. Usage example: StdioServerTransport transport = item.Transport().
    /// </summary>
    public StdioServerTransport Transport() => new(_name.Name(), _factory);
}
