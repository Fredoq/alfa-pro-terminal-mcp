using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

using System.Runtime.CompilerServices;

/// <summary>
/// Captures payloads sent through IRouterSocket. Usage example: new RouterSocketSpy().
/// </summary>
internal sealed class RouterSocketSpy : IRouterSocket
{
    private readonly TaskCompletionSource<string> payload;

    /// <summary>
    /// Initializes the spy with empty payload. Usage example: new RouterSocketSpy().
    /// </summary>
    public RouterSocketSpy()
    {
        payload = new(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    /// <summary>
    /// Gets the last payload sent. Usage example: var payload = spy.Payload.
    /// </summary>
    public string Payload => payload.Task.IsCompleted ? payload.Task.Result : string.Empty;

    /// <summary>
    /// Simulates router connection. Usage example: await spy.Connect(uri, token).
    /// </summary>
    public Task Connect(Uri endpoint, CancellationToken cancellationToken) => endpoint is null ? throw new ArgumentNullException(nameof(endpoint)) : Task.CompletedTask;

    /// <summary>
    /// Records outbound payload. Usage example: await spy.Send("text", token).
    /// </summary>
    public Task Send(string payload, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(payload);
        this.payload.TrySetResult(payload);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Provides an empty stream of messages. Usage example: await foreach (var _ in spy.Messages(token)) { }.
    /// </summary>
    public async IAsyncEnumerable<string> Messages([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        yield break;
    }

    /// <summary>
    /// Simulates close operation. Usage example: await spy.Close(token).
    /// </summary>
    public Task Close(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary>
    /// Disposes the spy. Usage example: await spy.DisposeAsync().
    /// </summary>
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
