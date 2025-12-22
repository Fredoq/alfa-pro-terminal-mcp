using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

using System.Runtime.CompilerServices;
using System.Text.Json;

/// <summary>
/// Simulates router behavior for archive requests by echoing crafted responses. Usage example: new ArchiveSocketFake(payload, true).
/// </summary>
internal sealed class ArchiveSocketFake : ITerminal
{
    private readonly string responsePayload;
    private readonly bool includeHeartbeat;
    private readonly TaskCompletionSource<string> requestId;

    public ArchiveSocketFake(string payload, bool includeHeartbeat)
    {
        ArgumentNullException.ThrowIfNull(payload);
        responsePayload = payload;
        this.includeHeartbeat = includeHeartbeat;
        requestId = new(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public Task Send(string payload, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(payload);
        using JsonDocument document = JsonDocument.Parse(payload);
        string id = document.RootElement.GetProperty("Id").GetString() ?? string.Empty;
        requestId.TrySetResult(id);
        return Task.CompletedTask;
    }

    public async IAsyncEnumerable<string> Messages([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        string id = await requestId.Task.WaitAsync(cancellationToken);
        if (includeHeartbeat)
        {
            yield return "{\"heartbeat\":true}";
        }
        string message = new ResponseText(id, responsePayload, "#Archive.Query", "response").Value();
        yield return message;
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

}
