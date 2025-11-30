using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Security.Cryptography;
using System.Text.Json;

/// <summary>
/// Validates DataQueryResponse parsing and acceptance logic. Usage example: executed by xUnit runner.
/// </summary>
public sealed class DataQueryResponseTests
{
    /// <summary>
    /// Confirms that DataQueryResponse accepts matching identifier, command, and channel. Usage example: new DataQueryResponse(message).Accepted(id).
    /// </summary>
    [Fact(DisplayName = "DataQueryResponse accepts matching response metadata")]
    public void Given_matching_metadata_when_checked_then_accepts()
    {
        string id = Guid.NewGuid().ToString();
        string payload = $"{{\"value\":\"{Guid.NewGuid()}-λ\"}}";
        string serialized = JsonSerializer.Serialize(payload);
        string message = $"{{\"Id\":\"{id}\",\"Command\":\"response\",\"Channel\":\"#Data.Query\",\"Payload\":{serialized}}}";
        DataQueryResponse response = new(message);
        CorrelationId correlation = new(id);
        bool accepted = response.Accepted(correlation);
        Assert.True(accepted, "DataQueryResponse does not accept matching metadata");
    }

    /// <summary>
    /// Ensures that DataQueryResponse rejects mismatched identifiers. Usage example: response.Accepted(wrongId).
    /// </summary>
    [Fact(DisplayName = "DataQueryResponse rejects mismatched metadata")]
    public void Given_mismatched_metadata_when_checked_then_rejects()
    {
        string id = Guid.NewGuid().ToString();
        string payload = $"{{\"value\":\"{Guid.NewGuid()}-ψ\"}}";
        string serialized = JsonSerializer.Serialize(payload);
        string message = $"{{\"Id\":\"{Guid.NewGuid()}\",\"Command\":\"response\",\"Channel\":\"#Data.Query\",\"Payload\":{serialized}}}";
        DataQueryResponse response = new(message);
        CorrelationId correlation = new(id);
        bool accepted = response.Accepted(correlation);
        Assert.False(accepted, "DataQueryResponse does not reject mismatched metadata");
    }

    /// <summary>
    /// Verifies JsonElementExtensions.Number parses numeric property. Usage example: element.Number(\"Data\").
    /// </summary>
    [Fact(DisplayName = "JsonElementExtensions parses numeric property")]
    public void Given_numeric_property_when_parsed_then_returns_number()
    {
        int number = RandomNumberGenerator.GetInt32(1, 9);
        using JsonDocument document = JsonDocument.Parse($"{{\"Data\":{number}}}");
        JsonElement element = document.RootElement;
        int value = element.Number("Data");
        Assert.True(value == number, "JsonElementExtensions does not parse numeric property");
    }
}
