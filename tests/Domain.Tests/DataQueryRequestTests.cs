using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests;

using System.Collections.Concurrent;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests.Support;

/// <summary>
/// Verifies DataQueryRequest serialization and identity. Usage example: executed by xUnit runner.
/// </summary>
public sealed class DataQueryRequestTests
{
    /// <summary>
    /// Confirms that DataQueryRequest serializes payload with routing metadata. Usage example: new DataQueryRequest(payload).AsString().
    /// </summary>
    [Fact(DisplayName = "DataQueryRequest serializes payload with metadata")]
    public void Given_payload_when_serialized_then_contains_metadata()
    {
        string content = $"данные-{Guid.NewGuid()}-ζ";
        IPayload payload = new PayloadFake(content);
        DataQueryRequest request = new(payload);
        string json = request.AsString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        string channel = root.GetProperty("Channel").GetString() ?? string.Empty;
        string command = root.GetProperty("Command").GetString() ?? string.Empty;
        string id = root.GetProperty("Id").GetString() ?? string.Empty;
        string serialized = root.GetProperty("Payload").GetString() ?? string.Empty;
        using JsonDocument payloadDocument = JsonDocument.Parse(serialized);
        string embedded = payloadDocument.RootElement.GetProperty("Content").GetString() ?? string.Empty;
        bool result = channel == "#Data.Query" && command == "request" && id.Length > 0 && embedded == content;
        Assert.True(result, "DataQueryRequest does not serialize payload with metadata");
    }

    /// <summary>
    /// Ensures that DataQueryRequest produces unique identifiers concurrently. Usage example: executed by xUnit runner.
    /// </summary>
    [Fact(DisplayName = "DataQueryRequest generates unique identifiers concurrently")]
    public void Given_multiple_requests_when_created_concurrently_then_ids_unique()
    {
        IPayload payload = new PayloadFake($"поток-{Guid.NewGuid()}-ß");
        int count = 17;
        ConcurrentDictionary<string, byte> map = new();
        Parallel.For(0, count, _ => map.TryAdd(new DataQueryRequest(payload).Id(), 0));
        bool unique = map.Count == count;
        Assert.True(unique, "DataQueryRequest does not generate unique identifiers concurrently");
    }
}
