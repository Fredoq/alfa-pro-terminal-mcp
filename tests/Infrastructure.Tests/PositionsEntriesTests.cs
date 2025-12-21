using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Filters;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;

/// <summary>
/// Verifies positions transformation from router payloads. Usage example: executed by xUnit runner.
/// </summary>
public sealed class PositionsEntriesTests
{
    /// <summary>
    /// Ensures that positions entries extract described positions for target account. Usage example: new SchemaEntries(...).Json().
    /// </summary>
    [Fact(DisplayName = "Positions entries return described positions for account")]
    public void Given_json_with_positions_when_parsed_then_filters_and_describes()
    {
        long account = RandomNumberGenerator.GetInt32(10_000, 99_999);
        long other = account + RandomNumberGenerator.GetInt32(3, 9);
        int lot = RandomNumberGenerator.GetInt32(1, 10);
        double price = RandomNumberGenerator.GetInt32(50, 150);
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdPosition = account + 1,
                    IdAccount = account,
                    Note = "ночь",
                    IdSubAccount = account + 2,
                    IdRazdel = 1,
                    IdObject = 777,
                    IdFiBalance = 888,
                    IdBalanceGroup = 1,
                    AssetsPercent = 0.5,
                    PSTNKD = 1.1,
                    IsMoney = false,
                    IsRur = true,
                    UchPrice = price,
                    TorgPos = 10.0,
                    Price = price + 1,
                    DailyPL = 2.0,
                    DailyPLPercentToMarketCurPrice = 0.1,
                    BackPos = 5.0,
                    PrevQuote = price - 1,
                    TrnIn = 0.2,
                    TrnOut = 0.1,
                    DailyBuyVolume = 1.0,
                    DailySellVolume = 0.5,
                    DailyBuyQuantity = 2.0,
                    DailySellQuantity = 1.0,
                    NKD = 0.01,
                    PriceStep = 0.05,
                    Lot = lot,
                    NPLtoMarketCurPrice = 3.0,
                    NPLPercent = 0.2,
                    PlanLong = 12.0,
                    PlanShort = 0.0
                },
                new
                {
                    IdPosition = other,
                    IdAccount = other,
                    IdSubAccount = other,
                    IdRazdel = 1,
                    IdObject = 999,
                    IdFiBalance = 999,
                    IdBalanceGroup = 1,
                    AssetsPercent = 0.1,
                    PSTNKD = 0.1,
                    IsMoney = true,
                    IsRur = false,
                    UchPrice = 1.0,
                    TorgPos = 1.0,
                    Price = 1.0,
                    DailyPL = 1.0,
                    DailyPLPercentToMarketCurPrice = 1.0,
                    BackPos = 1.0,
                    PrevQuote = 1.0,
                    TrnIn = 1.0,
                    TrnOut = 1.0,
                    DailyBuyVolume = 1.0,
                    DailySellVolume = 1.0,
                    DailyBuyQuantity = 1.0,
                    DailySellQuantity = 1.0,
                    NKD = 1.0,
                    PriceStep = 1.0,
                    Lot = 1,
                    NPLtoMarketCurPrice = 1.0,
                    NPLPercent = 1.0,
                    PlanLong = 1.0,
                    PlanShort = 1.0
                }
            }
        });
        SchemaEntries entries = new(new FilteredEntries(new PayloadArrayEntries(payload), new AccountScope(account), "Account positions are missing"), new PositionSchema());
        string json = entries.Json();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement entry = document.RootElement[0];
        bool result = entry.GetProperty("IdAccount").GetProperty("value").GetInt64() == account && entry.GetProperty("Lot").GetProperty("value").GetInt64() == lot && entry.GetProperty("Price").GetProperty("description").GetString()?.Length > 0;
        Assert.True(result, "Positions entries do not filter and describe positions");
    }

    /// <summary>
    /// Checks that positions entries yield consistent output in parallel calls. Usage example: entries.Json().
    /// </summary>
    [Fact(DisplayName = "Positions entries remain consistent under concurrency")]
    public void Given_concurrent_calls_when_parsed_then_outputs_identical()
    {
        long account = RandomNumberGenerator.GetInt32(100_000, 199_999);
        int lot = RandomNumberGenerator.GetInt32(2, 7);
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdPosition = account,
                    IdAccount = account,
                    IdSubAccount = account,
                    IdRazdel = 1,
                    IdObject = 1,
                    IdFiBalance = 1,
                    IdBalanceGroup = 1,
                    AssetsPercent = 0.2,
                    PSTNKD = 0.1,
                    IsMoney = false,
                    IsRur = true,
                    UchPrice = 1.0,
                    TorgPos = 2.0,
                    Price = 3.0,
                    DailyPL = 0.1,
                    DailyPLPercentToMarketCurPrice = 0.2,
                    BackPos = 0.3,
                    PrevQuote = 0.4,
                    TrnIn = 0.5,
                    TrnOut = 0.6,
                    DailyBuyVolume = 0.7,
                    DailySellVolume = 0.8,
                    DailyBuyQuantity = 0.9,
                    DailySellQuantity = 1.0,
                    NKD = 1.1,
                    PriceStep = 1.2,
                    Lot = lot,
                    NPLtoMarketCurPrice = 1.3,
                    NPLPercent = 1.4,
                    PlanLong = 1.5,
                    PlanShort = 1.6
                }
            }
        });
        SchemaEntries entries = new(new FilteredEntries(new PayloadArrayEntries(payload), new AccountScope(account), "Account positions are missing"), new PositionSchema());
        ConcurrentBag<string> results = new();
        Parallel.For(0, 5, _ => results.Add(entries.Json()));
        string sample = results.First();
        bool identical = results.All(item => item == sample);
        Assert.True(identical, "Positions entries do not remain consistent under concurrency");
    }

    /// <summary>
    /// Confirms that positions entries fail when account positions are missing. Usage example: entries.Json().
    /// </summary>
    [Fact(DisplayName = "Positions entries throw when positions are missing")]
    public void Given_missing_positions_when_parsed_then_throws()
    {
        long account = RandomNumberGenerator.GetInt32(201_000, 299_999);
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdPosition = account + 1,
                    IdAccount = account + 1,
                    IdSubAccount = account + 1,
                    IdRazdel = 1,
                    IdObject = 1,
                    IdFiBalance = 1,
                    IdBalanceGroup = 1,
                    AssetsPercent = 1.0,
                    PSTNKD = 1.0,
                    IsMoney = true,
                    IsRur = false,
                    UchPrice = 1.0,
                    TorgPos = 1.0,
                    Price = 1.0,
                    DailyPL = 1.0,
                    DailyPLPercentToMarketCurPrice = 1.0,
                    BackPos = 1.0,
                    PrevQuote = 1.0,
                    TrnIn = 1.0,
                    TrnOut = 1.0,
                    DailyBuyVolume = 1.0,
                    DailySellVolume = 1.0,
                    DailyBuyQuantity = 1.0,
                    DailySellQuantity = 1.0,
                    NKD = 1.0,
                    PriceStep = 1.0,
                    Lot = 1,
                    NPLtoMarketCurPrice = 1.0,
                    NPLPercent = 1.0,
                    PlanLong = 1.0,
                    PlanShort = 1.0
                }
            }
        });
        SchemaEntries entries = new(new FilteredEntries(new PayloadArrayEntries(payload), new AccountScope(account), "Account positions are missing"), new PositionSchema());
        Assert.Throws<InvalidOperationException>(() => entries.Json());
    }
}
