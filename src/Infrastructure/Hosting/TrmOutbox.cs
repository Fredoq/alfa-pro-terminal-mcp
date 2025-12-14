using System.Net.WebSockets;
using System.Text;
using System.Threading.Channels;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;

/// <summary>
/// Sends outbound messages to the terminal by draining a channel into a WebSocket.
/// Usage example: IOutbox outbox = new Outbox(socket); Task task = outbox.Pump(CancellationToken.None); await outbox.Send("{\"Command\":\"ping\"}", token); await outbox.Close(token); await task.
/// </summary>
public sealed class TrmOutbox : IOutbox
{
    private readonly WebSocket _socket;
    private readonly Channel<ArraySegment<byte>> _queue;

    /// <summary>
    /// Creates an outbound stream for a given WebSocket.
    /// Usage example: IOutbox outbox = new Outbox(socket).
    /// </summary>
    /// <param name="socket">WebSocket used for sending messages</param>
    public TrmOutbox(WebSocket socket)
    {
        ArgumentNullException.ThrowIfNull(socket);
        _socket = socket;
        _queue = Channel.CreateUnbounded<ArraySegment<byte>>();
    }

    /// <inheritdoc />
    public async Task Send(string payload, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(payload);
        if (payload.Length == 0)
        {
            throw new ArgumentException("Payload is empty", nameof(payload));
        }
        byte[] buffer = Encoding.UTF8.GetBytes(payload);
        await _queue.Writer.WriteAsync(new ArraySegment<byte>(buffer), token);
    }

    /// <inheritdoc />
    public async Task Pump(CancellationToken token)
    {
        await foreach (ArraySegment<byte> segment in _queue.Reader.ReadAllAsync(token))
        {
            await _socket.SendAsync(segment, WebSocketMessageType.Text, true, token);
        }
    }

    /// <inheritdoc />
    public Task Close(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        _queue.Writer.TryComplete();
        return Task.CompletedTask;
    }
}
