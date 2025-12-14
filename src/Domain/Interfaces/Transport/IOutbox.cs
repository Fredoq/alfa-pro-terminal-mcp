namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;

/// <summary>
/// Describes an outbound message stream for a terminal connection.
/// Usage example: await outbox.Send("{\"Command\":\"listen\"}", token); Task task = outbox.Pump(CancellationToken.None); await outbox.Close(token); await task;
/// </summary>
public interface IOutbox
{
    /// <summary>
    /// Enqueues an outbound payload for transmission.
    /// Usage example: await outbox.Send("{\"Command\":\"request\"}", token).
    /// </summary>
    Task Send(string payload, CancellationToken token);

    /// <summary>
    /// Pumps queued outbound messages to the terminal.
    /// Usage example: Task task = outbox.Pump(CancellationToken.None).
    /// </summary>
    Task Pump(CancellationToken token);

    /// <summary>
    /// Requests the outbound stream to stop producing new messages.
    /// Usage example: await outbox.Close(token).
    /// </summary>
    Task Close(CancellationToken token);
}
