namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Net;
using System.Net.WebSockets;
using System.Security.Cryptography;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

#pragma warning disable CA1859

/// <summary>
/// Verifies IOutbox contract when used with a concrete implementation. Usage example: IOutbox outbox = new TrmOutbox(socket).
/// </summary>
public sealed class IOutboxTests
{
    private readonly IPort port = new Port(IPAddress.Loopback);

    /// <summary>
    /// Ensures IOutbox supports concurrent Send calls without message loss. Usage example: await Task.WhenAll(sends).
    /// </summary>
    [Fact(DisplayName = "IOutbox delivers all concurrent payloads")]
    public async Task Given_concurrent_payloads_when_sent_then_all_reach_host()
    {
        using CancellationTokenSource source = new(TimeSpan.FromSeconds(5));
        int count = RandomNumberGenerator.GetInt32(2, 12);
        Uri http = new($"http://127.0.0.1:{port.Value()}/outbox-concurrent/");
        await using TestSocketHost host = new(http);
        await host.Start(source.Token);
        using ClientWebSocket socket = new();
        await socket.ConnectAsync(host.Endpoint(), source.Token);
        IOutbox outbox = new TrmOutbox(socket);
        Task pump = outbox.Pump(CancellationToken.None);
        string[] payloads = new string[count];
        for (int i = 0; i < payloads.Length; i++)
        {
            payloads[i] = $"данные-{Guid.NewGuid()}-λ-{i}";
        }
        Task[] sends = new Task[payloads.Length];
        for (int i = 0; i < sends.Length; i++)
        {
            sends[i] = outbox.Send(payloads[i], source.Token);
        }
        await Task.WhenAll(sends);
        HashSet<string> received = new(StringComparer.Ordinal);
        for (int i = 0; i < payloads.Length; i++)
        {
            received.Add(await host.Read(source.Token));
        }
        await outbox.Close(source.Token);
        await pump.WaitAsync(source.Token);
        Task<WebSocketReceiveResult> ack = host.Acknowledge(source.Token);
        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "close requested", source.Token);
        await ack;
        HashSet<string> expected = new(payloads, StringComparer.Ordinal);
        Assert.True(expected.SetEquals(received), "IOutbox does not deliver all concurrent payloads");
    }

}

#pragma warning restore CA1859
