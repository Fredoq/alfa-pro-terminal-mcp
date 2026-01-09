using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Security.Cryptography;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

/// <summary>
/// Verifies WsPositions behavior for position retrieval. Usage example: executed by xUnit runner.
/// </summary>
public sealed class WsPositionsTests
{
    /// <summary>
    /// Ensures that WsPositions returns JSON containing requested account positions. Usage example: await positions.Positions(id, token).
    /// </summary>
    [Fact(DisplayName = "WsPositions returns positions json for matching account")]
    public async Task Given_positions_response_when_requested_then_returns_json()
    {
        long account = RandomNumberGenerator.GetInt32(50_000, 80_000);
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdPosition = account,
                    IdAccount = account,
                    IdSubAccount = account + 1,
                    IdRazdel = 1,
                    IdObject = 2,
                    IdFiBalance = 3,
                    IdBalanceGroup = 4,
                    AssetsPercent = 0.1,
                    PSTNKD = 0.2,
                    IsMoney = false,
                    IsRur = true,
                    UchPrice = 1.0,
                    TorgPos = 2.0,
                    Price = 3.0,
                    DailyPL = 4.0,
                    DailyPLPercentToMarketCurPrice = 5.0,
                    BackPos = 6.0,
                    PrevQuote = 7.0,
                    TrnIn = 8.0,
                    TrnOut = 9.0,
                    DailyBuyVolume = 10.0,
                    DailySellVolume = 11.0,
                    DailyBuyQuantity = 12.0,
                    DailySellQuantity = 13.0,
                    NKD = 14.0,
                    PriceStep = 15.0,
                    Lot = 1,
                    NPLtoMarketCurPrice = 16.0,
                    NPLPercent = 17.0,
                    PlanLong = 18.0,
                    PlanShort = 19.0
                }
            }
        });
        await using PositionSocketFake socket = new(payload);
        LoggerFake logger = new();
        WsPositions positions = new(socket, logger);
        string json = (await positions.Entries(account)).StructuredContent().ToJsonString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement entry = document.RootElement[0];
        double step = entry.GetProperty("PriceStep").GetDouble();
        bool result = entry.GetProperty("IdAccount").GetInt64() == account && Math.Abs(step - 15.0) < 0.0001;
        Assert.True(result, "WsPositions does not return positions json for matching account");
    }

    /// <summary>
    /// Confirms that WsPositions fails when requested account is absent. Usage example: await positions.Positions(id, token).
    /// </summary>
    [Fact(DisplayName = "WsPositions throws when positions are missing")]
    public async Task Given_response_without_target_account_when_requested_then_throws()
    {
        long account = RandomNumberGenerator.GetInt32(81_000, 90_000);
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdPosition = account + 1,
                    IdAccount = account + 1,
                    IdSubAccount = account + 2,
                    IdRazdel = 1,
                    IdObject = 2,
                    IdFiBalance = 3,
                    IdBalanceGroup = 4,
                    AssetsPercent = 0.1,
                    PSTNKD = 0.2,
                    IsMoney = true,
                    IsRur = false,
                    UchPrice = 1.0,
                    TorgPos = 2.0,
                    Price = 3.0,
                    DailyPL = 4.0,
                    DailyPLPercentToMarketCurPrice = 5.0,
                    BackPos = 6.0,
                    PrevQuote = 7.0,
                    TrnIn = 8.0,
                    TrnOut = 9.0,
                    DailyBuyVolume = 10.0,
                    DailySellVolume = 11.0,
                    DailyBuyQuantity = 12.0,
                    DailySellQuantity = 13.0,
                    NKD = 14.0,
                    PriceStep = 15.0,
                    Lot = 1,
                    NPLtoMarketCurPrice = 16.0,
                    NPLPercent = 17.0,
                    PlanLong = 18.0,
                    PlanShort = 19.0
                }
            }
        });
        await using PositionSocketFake socket = new(payload);
        LoggerFake logger = new();
        WsPositions positions = new(socket, logger);
        Task<string> action = Task.Run(async () => (await positions.Entries(account)).StructuredContent().ToJsonString());
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await action);
    }
}
