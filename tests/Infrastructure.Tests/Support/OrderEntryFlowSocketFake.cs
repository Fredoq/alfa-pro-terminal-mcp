using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Channels;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

/// <summary>
/// Simulates router behavior for order entry flow requests using predefined responses. Usage example: new OrderEntryFlowSocketFake(responses).
/// </summary>
internal sealed class OrderEntryFlowSocketFake : ITerminal
{
    private readonly Channel<string> _queue = Channel.CreateUnbounded<string>(new UnboundedChannelOptions { SingleReader = false, SingleWriter = false });
    private readonly List<string> _items = [];
    private readonly IReadOnlyDictionary<string, string> _responses;

    /// <summary>
    /// Initializes the fake with response payloads. Usage example: var socket = new OrderEntryFlowSocketFake(responses).
    /// </summary>
    /// <param name="responses">Response payloads by entity name or channel.</param>
    public OrderEntryFlowSocketFake(IReadOnlyDictionary<string, string> responses)
    {
        _responses = responses;
    }

    /// <summary>
    /// Returns captured outbound payloads. Usage example: IReadOnlyList<string> list = socket.Items().
    /// </summary>
    /// <returns>Captured payloads.</returns>
    internal IReadOnlyList<string> Items() => _items;

    /// <summary>
    /// Captures routing request and enqueues a response. Usage example: await socket.Send(json, token).
    /// </summary>
    public Task Send(string payload, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(payload);
        using JsonDocument document = JsonDocument.Parse(payload);
        string id = document.RootElement.GetProperty("Id").GetString() ?? string.Empty;
        string channel = document.RootElement.GetProperty("Channel").GetString() ?? string.Empty;
        _items.Add(payload);
        string body = channel == "#Order.Enter.Query" ? Entry() : Data(document.RootElement);
        string text = new ResponseText(id, body, channel, "response").Value();
        _queue.Writer.TryWrite(text);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Streams queued messages. Usage example: await foreach (var message in socket.Messages(token)) { }.
    /// </summary>
    public async IAsyncEnumerable<string> Messages([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (await _queue.Reader.WaitToReadAsync(cancellationToken))
        {
            while (_queue.Reader.TryRead(out string? message))
            {
                if (message is null)
                {
                    throw new InvalidOperationException("Message is missing");
                }
                yield return message;
            }
        }
    }

    /// <summary>
    /// Disposes resources. Usage example: await socket.DisposeAsync().
    /// </summary>
    public ValueTask DisposeAsync()
    {
        _queue.Writer.TryComplete();
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Resolves a data query response payload. Usage example: string json = socket.Data(root).
    /// </summary>
    /// <param name="root">Routing request root element.</param>
    /// <returns>Response payload.</returns>
    private string Data(JsonElement root)
    {
        string payload = root.GetProperty("Payload").GetString() ?? string.Empty;
        using JsonDocument document = JsonDocument.Parse(payload);
        string type = document.RootElement.GetProperty("Type").GetString() ?? string.Empty;
        if (!_responses.TryGetValue(type, out string? value))
        {
            throw new InvalidOperationException("Response payload is missing");
        }
        return value;
    }

    /// <summary>
    /// Resolves an order entry response payload. Usage example: string json = socket.Entry().
    /// </summary>
    /// <returns>Response payload.</returns>
    private string Entry()
    {
        if (!_responses.TryGetValue("#Order.Enter.Query", out string? value))
        {
            throw new InvalidOperationException("Response payload is missing");
        }
        return value;
    }
}
