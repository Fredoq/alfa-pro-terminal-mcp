using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Security.Cryptography;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

/// <summary>
/// Verifies WsArchive behavior for archive retrieval. Usage example: executed by xUnit runner.
/// </summary>
public sealed class WsArchiveTests
{
    /// <summary>
    /// Ensures that WsArchive returns archive candles while skipping heartbeat. Usage example: await archive.Entries(payload).
    /// </summary>
    [Fact(DisplayName = "WsArchive returns archive json and ignores heartbeat")]
    public async Task Given_archive_response_with_heartbeat_when_requested_then_returns_json()
    {
        long volume = RandomNumberGenerator.GetInt32(9_000, 12_000);
        double open = RandomNumberGenerator.GetInt32(50, 70) + 0.5;
        string text = JsonSerializer.Serialize(new
        {
            LastTradeNo = 0,
            OHLCV = new object[]
            {
                new
                {
                    Open = open,
                    Close = open + 1.0,
                    Low = open - 0.3,
                    High = open + 1.2,
                    Volume = volume,
                    VolumeAsk = volume + 2,
                    OpenInt = volume + 4,
                    DT = "2024-08-08T08:00:00Z-ψ"
                }
            }
        });
        await using ArchiveSocketFake socket = new(text, true);
        LoggerFake logger = new();
        WsArchive archive = new(socket, logger);
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idFi":{"type":"integer","description":"Financial instrument identifier"},"candleType":{"type":"integer","description":"Candle kind: 0 for OHLCV, 2 for MPV"},"interval":{"type":"string","description":"Timeframe unit: second, minute, hour, day, week or month"},"period":{"type":"integer","description":"Interval multiplier matching the interval unit"},"firstDay":{"type":"string","format":"date-time","description":"First requested trading day inclusive"},"lastDay":{"type":"string","format":"date-time","description":"Last requested trading day inclusive"}},"required":["idFi","candleType","interval","period","firstDay","lastDay"]}"""));
        DateTime first = DateTime.UtcNow.Date.AddDays(-2);
        DateTime last = DateTime.UtcNow.Date;
        Dictionary<string, JsonElement> data = new(StringComparer.Ordinal)
        {
            ["idFi"] = JsonSerializer.Deserialize<JsonElement>("123"),
            ["candleType"] = JsonSerializer.Deserialize<JsonElement>("0"),
            ["interval"] = JsonSerializer.Deserialize<JsonElement>("\"day-é\""),
            ["period"] = JsonSerializer.Deserialize<JsonElement>("1"),
            ["firstDay"] = JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(first)),
            ["lastDay"] = JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(last))
        };
        MappedPayload payload = new(data, schema);
        string json = (await archive.Entries(payload)).StructuredContent().ToJsonString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement entry = document.RootElement.GetProperty("candles")[0];
        double value = entry.GetProperty("Open").GetDouble();
        bool result = Math.Abs(value - open) < 0.0001;
        Assert.True(result, "WsArchive does not return archive json and ignore heartbeat");
    }

    /// <summary>
    /// Ensures that WsArchive returns MPV levels through archive entries. Usage example: await archive.Entries(payload).
    /// </summary>
    [Fact(DisplayName = "WsArchive returns mpv levels")]
    public async Task Given_mpv_response_when_requested_then_returns_levels()
    {
        long volume = RandomNumberGenerator.GetInt32(15_000, 25_000);
        double price = RandomNumberGenerator.GetInt32(80, 120) + 0.9;
        string text = JsonSerializer.Serialize(new
        {
            LastTradeNo = 0,
            MPV = new object[]
            {
                new
                {
                    Open = price,
                    Close = price + 0.5,
                    DT = "2024-09-09T09:00:00Z-ω",
                    Prices = new object[] { price },
                    Volumes = new object[] { volume },
                    AskVolumes = new object[] { volume + 11 }
                }
            }
        });
        await using ArchiveSocketFake socket = new(text, false);
        LoggerFake logger = new();
        WsArchive archive = new(socket, logger);
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idFi":{"type":"integer","description":"Financial instrument identifier"},"candleType":{"type":"integer","description":"Candle kind: 0 for OHLCV, 2 for MPV"},"interval":{"type":"string","description":"Timeframe unit: second, minute, hour, day, week or month"},"period":{"type":"integer","description":"Interval multiplier matching the interval unit"},"firstDay":{"type":"string","format":"date-time","description":"First requested trading day inclusive"},"lastDay":{"type":"string","format":"date-time","description":"Last requested trading day inclusive"}},"required":["idFi","candleType","interval","period","firstDay","lastDay"]}"""));
        DateTime first = DateTime.UtcNow.Date.AddDays(-1);
        DateTime last = DateTime.UtcNow.Date;
        Dictionary<string, JsonElement> data = new(StringComparer.Ordinal)
        {
            ["idFi"] = JsonSerializer.Deserialize<JsonElement>("321"),
            ["candleType"] = JsonSerializer.Deserialize<JsonElement>("2"),
            ["interval"] = JsonSerializer.Deserialize<JsonElement>("\"hour-é\""),
            ["period"] = JsonSerializer.Deserialize<JsonElement>("3"),
            ["firstDay"] = JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(first)),
            ["lastDay"] = JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(last))
        };
        MappedPayload payload = new(data, schema);
        string json = (await archive.Entries(payload)).StructuredContent().ToJsonString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement level = document.RootElement.GetProperty("candles")[0].GetProperty("Levels")[0];
        double value = level.GetProperty("Price").GetDouble();
        bool result = Math.Abs(value - price) < 0.0001;
        Assert.True(result, "WsArchive does not return mpv levels");
    }
}
