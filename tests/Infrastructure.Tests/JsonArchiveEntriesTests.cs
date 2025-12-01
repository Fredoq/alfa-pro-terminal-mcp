using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;

/// <summary>
/// Verifies JsonArchiveEntries transformation for archive payloads. Usage example: executed by xUnit runner.
/// </summary>
public sealed class JsonArchiveEntriesTests
{
    /// <summary>
    /// Ensures that JsonArchiveEntries describes OHLCV candles. Usage example: new JsonArchiveEntries(payload).Json().
    /// </summary>
    [Fact(DisplayName = "JsonArchiveEntries returns described ohlcv candles")]
    public void Given_ohlcv_payload_when_parsed_then_describes_fields()
    {
        long volume = RandomNumberGenerator.GetInt32(1_000, 9_999);
        double open = RandomNumberGenerator.GetInt32(10, 99) + 0.25;
        string time = "2024-05-01T10:00:00+03:00-ж";
        string payload = JsonSerializer.Serialize(new
        {
            LastTradeNo = 0,
            OHLCV = new object[]
            {
                new
                {
                    Open = open,
                    Close = open + 1.0,
                    Low = open - 0.5,
                    High = open + 1.5,
                    Volume = volume,
                    VolumeAsk = volume + 5,
                    OpenInt = volume + 10,
                    DT = time
                }
            }
        });
        JsonArchiveEntries entries = new(payload);
        string json = entries.Json();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement entry = document.RootElement[0];
        double openValue = entry.GetProperty("Open").GetProperty("value").GetDouble();
        string timestamp = entry.GetProperty("Time").GetProperty("value").GetString() ?? string.Empty;
        bool result = Math.Abs(openValue - open) < 0.0001 && timestamp == time;
        Assert.True(result, "JsonArchiveEntries does not describe ohlcv candles");
    }

    /// <summary>
    /// Ensures that JsonArchiveEntries describes MPV candles with levels. Usage example: entries.Json().
    /// </summary>
    [Fact(DisplayName = "JsonArchiveEntries returns described mpv candles with levels")]
    public void Given_mpv_payload_when_parsed_then_describes_levels()
    {
        long volume = RandomNumberGenerator.GetInt32(20_000, 30_000);
        double price = RandomNumberGenerator.GetInt32(100, 200) + 0.75;
        string payload = JsonSerializer.Serialize(new
        {
            LastTradeNo = 0,
            MPV = new object[]
            {
                new
                {
                    Open = price,
                    Close = price + 2.0,
                    DT = "2024-06-06T12:00:00Z-ξ",
                    Prices = new object[] { price, price + 1.0 },
                    Volumes = new object[] { volume, volume + 3 },
                    AskVolumes = new object[] { volume + 7, volume + 9 }
                }
            }
        });
        JsonArchiveEntries entries = new(payload);
        string json = entries.Json();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement levels = document.RootElement[0].GetProperty("Levels");
        double firstPrice = levels[0].GetProperty("Price").GetProperty("value").GetDouble();
        int count = levels.GetArrayLength();
        bool result = Math.Abs(firstPrice - price) < 0.0001 && count == 2;
        Assert.True(result, "JsonArchiveEntries does not describe mpv candles with levels");
    }

    /// <summary>
    /// Confirms that JsonArchiveEntries outputs consistent JSON under concurrency. Usage example: entries.Json().
    /// </summary>
    [Fact(DisplayName = "JsonArchiveEntries remains consistent under concurrency")]
    public void Given_concurrent_calls_when_parsed_then_outputs_identical()
    {
        long volume = RandomNumberGenerator.GetInt32(5_000, 8_000);
        string payload = JsonSerializer.Serialize(new
        {
            LastTradeNo = 0,
            OHLCV = new object[]
            {
                new
                {
                    Open = 1.1,
                    Close = 1.2,
                    Low = 1.0,
                    High = 1.3,
                    Volume = volume,
                    VolumeAsk = volume + 1,
                    OpenInt = volume + 2,
                    DT = "2024-07-07T00:00:00Z-φ"
                }
            }
        });
        JsonArchiveEntries entries = new(payload);
        ConcurrentBag<string> results = new();
        Parallel.For(0, 5, _ => results.Add(entries.Json()));
        string sample = results.First();
        bool identical = results.All(item => item == sample);
        Assert.True(identical, "JsonArchiveEntries does not remain consistent under concurrency");
    }

    /// <summary>
    /// Validates that JsonArchiveEntries fails on missing data. Usage example: entries.Json().
    /// </summary>
    [Fact(DisplayName = "JsonArchiveEntries throws when archive candles are missing")]
    public void Given_empty_data_when_parsed_then_throws()
    {
        string payload = JsonSerializer.Serialize(new { LastTradeNo = 0, OHLCV = Array.Empty<object>() });
        JsonArchiveEntries entries = new(payload);
        Assert.Throws<InvalidOperationException>(() => entries.Json());
    }
}
