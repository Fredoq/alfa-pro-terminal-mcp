namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

using System.Globalization;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// Represents a source of ephemeral TCP ports for tests. Usage example: int value = new Port(IPAddress.Loopback).Value();
/// </summary>
internal sealed class Port : IPort
{
    private readonly IPAddress host;

    /// <summary>
    /// Creates a port source bound to a host address. Usage example: new Port(IPAddress.Loopback).
    /// </summary>
    public Port(IPAddress host)
    {
        this.host = host;
    }

    /// <summary>
    /// Provides an available TCP port value. Usage example: int value = port.Value();
    /// </summary>
    public int Value()
    {
        using TcpListener listener = new(host, 0);
        listener.Start();
        string text = listener.LocalEndpoint.ToString() ?? string.Empty;
        listener.Stop();
        int index = text.LastIndexOf(':');
        if (index < 0)
        {
            throw new InvalidOperationException("Port value cannot be resolved from endpoint");
        }
        string value = text[(index + 1)..];
        int port = int.Parse(value, CultureInfo.InvariantCulture);
        return port;
    }
}

/// <summary>
/// Exposes a port value. Usage example: int value = port.Value();
/// </summary>
internal interface IPort
{
    /// <summary>
    /// Provides an available TCP port value. Usage example: int value = port.Value();
    /// </summary>
    int Value();
}
