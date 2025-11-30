namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Security.Cryptography;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

/// <summary>
/// Verifies AssetInfoEntityResponse retrieves payloads from router stream. Usage example: executed by xUnit runner.
/// </summary>
public sealed class AssetInfoEntityResponseTests
{
    /// <summary>
    /// Ensures that AssetInfoEntityResponse returns the first matching payload. Usage example: await response.NextMessage(token).
    /// </summary>
    [Fact(DisplayName = "AssetInfoEntityResponse returns payload from matching response")]
    public async Task Given_matching_response_when_next_message_then_returns_payload()
    {
        string id = Guid.NewGuid().ToString();
        long asset = RandomNumberGenerator.GetInt32(1_000, 10_000);
        string payload = JsonSerializer.Serialize(new { IdObject = asset, Name = $"актив-{Guid.NewGuid()}-α" });
        string other = Build(Guid.NewGuid().ToString(), payload, "#Data.Query");
        string message = Build(id, payload, "#Data.Query");
        await using RouterSocketSequence socket = new([other, message]);
        IncomingStub incoming = new(id);
        LoggerFake logger = new();
        Messaging.Responses.AssetInfoEntityResponse response = new(incoming, socket, logger);
        using CancellationTokenSource source = new(TimeSpan.FromSeconds(2));
        string value = await response.NextMessage(source.Token);
        Assert.True(value == payload, "AssetInfoEntityResponse does not return matching payload");
    }

    /// <summary>
    /// Confirms that AssetInfoEntityResponse fails when matching response is absent. Usage example: await response.NextMessage(token).
    /// </summary>
    [Fact(DisplayName = "AssetInfoEntityResponse throws when no response matches")]
    public async Task Given_no_matching_response_when_next_message_then_throws()
    {
        string id = Guid.NewGuid().ToString();
        long asset = RandomNumberGenerator.GetInt32(3_000, 9_000);
        string payload = JsonSerializer.Serialize(new { IdObject = asset, Name = $"нет-{Guid.NewGuid()}-β" });
        string other = Build(Guid.NewGuid().ToString(), payload, "#Data.Query");
        await using RouterSocketSequence socket = new([other]);
        IncomingStub incoming = new(id);
        LoggerFake logger = new();
        Messaging.Responses.AssetInfoEntityResponse response = new(incoming, socket, logger);
        using CancellationTokenSource source = new(TimeSpan.FromSeconds(2));
        Task<string> action = response.NextMessage(source.Token);
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await action);
    }

    /// <summary>
    /// Builds serialized router response text. Usage example: Build(id, payload, channel).
    /// </summary>
    private static string Build(string id, string payload, string channel) => $"{{\"Id\":\"{id}\",\"Command\":\"response\",\"Channel\":\"{channel}\",\"Payload\":{JsonSerializer.Serialize(payload)}}}";
}
