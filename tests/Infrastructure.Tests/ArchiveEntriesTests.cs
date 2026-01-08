using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;

/// <summary>
/// Verifies archive entries transformation for archive payloads. Usage example: executed by xUnit runner.
/// </summary>
public sealed class ArchiveEntriesTests
{
    /// <summary>
    /// Ensures that archive entries return OHLCV candles. Usage example: new SchemaEntries(...).Json().
    /// </summary>
    [Fact(DisplayName = "Archive entries return ohlcv candles")]
    public void Given_ohlcv_payload_when_parsed_then_returns_fields()
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
        FallbackEntries entries = new(new RequiredEntries(new SchemaEntries(new PayloadArrayEntries(payload, "OHLCV"), new OhlcvSchema()), "Archive candles are missing"), new RequiredEntries(new SchemaEntries(new PayloadArrayEntries(payload, "MPV"), new MpvSchema()), "Archive candles are missing"));
        string json = entries.Json();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement entry = document.RootElement[0];
        double value = entry.GetProperty("Open").GetDouble();
        string stamp = entry.GetProperty("Time").GetString() ?? string.Empty;
        bool result = Math.Abs(value - open) < 0.0001 && stamp == time;
        Assert.True(result, "Archive entries do not return ohlcv candles");
    }

    /// <summary>
    /// Ensures that archive entries return MPV candles with levels. Usage example: entries.Json().
    /// </summary>
    [Fact(DisplayName = "Archive entries return mpv candles with levels")]
    public void Given_mpv_payload_when_parsed_then_returns_levels()
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
        FallbackEntries entries = new(new RequiredEntries(new SchemaEntries(new PayloadArrayEntries(payload, "OHLCV"), new OhlcvSchema()), "Archive candles are missing"), new RequiredEntries(new SchemaEntries(new PayloadArrayEntries(payload, "MPV"), new MpvSchema()), "Archive candles are missing"));
        string json = entries.Json();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement levels = document.RootElement[0].GetProperty("Levels");
        double value = levels[0].GetProperty("Price").GetDouble();
        int count = levels.GetArrayLength();
        bool result = Math.Abs(value - price) < 0.0001 && count == 2;
        Assert.True(result, "Archive entries do not return mpv candles with levels");
    }

    /// <summary>
    /// Confirms that archive entries output consistent JSON under concurrency. Usage example: entries.Json().
    /// </summary>
    [Fact(DisplayName = "Archive entries remain consistent under concurrency")]
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
        FallbackEntries entries = new(new RequiredEntries(new SchemaEntries(new PayloadArrayEntries(payload, "OHLCV"), new OhlcvSchema()), "Archive candles are missing"), new RequiredEntries(new SchemaEntries(new PayloadArrayEntries(payload, "MPV"), new MpvSchema()), "Archive candles are missing"));
        ConcurrentBag<string> results = new();
        Parallel.For(0, 5, _ => results.Add(entries.Json()));
        string sample = results.First();
        bool identical = results.All(item => item == sample);
        Assert.True(identical, "Archive entries do not remain consistent under concurrency");
    }

    /// <summary>
    /// Validates that archive entries fail on missing data. Usage example: entries.Json().
    /// </summary>
    [Fact(DisplayName = "Archive entries throw when archive candles are missing")]
    public void Given_empty_data_when_parsed_then_throws()
    {
        string payload = JsonSerializer.Serialize(new { LastTradeNo = 0, OHLCV = Array.Empty<object>() });
        FallbackEntries entries = new(new RequiredEntries(new SchemaEntries(new PayloadArrayEntries(payload, "OHLCV"), new OhlcvSchema()), "Archive candles are missing"), new RequiredEntries(new SchemaEntries(new PayloadArrayEntries(payload, "MPV"), new MpvSchema()), "Archive candles are missing"));
        Assert.Throws<InvalidOperationException>(() => entries.Json());
    }
}
