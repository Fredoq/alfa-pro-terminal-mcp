namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

using System.Net;
using System.Net.WebSockets;
using System.Text;

/// <summary>
/// Hosts a lightweight WebSocket endpoint for integration tests. Usage example: await using var host = new TestSocketHost(httpUri); await host.Start(token);.
/// </summary>
internal sealed class TestSocketHost : ITestSocketHost
{
    private readonly HttpListener listener;
    private readonly Uri endpoint;
    private readonly string prefix;
    private Task<WebSocket>? accept;

    /// <summary>
    /// Initializes the host with the given HTTP endpoint. Usage example: var host = new TestSocketHost(httpUri).
    /// </summary>
    public TestSocketHost(Uri httpEndpoint)
    {
        ArgumentNullException.ThrowIfNull(httpEndpoint);
        listener = new HttpListener();
        endpoint = new Uri(httpEndpoint.ToString().Replace("http", "ws", StringComparison.OrdinalIgnoreCase));
        prefix = httpEndpoint.ToString();
    }

    /// <summary>
    /// Starts listening and accepting the first WebSocket. Usage example: await host.Start(token).
    /// </summary>
    public Task Start(CancellationToken cancellationToken)
    {
        listener.Prefixes.Add(prefix);
        listener.Start();
        accept = Accept(cancellationToken);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Returns the router-facing WebSocket endpoint. Usage example: Uri uri = host.Endpoint().
    /// </summary>
    public Uri Endpoint() => endpoint;

    /// <summary>
    /// Sends a text message to the connected client. Usage example: await host.Send("text", token).
    /// </summary>
    public async Task Send(string payload, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(payload);
        WebSocket socket = await Take(cancellationToken);
        byte[] buffer = Encoding.UTF8.GetBytes(payload);
        await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationToken);
    }

    /// <summary>
    /// Reads a single text message from the connected client. Usage example: string value = await host.Read(token).
    /// </summary>
    public async Task<string> Read(CancellationToken cancellationToken)
    {
        WebSocket socket = await Take(cancellationToken);
        byte[] buffer = new byte[4096];
        WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
        return Encoding.UTF8.GetString(buffer, 0, result.Count);
    }

    /// <summary>
    /// Awaits the close frame from the client. Usage example: WebSocketReceiveResult result = await host.Acknowledge(token).
    /// </summary>
    public async Task<WebSocketReceiveResult> Acknowledge(CancellationToken cancellationToken)
    {
        WebSocket socket = await Take(cancellationToken);
        byte[] buffer = new byte[2];
        WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "ack", cancellationToken);
        return result;
    }

    /// <summary>
    /// Retrieves the accepted WebSocket. Usage example: WebSocket socket = await host.Take(token).
    /// </summary>
    public async Task<WebSocket> Take(CancellationToken cancellationToken)
    {
        if (accept is null)
        {
            throw new InvalidOperationException("Accept was not started");
        }

        return await accept.WaitAsync(cancellationToken);
    }

    /// <summary>
    /// Disposes the host and closes active sockets. Usage example: await host.DisposeAsync().
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (accept is not null && accept.IsCompletedSuccessfully)
        {
            WebSocket socket = await accept;
            if (socket.State == WebSocketState.Open)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "dispose", CancellationToken.None);
            }

            socket.Dispose();
        }
        if (listener.IsListening)
        {
            listener.Stop();
        }

        listener.Close();
    }

    /// <summary>
    /// Accepts the initial WebSocket connection. Usage example: internal pipeline call.
    /// </summary>
    private async Task<WebSocket> Accept(CancellationToken cancellationToken)
    {
        HttpListenerContext context = await listener.GetContextAsync().WaitAsync(cancellationToken);
        if (!context.Request.IsWebSocketRequest)
        {
            throw new InvalidOperationException("Request is not websocket");
        }

        HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
        return wsContext.WebSocket;
    }
}
