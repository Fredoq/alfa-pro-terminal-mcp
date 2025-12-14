using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;

/// <summary>
/// Ensures router connection is established at startup. Usage example: registered as hosted service.
/// </summary>
public sealed class AlfaProTerminal : IHostedService, ITerminal
{
    private readonly ClientWebSocket _socket;
    private readonly IOutbox _outbox;
    private readonly ITerminalProfile _profile;
    private Task _task;

    /// <summary>
    /// Creates the terminal instance that connects the router. Usage example: new AlfaProTerminal(configuration).
    /// </summary>
    /// <param name="config">Application configuration root</param>
    public AlfaProTerminal(IConfigurationRoot config) : this(new ClientWebSocket(), new TrmProfile((config ?? throw new ArgumentException("Configuration root is null")).GetSection("Terminal")))
    {
    }

    /// <summary>
    /// Creates the terminal instance with a custom socket and profile. Usage example: new AlfaProTerminal(socket, profile).
    /// </summary>
    /// <param name="socket">Client WebSocket instance</param>
    /// <param name="profile">Terminal connection settings</param>
    public AlfaProTerminal(ClientWebSocket socket, ITerminalProfile profile) : this(socket, profile, new TrmOutbox(socket))
    {
    }

    /// <summary>
    /// Creates the terminal instance with a custom socket, profile and outbox. Usage example: new AlfaProTerminal(socket, profile, outbox).
    /// </summary>
    /// <param name="socket">Client WebSocket instance</param>
    /// <param name="profile">Terminal connection settings</param>
    /// <param name="outbox">Terminal message outbox</param>
    public AlfaProTerminal(ClientWebSocket socket, ITerminalProfile profile, IOutbox outbox)
    {
        _socket = socket;
        _outbox = outbox;
        _profile = profile;
        _task = Task.CompletedTask;
    }

    /// <summary>
    /// Queues a textual payload for sending. Usage example: await socket.Send("{\"Command\":\"request\"}", token).
    /// </summary>
    public Task Send(string payload, CancellationToken cancellationToken) => _outbox.Send(payload, cancellationToken);

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
        await _socket.ConnectAsync(_profile.Address(), cancellationToken);
        _task = _outbox.Pump(CancellationToken.None);
    }

    /// <summary>
    /// Closes terminal connection on shutdown. Usage example: called by infrastructure.
    /// </summary>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _outbox.Close(cancellationToken);
        await _task.WaitAsync(cancellationToken);
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
        using CancellationTokenSource source = new(_profile.Duration());
        await StopAsync(source.Token);
        _socket.Dispose();
    }
}
