using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Security.Cryptography;
using System.Text.Json;

/// <summary>
/// Validates QueryResponse parsing and acceptance logic. Usage example: executed by xUnit runner.
/// </summary>
public sealed class QueryResponseTests
{
    /// <summary>
    /// Confirms that QueryResponse accepts matching identifier, command, and channel. Usage example: response.Accepted(message, id).
    /// </summary>
    [Fact(DisplayName = "QueryResponse accepts matching response metadata")]
    public void Given_matching_metadata_when_checked_then_accepts()
    {
        string id = Guid.NewGuid().ToString();
        string payload = JsonSerializer.Serialize(new { Value = $"значение-{Guid.NewGuid()}-λ" });
        string serialized = JsonSerializer.Serialize(payload);
        string message = $"{{\"Id\":\"{id}\",\"Command\":\"response\",\"Channel\":\"#Data.Query\",\"Payload\":{serialized}}}";
        QueryResponse response = new("#Data.Query");
        CorrelationId correlation = new(id);
        bool accepted = response.Accepted(message, correlation);
        Assert.True(accepted, "QueryResponse does not accept matching metadata");
    }

    /// <summary>
    /// Ensures that QueryResponse rejects mismatched identifiers. Usage example: response.Accepted(message, wrongId).
    /// </summary>
    [Fact(DisplayName = "QueryResponse rejects mismatched identifier")]
    public void Given_mismatched_identifier_when_checked_then_rejects()
    {
        string id = Guid.NewGuid().ToString();
        long number = RandomNumberGenerator.GetInt32(10, 99);
        string payload = JsonSerializer.Serialize(new { Value = $"нет-{Guid.NewGuid()}-ψ", Number = number });
        string serialized = JsonSerializer.Serialize(payload);
        string message = $"{{\"Id\":\"{Guid.NewGuid()}\",\"Command\":\"response\",\"Channel\":\"#Data.Query\",\"Payload\":{serialized}}}";
        QueryResponse response = new("#Data.Query");
        CorrelationId correlation = new(id);
        bool accepted = response.Accepted(message, correlation);
        Assert.False(accepted, "QueryResponse does not reject mismatched identifier");
    }

    /// <summary>
    /// Confirms that QueryResponse accepts archive channel metadata. Usage example: response.Accepted(message, id).
    /// </summary>
    [Fact(DisplayName = "QueryResponse accepts archive channel")]
    public void Given_archive_channel_when_checked_then_accepts()
    {
        string id = Guid.NewGuid().ToString();
        string payload = JsonSerializer.Serialize(new { Value = $"архив-{Guid.NewGuid()}-α" });
        string serialized = JsonSerializer.Serialize(payload);
        string message = $"{{\"Id\":\"{id}\",\"Command\":\"response\",\"Channel\":\"#Archive.Query\",\"Payload\":{serialized}}}";
        QueryResponse response = new("#Archive.Query");
        CorrelationId correlation = new(id);
        bool accepted = response.Accepted(message, correlation);
        Assert.True(accepted, "QueryResponse does not accept archive channel metadata");
    }

    /// <summary>
    /// Verifies that QueryResponse extracts payload from a router message. Usage example: string payload = response.Payload(message).
    /// </summary>
    [Fact(DisplayName = "QueryResponse extracts payload")]
    public void Given_payload_when_extracted_then_returns_value()
    {
        string id = Guid.NewGuid().ToString();
        string payload = JsonSerializer.Serialize(new { Value = $"данные-{Guid.NewGuid()}-ß" });
        string serialized = JsonSerializer.Serialize(payload);
        string message = $"{{\"Id\":\"{id}\",\"Command\":\"response\",\"Channel\":\"#Data.Query\",\"Payload\":{serialized}}}";
        QueryResponse response = new("#Data.Query");
        string value = response.Payload(message);
        Assert.True(value == payload, "QueryResponse does not extract payload");
    }
}
