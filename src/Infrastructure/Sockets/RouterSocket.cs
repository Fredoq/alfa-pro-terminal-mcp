using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Sockets;

/// <summary>
/// Maintains a single WebSocket connection to the terminal router. Usage example: var socket = new RouterSocket(); await socket.Connect(new Uri("ws://127.0.0.1:3366/router/"), token); await socket.Send("{\"Command\":\"listen\",\"Channel\":\"#Data.Bus.ObjectEntity\"}", token); await foreach (var message in socket.Messages(token)) { /* process */ }; await socket.Close(token).
/// </summary>
internal sealed class RouterSocket : IRouterSocket
{
    private readonly ClientWebSocket _socket;
    private readonly Channel<ArraySegment<byte>> _outbound;
    private readonly TimeSpan _timeout;

    /// <summary>
    /// Initializes the socket wrapper with outbound buffering. Usage example: var socket = new RouterSocket().
    /// </summary>
    public RouterSocket()
    {
        _socket = new ClientWebSocket();
        _outbound = Channel.CreateUnbounded<ArraySegment<byte>>();
        _timeout = TimeSpan.FromSeconds(5);
    }

    /// <summary>
    /// Opens a connection to the router endpoint. Usage example: await socket.Connect(new Uri("ws://127.0.0.1:3366/router/"), token).
    /// </summary>
    public async Task Connect(Uri endpoint, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        await _socket.ConnectAsync(endpoint, cancellationToken);
        _ = Task.Run(() => Pump(cancellationToken), cancellationToken);
    }

    /// <summary>
    /// Queues a textual payload for sending. Usage example: await socket.Send("{\"Command\":\"request\"}", token).
    /// </summary>
    public async Task Send(string payload, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(payload);
        if (payload.Length == 0)
        {
            throw new ArgumentException("Payload is empty", nameof(payload));
        }

        byte[] buffer = Encoding.UTF8.GetBytes(payload);
        ArraySegment<byte> segment = new(buffer);
        await _outbound.Writer.WriteAsync(segment, cancellationToken);
    }

    /// <summary>
    /// Provides an async stream of messages from the router. Usage example: await foreach (var message in socket.Messages(token)) { /* process */ }.
    /// </summary>
    public async IAsyncEnumerable<string> Messages([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        byte[] buffer = new byte[4096];
        StringBuilder builder = new();
        while (!cancellationToken.IsCancellationRequested)
        {
            WebSocketReceiveResult result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
            builder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
            if (!result.EndOfMessage)
            {
                continue;
            }

            string message = builder.ToString();
            builder.Clear();
            yield return message;
            if (result.CloseStatus.HasValue)
            {
                yield break;
            }
        }
    }

    /// <summary>
    /// Closes the connection gracefully. Usage example: await socket.Close(token).
    /// </summary>
    public async Task Close(CancellationToken cancellationToken)
    {
        _outbound.Writer.TryComplete();
        if (_socket.State == WebSocketState.Open || _socket.State == WebSocketState.CloseReceived)
        {
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "close requested", cancellationToken);
        }
    }

    /// <summary>
    /// Disposes the socket by requesting closure first. Usage example: await socket.DisposeAsync().
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        using CancellationTokenSource source = new(_timeout);
        await Close(source.Token);
        _socket.Dispose();
    }

    /// <summary>
    /// Pumps queued outbound messages to the router. Usage example: invoked automatically after Connect.
    /// </summary>
    private async Task Pump(CancellationToken cancellationToken)
    {
        await foreach (ArraySegment<byte> segment in _outbound.Reader.ReadAllAsync(cancellationToken))
        {
            await _socket.SendAsync(segment, WebSocketMessageType.Text, true, cancellationToken);
        }
    }
}
