namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;

/// <summary>
/// Describes a single connection to the terminal router over WebSocket. Usage example: await socket.Connect(new Uri("ws://127.0.0.1:3366/router/"), token); await socket.Send("payload", token); await foreach (var message in socket.Messages(token)) { /* handle */ }.
/// </summary>
public interface ITerminal : IAsyncDisposable
{
    /// <summary>
    /// Enqueues an outbound payload for transmission. Usage example: await socket.Send("{\"Command\":\"listen\"}", token).
    /// </summary>
    Task Send(string payload, CancellationToken cancellationToken);

    /// <summary>
    /// Streams textual messages received from the router. Usage example: await foreach (var message in socket.Messages(token)) { /* handle */ }.
    /// </summary>
    IAsyncEnumerable<string> Messages(CancellationToken cancellationToken);
}
