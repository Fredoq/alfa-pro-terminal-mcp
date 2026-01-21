using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests.Support;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests;

using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;

/// <summary>
/// Validates ArchiveQueryRequest serialization and identity. Usage example: executed by xUnit runner.
/// </summary>
public sealed class ArchiveQueryRequestTests
{
    /// <summary>
    /// Ensures that ArchiveQueryRequest wraps payload with archive routing metadata. Usage example: new ArchiveQueryRequest(payload).AsString().
    /// </summary>
    [Fact(DisplayName = "ArchiveQueryRequest serializes archive routing metadata")]
    public void Given_payload_when_serialized_then_contains_archive_metadata()
    {
        string value = $"café-{RandomNumberGenerator.GetInt32(10_000, 90_000)}";
        PayloadFake payload = new(value);
        ArchiveQueryRequest request = new(payload);
        string json = request.AsString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        string channel = root.GetProperty("Channel").GetString() ?? string.Empty;
        string command = root.GetProperty("Command").GetString() ?? string.Empty;
        string id = root.GetProperty("Id").GetString() ?? string.Empty;
        string serialized = root.GetProperty("Payload").GetString() ?? string.Empty;
        using JsonDocument payloadDocument = JsonDocument.Parse(serialized);
        string embedded = payloadDocument.RootElement.GetProperty("Content").GetString() ?? string.Empty;
        bool result = channel == "#Archive.Query" && command == "request" && id.Length > 0 && embedded == value;
        Assert.True(result, "ArchiveQueryRequest does not serialize archive routing metadata");
    }

    /// <summary>
    /// Confirms that ArchiveQueryRequest produces unique identifiers under concurrency. Usage example: executed by xUnit runner.
    /// </summary>
    [Fact(DisplayName = "ArchiveQueryRequest generates unique identifiers concurrently")]
    public void Given_multiple_requests_when_created_concurrently_then_ids_unique()
    {
        IPayload payload = new PayloadFake($"café-{RandomNumberGenerator.GetInt32(100_000, 999_999)}");
        int count = 13;
        ConcurrentDictionary<string, byte> ids = new();
        Parallel.For(0, count, _ => ids.TryAdd(new ArchiveQueryRequest(payload).Id(), 0));
        bool unique = ids.Count == count;
        Assert.True(unique, "ArchiveQueryRequest does not generate unique identifiers concurrently");
    }
}
