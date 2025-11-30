using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

using System.Runtime.CompilerServices;

/// <summary>
/// Captures router connection attempts without network traffic. Usage example: new RouterSocketProbe().
/// </summary>
internal sealed class RouterSocketProbe : IRouterSocket
{
    private readonly TaskCompletionSource<Uri> endpoint;

    /// <summary>
    /// Initializes the probe. Usage example: new RouterSocketProbe().
    /// </summary>
    public RouterSocketProbe()
    {
        endpoint = new(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    /// <summary>
    /// Gets the endpoint provided to Connect. Usage example: var uri = probe.Endpoint.
    /// </summary>
    public Uri Endpoint => endpoint.Task.IsCompleted ? endpoint.Task.Result : new Uri("about:blank");

    /// <summary>
    /// Records the connection endpoint. Usage example: await probe.Connect(uri, token).
    /// </summary>
    public Task Connect(Uri endpoint, CancellationToken cancellationToken) => endpoint is null ? throw new ArgumentNullException(nameof(endpoint)) : Try(endpoint);

    /// <summary>
    /// Ignores outbound payloads. Usage example: await probe.Send(text, token).
    /// </summary>
    public Task Send(string payload, CancellationToken cancellationToken) => payload is null ? throw new ArgumentNullException(nameof(payload)) : Task.CompletedTask;

    /// <summary>
    /// Yields no inbound messages. Usage example: await foreach (var _ in probe.Messages(token)) { }.
    /// </summary>
    public async IAsyncEnumerable<string> Messages([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        yield break;
    }

    /// <summary>
    /// Simulates close operation. Usage example: await probe.Close(token).
    /// </summary>
    public Task Close(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary>
    /// Disposes the probe. Usage example: await probe.DisposeAsync().
    /// </summary>
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    /// <summary>
    /// Completes endpoint recording. Usage example: await Try(uri).
    /// </summary>
    private Task Try(Uri endpoint)
    {
        this.endpoint.TrySetResult(endpoint);
        return Task.CompletedTask;
    }
}
