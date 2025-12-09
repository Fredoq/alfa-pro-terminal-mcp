using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;

/// <summary>
/// Ensures router connection is established at startup. Usage example: registered as hosted service.
/// </summary>
internal sealed class AlfaProTerminal : IHostedService, ITerminal
{
    private readonly IOptions<TerminalOptions> _options;
    private readonly ClientWebSocket _socket;
    private readonly Channel<ArraySegment<byte>> _outbound;

    /// <summary>
    /// Creates the hosted service that connects the router. Usage example: new AlfaProTerminal(options).
    /// </summary>
    /// <param name="options">Terminal options</param>
    public AlfaProTerminal(IOptions<TerminalOptions> options) : this(options, new ClientWebSocket(), Channel.CreateUnbounded<ArraySegment<byte>>())
    {
    }

    /// <summary>
    /// Creates the hosted service with a custom socket. Usage example: new AlfaProTerminal(options, socket).
    /// </summary>
    /// <param name="options">Terminal options</param>
    /// <param name="socket">Client WebSocket instance</param>
    /// <param name="outbound">Outbound message channel</param>
    public AlfaProTerminal(IOptions<TerminalOptions> options, ClientWebSocket socket, Channel<ArraySegment<byte>> outbound)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(socket);
        ArgumentNullException.ThrowIfNull(outbound);
        _options = options;
        _socket = socket;
        _outbound = outbound;
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
    /// Connects to the router when the host starts. Usage example: called by infrastructure.
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!Uri.TryCreate(_options.Value.Endpoint, UriKind.Absolute, out Uri? parsed))
        {
            throw new InvalidOperationException("Terminal endpoint is invalid");
        }
        await _socket.ConnectAsync(parsed, cancellationToken);
        _ = Pump(cancellationToken);
    }

    /// <summary>
    /// Closes terminal connection on shutdown. Usage example: called by infrastructure.
    /// </summary>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _outbound.Writer.TryComplete();
        if (_socket.State == WebSocketState.Open || _socket.State == WebSocketState.CloseReceived)
        {
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "close requested", cancellationToken);
        }
    }

    /// <summary>
    /// Disposes the WebSocket instance. Usage example: called by infrastructure.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        using CancellationTokenSource source = new(_options.Value.Timeout);
        await StopAsync(source.Token);
        _socket.Dispose();
    }

    /// <summary>
    /// Pumps queued outbound messages to the terminal. Usage example: invoked automatically after Connect.
    /// </summary>
    private async Task Pump(CancellationToken cancellationToken)
    {
        await foreach (ArraySegment<byte> segment in _outbound.Reader.ReadAllAsync(cancellationToken))
        {
            await _socket.SendAsync(segment, WebSocketMessageType.Text, true, cancellationToken);
        }
    }
}
