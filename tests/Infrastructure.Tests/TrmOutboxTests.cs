namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

/// <summary>
/// Verifies TrmOutbox drains queued outbound payloads into a WebSocket. Usage example: executed by xUnit runner.
/// </summary>
public sealed class TrmOutboxTests
{
    /// <summary>
    /// Ensures TrmOutbox rejects empty payloads. Usage example: await outbox.Send(string.Empty, token).
    /// </summary>
    [Fact(DisplayName = "TrmOutbox rejects empty payload")]
    public async Task Given_empty_payload_when_sending_then_throws()
    {
        using ClientWebSocket socket = new();
        TrmOutbox outbox = new(socket);
        await Assert.ThrowsAsync<ArgumentException>(async () => await outbox.Send(string.Empty, CancellationToken.None));
    }

    /// <summary>
    /// Confirms TrmOutbox pump completes after Close is requested. Usage example: await outbox.Close(token); await pump.
    /// </summary>
    [Fact(DisplayName = "TrmOutbox pump completes after close")]
    public async Task Given_closed_outbox_when_pumping_then_completes()
    {
        using CancellationTokenSource source = new(TimeSpan.FromSeconds(5));
        using ClientWebSocket socket = new();
        TrmOutbox outbox = new(socket);
        Task pump = outbox.Pump(source.Token);
        await outbox.Close(source.Token);
        await pump.WaitAsync(source.Token);
        Assert.True(pump.IsCompletedSuccessfully, "TrmOutbox does not stop pumping after close");
    }

    /// <summary>
    /// Confirms TrmOutbox delivers payloads to a WebSocket host. Usage example: await outbox.Send(payload, token).
    /// </summary>
    [Fact(DisplayName = "TrmOutbox delivers payloads to host")]
    public async Task Given_payload_when_sent_then_reaches_host()
    {
        using CancellationTokenSource source = new(TimeSpan.FromSeconds(5));
        Uri http = new($"http://127.0.0.1:{Pick()}/outbox/");
        await using TestSocketHost host = new(http);
        await host.Start(source.Token);
        using ClientWebSocket socket = new();
        await socket.ConnectAsync(host.Endpoint(), source.Token);
        TrmOutbox outbox = new(socket);
        Task pump = outbox.Pump(CancellationToken.None);
        string payload = $"передача-{Guid.NewGuid()}-β";
        await outbox.Send(payload, source.Token);
        string received = await host.Read(source.Token);
        await outbox.Close(source.Token);
        await pump.WaitAsync(source.Token);
        Task<WebSocketReceiveResult> ack = host.Acknowledge(source.Token);
        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "close requested", source.Token);
        await ack;
        Assert.True(received == payload, "TrmOutbox does not deliver outbound payloads");
    }

    /// <summary>
    /// Generates an available TCP port number for test hosts.
    /// </summary>
    private static int Pick()
    {
        using TcpListener listener = new(IPAddress.Loopback, 0);
        listener.Start();
        int port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
}
