namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure;

/// <summary>
/// Verifies RouterSocket behavior against a real WebSocket endpoint. Usage example: executed via xUnit.
/// </summary>
public sealed class RouterSocketTests
{
    /// <summary>
    /// Ensures that RouterSocket connects when the host accepts connections.
    /// </summary>
    [Fact]
    public async Task Router_socket_establishes_connection_when_host_is_available()
    {
        using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(5));
        int port = Pick();
        Uri http = new($"http://127.0.0.1:{port}/ws/");
        await using TestSocketHost host = new(http);
        await host.Start(tokenSource.Token);
        await using RouterSocket socket = new();
        await socket.Connect(host.Endpoint(), tokenSource.Token);
        Task<WebSocketReceiveResult> acknowledgement = host.Acknowledge(tokenSource.Token);
        await socket.Close(tokenSource.Token);
        WebSocketReceiveResult result = await acknowledgement;
        Assert.True(result.MessageType == WebSocketMessageType.Close, "RouterSocket did not establish and close the connection");
    }

    /// <summary>
    /// Ensures that RouterSocket sends payloads to the host.
    /// </summary>
    [Fact]
    public async Task Router_socket_delivers_payload_to_host()
    {
        using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(5));
        int port = Pick();
        Uri http = new($"http://127.0.0.1:{port}/send/");
        await using TestSocketHost host = new(http);
        await host.Start(tokenSource.Token);
        await using RouterSocket socket = new();
        await socket.Connect(host.Endpoint(), tokenSource.Token);
        string payload = $"данные-{Guid.NewGuid()}-🚀";
        await socket.Send(payload, tokenSource.Token);
        string received = await host.Read(tokenSource.Token);
        Task<WebSocketReceiveResult> acknowledgement = host.Acknowledge(tokenSource.Token);
        await socket.Close(tokenSource.Token);
        await acknowledgement;
        Assert.True(received == payload, "Payload did not reach the host");
    }

    /// <summary>
    /// Ensures that RouterSocket yields incoming messages from the host.
    /// </summary>
    [Fact]
    public async Task Router_socket_yields_incoming_messages()
    {
        using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(5));
        int port = Pick();
        Uri http = new($"http://127.0.0.1:{port}/messages/");
        await using TestSocketHost host = new(http);
        await host.Start(tokenSource.Token);
        await using RouterSocket socket = new();
        await socket.Connect(host.Endpoint(), tokenSource.Token);
        string payload = $"ответ-{Guid.NewGuid()}-测试";
        await host.Send(payload, tokenSource.Token);
        await using IAsyncEnumerator<string> enumerator = socket.Messages(tokenSource.Token).GetAsyncEnumerator(tokenSource.Token);
        bool moved = await enumerator.MoveNextAsync();
        string message = moved ? enumerator.Current : string.Empty;
        Task<WebSocketReceiveResult> acknowledgement = host.Acknowledge(tokenSource.Token);
        await socket.Close(tokenSource.Token);
        await acknowledgement;
        Assert.True(moved && message == payload, "RouterSocket did not yield the incoming message");
    }

    /// <summary>
    /// Ensures that RouterSocket yields two delayed messages in order.
    /// </summary>
    [Fact]
    public async Task Router_socket_yields_two_messages_with_delay()
    {
        using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(9));
        int port = Pick();
        Uri http = new($"http://127.0.0.1:{port}/messages-delayed/");
        await using TestSocketHost host = new(http);
        await host.Start(tokenSource.Token);
        await using RouterSocket socket = new();
        await socket.Connect(host.Endpoint(), tokenSource.Token);
        string firstPayload = $"первое-{Guid.NewGuid()}-α";
        string secondPayload = $"второе-{Guid.NewGuid()}-β";
        await host.Send(firstPayload, tokenSource.Token);
        await Task.Delay(TimeSpan.FromSeconds(2), tokenSource.Token);
        await host.Send(secondPayload, tokenSource.Token);
        await using IAsyncEnumerator<string> enumerator = socket.Messages(tokenSource.Token).GetAsyncEnumerator(tokenSource.Token);
        bool firstReceived = await enumerator.MoveNextAsync();
        string firstMessage = firstReceived ? enumerator.Current : string.Empty;
        bool secondReceived = await enumerator.MoveNextAsync();
        string secondMessage = secondReceived ? enumerator.Current : string.Empty;
        Task<WebSocketReceiveResult> acknowledgement = host.Acknowledge(tokenSource.Token);
        await socket.Close(tokenSource.Token);
        await acknowledgement;
        Assert.True(firstReceived && secondReceived && firstMessage == firstPayload && secondMessage == secondPayload, "RouterSocket did not yield two delayed messages in order");
    }

    /// <summary>
    /// Ensures that RouterSocket issues a close frame to the host.
    /// </summary>
    [Fact]
    public async Task Router_socket_sends_close_frame_to_host()
    {
        using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(5));
        int port = Pick();
        Uri http = new($"http://127.0.0.1:{port}/close/");
        await using TestSocketHost host = new(http);
        await host.Start(tokenSource.Token);
        await using RouterSocket socket = new();
        await socket.Connect(host.Endpoint(), tokenSource.Token);
        Task<WebSocketReceiveResult> acknowledgement = host.Acknowledge(tokenSource.Token);
        await socket.Close(tokenSource.Token);
        WebSocketReceiveResult result = await acknowledgement;
        Assert.True(result.MessageType == WebSocketMessageType.Close, "RouterSocket did not send a close frame");
    }

    /// <summary>
    /// Ensures that RouterSocket disposal closes the connection.
    /// </summary>
    [Fact]
    public async Task Router_socket_disposes_and_terminates_connection()
    {
        using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(5));
        int port = Pick();
        Uri http = new($"http://127.0.0.1:{port}/dispose/");
        await using TestSocketHost host = new(http);
        await host.Start(tokenSource.Token);
        await using RouterSocket socket = new();
        await socket.Connect(host.Endpoint(), tokenSource.Token);
        Task<WebSocketReceiveResult> acknowledgement = host.Acknowledge(tokenSource.Token);
        await socket.DisposeAsync();
        WebSocketReceiveResult result = await acknowledgement;
        Assert.True(result.MessageType == WebSocketMessageType.Close, "RouterSocket did not terminate connection on dispose");
    }

    /// <summary>
    /// Generates an available TCP port number.
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
