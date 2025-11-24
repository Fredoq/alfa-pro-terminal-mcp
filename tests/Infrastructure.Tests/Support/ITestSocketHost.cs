namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

using System.Net.WebSockets;

/// <summary>
/// Describes a lightweight WebSocket endpoint for integration tests. Usage example: await host.Start(token); Uri endpoint = host.Endpoint();.
/// </summary>
public interface ITestSocketHost : IAsyncDisposable
{
    /// <summary>
    /// Starts listening for a connection. Usage example: await host.Start(token).
    /// </summary>
    Task Start(CancellationToken cancellationToken);

    /// <summary>
    /// Returns the WebSocket endpoint URI. Usage example: Uri uri = host.Endpoint().
    /// </summary>
    Uri Endpoint();

    /// <summary>
    /// Sends a payload to the connected client. Usage example: await host.Send("text", token).
    /// </summary>
    Task Send(string payload, CancellationToken cancellationToken);

    /// <summary>
    /// Reads a payload from the connected client. Usage example: string value = await host.Read(token).
    /// </summary>
    Task<string> Read(CancellationToken cancellationToken);

    /// <summary>
    /// Acknowledges a close frame sent by the client. Usage example: WebSocketReceiveResult result = await host.Acknowledge(token).
    /// </summary>
    Task<WebSocketReceiveResult> Acknowledge(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the accepted WebSocket. Usage example: WebSocket socket = await host.Take(token).
    /// </summary>
    Task<WebSocket> Take(CancellationToken cancellationToken);
}
