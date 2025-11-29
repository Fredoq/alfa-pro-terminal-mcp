using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

using System.Runtime.CompilerServices;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain;

/// <summary>
/// Yields a predefined sequence of router messages. Usage example: new RouterSocketSequence(list).
/// </summary>
internal sealed class RouterSocketSequence : IRouterSocket
{
    private readonly Queue<string> messages;

    /// <summary>
    /// Initializes the sequence with messages. Usage example: new RouterSocketSequence(list).
    /// </summary>
    public RouterSocketSequence(IEnumerable<string> messages)
    {
        ArgumentNullException.ThrowIfNull(messages);
        this.messages = new Queue<string>(messages);
    }

    /// <summary>
    /// Simulates router connection. Usage example: await socket.Connect(uri, token).
    /// </summary>
    public Task Connect(Uri endpoint, CancellationToken cancellationToken) => endpoint is null ? throw new ArgumentNullException(nameof(endpoint)) : Task.CompletedTask;

    /// <summary>
    /// Ignores outbound payloads. Usage example: await socket.Send(text, token).
    /// </summary>
    public Task Send(string payload, CancellationToken cancellationToken) => payload is null ? throw new ArgumentNullException(nameof(payload)) : Task.CompletedTask;

    /// <summary>
    /// Streams predefined messages with brief delays. Usage example: await foreach (var message in socket.Messages(token)) { }.
    /// </summary>
    public async IAsyncEnumerable<string> Messages([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (messages.Count > 0 && !cancellationToken.IsCancellationRequested)
        {
            string message = messages.Dequeue();
            await Task.Delay(TimeSpan.FromMilliseconds(25), cancellationToken);
            yield return message;
        }
    }

    /// <summary>
    /// Simulates closure of router connection. Usage example: await socket.Close(token).
    /// </summary>
    public Task Close(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary>
    /// Disposes the sequence. Usage example: await socket.DisposeAsync().
    /// </summary>
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
