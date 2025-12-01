using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Archive;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;

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
        long idFi = RandomNumberGenerator.GetInt32(30_000, 60_000);
        ArchiveQueryPayload payload = new(idFi, 0, "day", 1, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(1));
        ArchiveQueryRequest request = new(payload);
        string json = request.AsString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        string channel = root.GetProperty("Channel").GetString() ?? string.Empty;
        string command = root.GetProperty("Command").GetString() ?? string.Empty;
        string id = root.GetProperty("Id").GetString() ?? string.Empty;
        string serialized = root.GetProperty("Payload").GetString() ?? string.Empty;
        using JsonDocument payloadDocument = JsonDocument.Parse(serialized);
        long embedded = payloadDocument.RootElement.GetProperty("IdFi").GetInt64();
        bool result = channel == "#Archive.Query" && command == "request" && id.Length > 0 && embedded == idFi;
        Assert.True(result, "ArchiveQueryRequest does not serialize archive routing metadata");
    }

    /// <summary>
    /// Confirms that ArchiveQueryRequest produces unique identifiers under concurrency. Usage example: executed by xUnit runner.
    /// </summary>
    [Fact(DisplayName = "ArchiveQueryRequest generates unique identifiers concurrently")]
    public void Given_multiple_requests_when_created_concurrently_then_ids_unique()
    {
        IPayload payload = new ArchiveQueryPayload(RandomNumberGenerator.GetInt32(100_000, 999_999), 2, "minute", 5, DateTime.UtcNow.Date.AddDays(-3), DateTime.UtcNow.Date);
        int count = 13;
        ConcurrentDictionary<string, byte> ids = new();
        Parallel.For(0, count, _ => ids.TryAdd(new ArchiveQueryRequest(payload).Id(), 0));
        bool unique = ids.Count == count;
        Assert.True(unique, "ArchiveQueryRequest does not generate unique identifiers concurrently");
    }
}
