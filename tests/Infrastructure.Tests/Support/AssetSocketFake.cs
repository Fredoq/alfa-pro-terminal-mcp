using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

using System.Runtime.CompilerServices;
using System.Text.Json;

/// <summary>
/// Simulates router behavior for asset info requests by echoing a crafted response. Usage example: new AssetSocketFake(payload).
/// </summary>
internal sealed class AssetSocketFake : ITerminal
{
    private readonly string responsePayload;
    private readonly TaskCompletionSource<string> requestId;

    /// <summary>
    /// Initializes the fake with response payload. Usage example: new AssetSocketFake(payload).
    /// </summary>
    public AssetSocketFake(string payload)
    {
        ArgumentNullException.ThrowIfNull(payload);
        responsePayload = payload;
        requestId = new(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    /// <summary>
    /// Captures routing request identifier. Usage example: await socket.Send(json, token).
    /// </summary>
    public Task Send(string payload, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(payload);
        using JsonDocument document = JsonDocument.Parse(payload);
        string id = document.RootElement.GetProperty("Id").GetString() ?? string.Empty;
        requestId.TrySetResult(id);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Yields a single response message with captured correlation id. Usage example: await foreach (var m in socket.Messages(token)) { }.
    /// </summary>
    public async IAsyncEnumerable<string> Messages([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        string id = await requestId.Task.WaitAsync(cancellationToken);
        string message = new ResponseText(id, responsePayload, "#Data.Query", "response").Value();
        yield return message;
    }

    /// <summary>
    /// Disposes resources. Usage example: await socket.DisposeAsync().
    /// </summary>
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

}
