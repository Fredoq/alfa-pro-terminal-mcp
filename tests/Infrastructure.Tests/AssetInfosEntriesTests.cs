using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Filters;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;

/// <summary>
/// Verifies asset infos entries transformation from router payloads. Usage example: executed by xUnit runner.
/// </summary>
public sealed class AssetInfosEntriesTests
{
    /// <summary>
    /// Ensures that asset infos entries extract assets for target identifiers. Usage example: new SchemaEntries(...).Text().
    /// </summary>
    [Fact(DisplayName = "Asset infos entries return assets for identifiers")]
    public void Given_json_with_assets_when_parsed_then_filters()
    {
        long id = RandomNumberGenerator.GetInt32(10_000, 99_999);
        long other = id + RandomNumberGenerator.GetInt32(3, 9);
        string ticker = $"T{id}";
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdObject = id,
                    Ticker = ticker,
                    ISIN = "ISIN123",
                    Name = "Name",
                    Description = (string?)null,
                    Nominal = 10.5,
                    IdObjectType = 1,
                    IdObjectGroup = 2,
                    IdObjectBase = 3,
                    IdObjectFaceUnit = 4,
                    MatDateObject = "2025-01-01",
                    Instruments = new object[]
                    {
                        new { IdFi = 7, RCode = "R1", IsLiquid = true, IdMarketBoard = 10 },
                        new { IdFi = 8, RCode = "R2", IsLiquid = false, IdMarketBoard = 11 }
                    }
                },
                new
                {
                    IdObject = other,
                    Ticker = "SKIP",
                    ISIN = "ISIN999",
                    Name = "Other",
                    Description = "Other",
                    Nominal = 1.0,
                    IdObjectType = 1,
                    IdObjectGroup = 1,
                    IdObjectBase = 1,
                    IdObjectFaceUnit = 1,
                    MatDateObject = "2025-02-02",
                    Instruments = new object[]
                    {
                        new { IdFi = 1, RCode = "R3", IsLiquid = false, IdMarketBoard = 1 }
                    }
                }
            }
        });
        SchemaEntries entries = new(new FilteredEntries(new PayloadArrayEntries(payload), new AssetIdsScope([id]), "Asset infos are missing"), new AssetInfoSchema());
        string json = entries.Text();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement entry = document.RootElement[0];
        JsonElement instruments = entry.GetProperty("Instruments");
        bool result = entry.GetProperty("Ticker").GetString() == ticker && string.IsNullOrEmpty(entry.GetProperty("Description").GetString()) && instruments.GetArrayLength() == 2 && instruments[0].GetProperty("IsLiquid").GetBoolean();
        Assert.True(result, "Asset infos entries do not filter assets");
    }

    /// <summary>
    /// Checks that asset infos entries yield consistent output in parallel calls. Usage example: entries.Text().
    /// </summary>
    [Fact(DisplayName = "Asset infos entries remain consistent under concurrency")]
    public void Given_concurrent_calls_when_parsed_then_outputs_identical()
    {
        long id = RandomNumberGenerator.GetInt32(100_000, 199_999);
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdObject = id,
                    Ticker = "AAA",
                    ISIN = "BBB",
                    Name = "CCC",
                    Description = "DDD",
                    Nominal = 1.0,
                    IdObjectType = 1,
                    IdObjectGroup = 1,
                    IdObjectBase = 1,
                    IdObjectFaceUnit = 1,
                    MatDateObject = "2025-03-03",
                    Instruments = new object[]
                    {
                        new { IdFi = 5, RCode = "R5", IsLiquid = true, IdMarketBoard = 9 }
                    }
                }
            }
        });
        SchemaEntries entries = new(new FilteredEntries(new PayloadArrayEntries(payload), new AssetIdsScope([id]), "Asset infos are missing"), new AssetInfoSchema());
        ConcurrentBag<string> results = [];
        Parallel.For(0, 5, _ => results.Add(entries.Text()));
        string sample = results.First();
        bool identical = results.All(item => item == sample);
        Assert.True(identical, "Asset infos entries do not remain consistent under concurrency");
    }

    /// <summary>
    /// Confirms that asset infos entries fail when asset infos are missing. Usage example: entries.Text().
    /// </summary>
    [Fact(DisplayName = "Asset infos entries throw when assets are missing")]
    public void Given_missing_assets_when_parsed_then_throws()
    {
        long id = RandomNumberGenerator.GetInt32(201_000, 299_999);
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdObject = id + 1,
                    Ticker = "X",
                    ISIN = "Y",
                    Name = "Z",
                    Description = "D",
                    Nominal = 1.0,
                    IdObjectType = 1,
                    IdObjectGroup = 1,
                    IdObjectBase = 1,
                    IdObjectFaceUnit = 1,
                    MatDateObject = "2025-04-04",
                    Instruments = new object[]
                    {
                        new { IdFi = 1, RCode = "R", IsLiquid = false, IdMarketBoard = 1 }
                    }
                }
            }
        });
        SchemaEntries entries = new(new FilteredEntries(new PayloadArrayEntries(payload), new AssetIdsScope(new[] { id }), "Asset infos are missing"), new AssetInfoSchema());
        Assert.Throws<InvalidOperationException>(() => entries.Text());
    }
}
