using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;
using Microsoft.Extensions.Configuration;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Verifies AlfaProTerminal hosted service behavior when interacting with router endpoint. Usage example: executed by xUnit runner.
/// </summary>
public sealed class AlfaProTerminalTests
{
    /// <summary>
    /// Ensures that AlfaProTerminal connects to host on start and issues close on stop. Usage example: await terminal.StartAsync(token).
    /// </summary>
    [Fact(DisplayName = "AlfaProTerminal connects and closes with host lifecycle")]
    public async Task Given_host_when_started_and_stopped_then_connects_and_closes()
    {
        using CancellationTokenSource source = new(TimeSpan.FromSeconds(5));
        Uri http = new($"http://127.0.0.1:{Pick()}/terminal/");
        await using TestSocketHost host = new(http);
        await host.Start(source.Token);
        Dictionary<string, string?> settings = new() { ["Terminal:Endpoint"] = host.Endpoint().ToString() };
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();
        await using AlfaProTerminal terminal = new(config);
        await terminal.StartAsync(source.Token);
        WebSocket accepted = await host.Take(source.Token);
        bool connected = accepted.State == WebSocketState.Open;
        Task<WebSocketReceiveResult> acknowledgement = host.Acknowledge(source.Token);
        await terminal.StopAsync(source.Token);
        WebSocketReceiveResult result = await acknowledgement;
        bool closed = result.MessageType == WebSocketMessageType.Close;
        Assert.True(connected && closed, "AlfaProTerminal does not connect and close with host lifecycle");
    }

    /// <summary>
    /// Confirms that AlfaProTerminal forwards outbound payloads through pump. Usage example: await terminal.Send(payload, token).
    /// </summary>
    [Fact(DisplayName = "AlfaProTerminal delivers outbound payloads to host")]
    public async Task Given_payload_when_sent_then_reaches_host()
    {
        using CancellationTokenSource source = new(TimeSpan.FromSeconds(5));
        Uri http = new($"http://127.0.0.1:{Pick()}/send/");
        await using TestSocketHost host = new(http);
        await host.Start(source.Token);
        Dictionary<string, string?> settings = new() { ["Terminal:Endpoint"] = host.Endpoint().ToString() };
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();
        await using AlfaProTerminal terminal = new(config);
        await terminal.StartAsync(source.Token);
        string payload = $"данные-{Guid.NewGuid()}-γ";
        await terminal.Send(payload, source.Token);
        string received = await host.Read(source.Token);
        Task<WebSocketReceiveResult> acknowledgement = host.Acknowledge(source.Token);
        await terminal.StopAsync(source.Token);
        await acknowledgement;
        Assert.True(received == payload, "AlfaProTerminal does not deliver outbound payloads");
    }

    /// <summary>
    /// Verifies that pump keeps running after startup token cancellation. Usage example: cancel startup token then send payload.
    /// </summary>
    [Fact(DisplayName = "AlfaProTerminal pump ignores startup cancellation")]
    public async Task Given_cancelled_startup_token_when_sending_then_pump_runs()
    {
        using CancellationTokenSource startup = new(TimeSpan.FromSeconds(5));
        Uri http = new($"http://127.0.0.1:{Pick()}/pump/");
        await using TestSocketHost host = new(http);
        await host.Start(startup.Token);
        Dictionary<string, string?> settings = new() { ["Terminal:Endpoint"] = host.Endpoint().ToString() };
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();
        await using AlfaProTerminal terminal = new(config);
        await terminal.StartAsync(startup.Token);
        await startup.CancelAsync();
        using CancellationTokenSource flow = new(TimeSpan.FromSeconds(5));
        string payload = $"передача-{Guid.NewGuid()}-λ";
        await terminal.Send(payload, flow.Token);
        string received = await host.Read(flow.Token);
        Task<WebSocketReceiveResult> acknowledgement = host.Acknowledge(flow.Token);
        await terminal.StopAsync(flow.Token);
        await acknowledgement;
        Assert.True(received == payload, "AlfaProTerminal cancels pump after startup token");
    }

    /// <summary>
    /// Ensures that AlfaProTerminal yields incoming messages from host. Usage example: await foreach (var message in terminal.Messages(token)) { }.
    /// </summary>
    [Fact(DisplayName = "AlfaProTerminal yields incoming messages from host")]
    public async Task Given_incoming_message_when_streamed_then_returns_value()
    {
        using CancellationTokenSource source = new(TimeSpan.FromSeconds(5));
        Uri http = new($"http://127.0.0.1:{Pick()}/messages/");
        await using TestSocketHost host = new(http);
        await host.Start(source.Token);
        Dictionary<string, string?> settings = new() { ["Terminal:Endpoint"] = host.Endpoint().ToString() };
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();
        await using AlfaProTerminal terminal = new(config);
        await terminal.StartAsync(source.Token);
        string payload = $"ответ-{Guid.NewGuid()}-δ";
        await host.Send(payload, source.Token);
        await using IAsyncEnumerator<string> enumerator = terminal.Messages(source.Token).GetAsyncEnumerator(source.Token);
        bool moved = await enumerator.MoveNextAsync();
        string message = moved ? enumerator.Current : string.Empty;
        Task<WebSocketReceiveResult> acknowledgement = host.Acknowledge(source.Token);
        await terminal.StopAsync(source.Token);
        await acknowledgement;
        Assert.True(moved && message == payload, "AlfaProTerminal does not yield incoming messages");
    }

    /// <summary>
    /// Verifies that AlfaProTerminal rejects invalid endpoint configuration. Usage example: await terminal.StartAsync(token).
    /// </summary>
    [Fact(DisplayName = "AlfaProTerminal throws for invalid endpoint")]
    public async Task Given_invalid_endpoint_when_started_then_throws()
    {
        Dictionary<string, string?> settings = new() { ["Terminal:Endpoint"] = "not-a-uri" };
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();
        await using AlfaProTerminal terminal = new(config);
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await terminal.StartAsync(CancellationToken.None));
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
