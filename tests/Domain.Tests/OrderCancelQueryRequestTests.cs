using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests;

using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests.Support;

/// <summary>
/// Verifies OrderCancelQueryRequest serialization and identity. Usage example: executed by xUnit runner.
/// </summary>
public sealed class OrderCancelQueryRequestTests
{
    /// <summary>
    /// Confirms that OrderCancelQueryRequest serializes payload with routing metadata. Usage example: new OrderCancelQueryRequest(payload).AsString().
    /// </summary>
    [Fact(DisplayName = "OrderCancelQueryRequest serializes payload with metadata")]
    public void Given_payload_when_serialized_then_contains_metadata()
    {
        string value = $"cancel-{RandomNumberGenerator.GetInt32(10_000, 90_000)}-naïve";
        IPayload payload = new PayloadFake(value);
        OrderCancelQueryRequest request = new(payload);
        string json = request.AsString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        string channel = root.GetProperty("Channel").GetString() ?? string.Empty;
        string command = root.GetProperty("Command").GetString() ?? string.Empty;
        string id = root.GetProperty("Id").GetString() ?? string.Empty;
        string serialized = root.GetProperty("Payload").GetString() ?? string.Empty;
        using JsonDocument payloadDocument = JsonDocument.Parse(serialized);
        string embedded = payloadDocument.RootElement.GetProperty("Content").GetString() ?? string.Empty;
        bool result = channel == "#Order.Cancel.Query" && command == "request" && id.Length > 0 && embedded == value;
        Assert.True(result, "OrderCancelQueryRequest does not serialize payload with metadata");
    }

    /// <summary>
    /// Ensures that OrderCancelQueryRequest produces unique identifiers concurrently. Usage example: executed by xUnit runner.
    /// </summary>
    [Fact(DisplayName = "OrderCancelQueryRequest generates unique identifiers concurrently")]
    public void Given_multiple_requests_when_created_concurrently_then_ids_unique()
    {
        IPayload payload = new PayloadFake($"cancel-{RandomNumberGenerator.GetInt32(100_000, 999_999)}-café");
        int count = 11;
        ConcurrentDictionary<string, byte> map = new();
        Parallel.For(0, count, _ => map.TryAdd(new OrderCancelQueryRequest(payload).Id(), 0));
        bool unique = map.Count == count;
        Assert.True(unique, "OrderCancelQueryRequest does not generate unique identifiers concurrently");
    }
}
