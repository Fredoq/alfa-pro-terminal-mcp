using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

/// <summary>
/// Simulates router behavior for order entry requests by echoing a crafted response. Usage example: new OrderEntrySocketFake(payload).
/// </summary>
[SuppressMessage("Usage", "CA1812:OrderEntrySocketFake is an internal class that is apparently never instantiated")]
internal sealed class OrderEntrySocketFake : ITerminal
{
    private readonly string _payload;
    private readonly TaskCompletionSource<string> _id;

    /// <summary>
    /// Initializes the fake with response payload. Usage example: new OrderEntrySocketFake(payload).
    /// </summary>
    public OrderEntrySocketFake(string payload)
    {
        _payload = payload ?? throw new ArgumentNullException(nameof(payload));
        _id = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    /// <summary>
    /// Captures routing request identifier. Usage example: await socket.Send(json, token).
    /// </summary>
    public Task Send(string payload, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(payload);
        using JsonDocument document = JsonDocument.Parse(payload);
        if (!document.RootElement.TryGetProperty("Id", out JsonElement element))
        {
            throw new ArgumentException("Payload must contain non-empty Id", nameof(payload));
        }
        string id = element.GetString() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Payload must contain non-empty Id", nameof(payload));
        }
        _id.TrySetResult(id);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Yields a single response message with captured correlation id. Usage example: await foreach (var m in socket.Messages(token)) { }.
    /// </summary>
    public async IAsyncEnumerable<string> Messages([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        string id = await _id.Task.WaitAsync(cancellationToken);
        string message = new ResponseText(id, _payload, "#Order.Enter.Query", "response").Value();
        yield return message;
    }

    /// <summary>
    /// Disposes resources. Usage example: await socket.DisposeAsync().
    /// </summary>
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
