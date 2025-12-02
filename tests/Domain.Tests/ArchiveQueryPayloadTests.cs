using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Archive;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests;

using System.Security.Cryptography;
using System.Text.Json;

/// <summary>
/// Verifies ArchiveQueryPayload serialization. Usage example: executed by xUnit runner.
/// </summary>
public sealed class ArchiveQueryPayloadTests
{
    /// <summary>
    /// Confirms that ArchiveQueryPayload serializes provided parameters. Usage example: new ArchiveQueryPayload(...).AsString().
    /// </summary>
    [Fact(DisplayName = "ArchiveQueryPayload serializes provided parameters")]
    public void Given_parameters_when_serialized_then_contains_values()
    {
        long idFi = RandomNumberGenerator.GetInt32(10_000, 99_999);
        int candleType = RandomNumberGenerator.GetInt32(0, 3);
        string interval = "hour";
        int period = RandomNumberGenerator.GetInt32(1, 5);
        DateTime firstDay = new DateTime(2024, 6, RandomNumberGenerator.GetInt32(1, 20), 9, 0, 0, DateTimeKind.Utc);
        DateTime lastDay = firstDay.AddDays(RandomNumberGenerator.GetInt32(1, 5));
        ArchiveQueryPayload payload = new(idFi, candleType, interval, period, firstDay, lastDay);
        string json = payload.AsString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        bool result = root.GetProperty("IdFi").GetInt64() == idFi && root.GetProperty("CandleType").GetInt32() == candleType && root.GetProperty("Interval").GetString() == interval && root.GetProperty("Period").GetInt32() == period && root.GetProperty("FirstDay").GetDateTime() == firstDay && root.GetProperty("LastDay").GetDateTime() == lastDay;
        Assert.True(result, "ArchiveQueryPayload does not serialize provided parameters");
    }
}
