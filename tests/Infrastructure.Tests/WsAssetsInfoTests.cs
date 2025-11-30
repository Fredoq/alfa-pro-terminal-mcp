using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Security.Cryptography;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

/// <summary>
/// Verifies WsAssetsInfo behavior for asset info retrieval. Usage example: executed by xUnit runner.
/// </summary>
public sealed class WsAssetsInfoTests
{
    /// <summary>
    /// Ensures that WsAssetsInfo returns JSON containing requested asset info. Usage example: await infos.Info(ids, token).
    /// </summary>
    [Fact(DisplayName = "WsAssetsInfo returns asset infos json for matching ids")]
    public async Task Given_asset_response_when_requested_then_returns_json()
    {
        long id = RandomNumberGenerator.GetInt32(50_000, 80_000);
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdObject = id,
                    Ticker = "AA",
                    ISIN = "BB",
                    Name = "CC",
                    Description = "DD",
                    Nominal = 1.0,
                    IdObjectType = 1,
                    IdObjectGroup = 1,
                    IdObjectBase = 1,
                    IdObjectFaceUnit = 1,
                    MatDateObject = "2025-05-05",
                    Instruments = new object[]
                    {
                        new { IdFi = 7, RCode = "R1", IsLiquid = true, IdMarketBoard = 2 }
                    }
                }
            }
        });
        await using AssetSocketFake socket = new(payload);
        LoggerFake logger = new();
        WsAssetsInfo infos = new(socket, logger);
        string json = (await infos.Info(new[] { id })).Json();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement entry = document.RootElement[0];
        bool result = entry.GetProperty("IdObject").GetProperty("value").GetInt64() == id && entry.GetProperty("Instruments")[0].GetProperty("IdFi").GetProperty("value").GetInt64() == 7;
        Assert.True(result, "WsAssetsInfo does not return asset infos json for matching ids");
    }

    /// <summary>
    /// Confirms that WsAssetsInfo fails when requested assets are absent. Usage example: await infos.Info(ids, token).
    /// </summary>
    [Fact(DisplayName = "WsAssetsInfo throws when asset infos are missing")]
    public async Task Given_response_without_target_assets_when_requested_then_throws()
    {
        long id = RandomNumberGenerator.GetInt32(81_000, 90_000);
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdObject = id + 1,
                    Ticker = "EE",
                    ISIN = "FF",
                    Name = "GG",
                    Description = "HH",
                    Nominal = 1.0,
                    IdObjectType = 1,
                    IdObjectGroup = 1,
                    IdObjectBase = 1,
                    IdObjectFaceUnit = 1,
                    MatDateObject = "2025-06-06",
                    Instruments = new object[]
                    {
                        new { IdFi = 1, RCode = "R", IsLiquid = false, IdMarketBoard = 1 }
                    }
                }
            }
        });
        await using AssetSocketFake socket = new(payload);
        LoggerFake logger = new();
        WsAssetsInfo infos = new(socket, logger);
        Task<string> action = Task.Run(async () => (await infos.Info(new[] { id })).Json());
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await action);
    }
}
