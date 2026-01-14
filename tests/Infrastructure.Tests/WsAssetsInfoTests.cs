using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Security.Cryptography;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

/// <summary>
/// Verifies WsAssetsInfo behavior for asset info retrieval. Usage example: executed by xUnit runner.
/// </summary>
public sealed class WsAssetsInfoTests
{
    /// <summary>
    /// Ensures that WsAssetsInfo returns JSON containing requested asset info. Usage example: await infos.Entries(payload, token).
    /// </summary>
    [Fact(DisplayName = "WsAssetsInfo returns asset infos json for matching ids")]
    public async Task Given_asset_response_when_requested_then_returns_json()
    {
        long id = RandomNumberGenerator.GetInt32(50_000, 80_000);
        string text = JsonSerializer.Serialize(new
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
        await using AssetSocketFake socket = new(text);
        LoggerFake logger = new();
        WsAssetsInfo infos = new(socket, logger);
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idObjects":{"type":"array","description":"Collection of IdObject values to extract","items":{"type":"integer"}}},"required":["idObjects"]}"""));
        Dictionary<string, JsonElement> data = new(StringComparer.Ordinal) { ["idObjects"] = JsonSerializer.Deserialize<JsonElement>($"[{id}]") };
        MappedPayload payload = new(data, schema);
        string json = (await infos.Entries(payload)).StructuredContent().ToJsonString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement entry = document.RootElement.GetProperty("assets")[0];
        bool result = entry.GetProperty("IdObject").GetInt64() == id && entry.GetProperty("Instruments")[0].GetProperty("IdFi").GetInt64() == 7;
        Assert.True(result, "WsAssetsInfo does not return asset infos json for matching ids");
    }

    /// <summary>
    /// Confirms that WsAssetsInfo fails when requested assets are absent. Usage example: await infos.Entries(payload, token).
    /// </summary>
    [Fact(DisplayName = "WsAssetsInfo throws when asset infos are missing")]
    public async Task Given_response_without_target_assets_when_requested_then_throws()
    {
        long id = RandomNumberGenerator.GetInt32(81_000, 90_000);
        string text = JsonSerializer.Serialize(new
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
        await using AssetSocketFake socket = new(text);
        LoggerFake logger = new();
        WsAssetsInfo infos = new(socket, logger);
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idObjects":{"type":"array","description":"Collection of IdObject values to extract","items":{"type":"integer"}}},"required":["idObjects"]}"""));
        Dictionary<string, JsonElement> data = new(StringComparer.Ordinal) { ["idObjects"] = JsonSerializer.Deserialize<JsonElement>($"[{id}]") };
        MappedPayload payload = new(data, schema);
        Task<string> action = Task.Run(async () => (await infos.Entries(payload)).StructuredContent().ToJsonString());
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await action);
    }

    /// <summary>
    /// Ensures that WsAssetsInfo returns JSON containing requested assets by ticker. Usage example: await infos.Entries(payload, token).
    /// </summary>
    [Fact(DisplayName = "WsAssetsInfo returns asset infos json for matching tickers")]
    public async Task Given_asset_response_when_ticker_requested_then_returns_json()
    {
        string ticker = $"tk{RandomNumberGenerator.GetInt32(901, 999)}é";
        string text = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdObject = RandomNumberGenerator.GetInt32(90_100, 90_999),
                    Ticker = ticker,
                    ISIN = "I1",
                    Name = "N1",
                    Description = "D1",
                    Nominal = 2.5,
                    IdObjectType = 2,
                    IdObjectGroup = 3,
                    IdObjectBase = 4,
                    IdObjectFaceUnit = 5,
                    MatDateObject = "2026-07-07",
                    Instruments = new object[]
                    {
                        new { IdFi = 12, RCode = "R12", IsLiquid = true, IdMarketBoard = 8 }
                    }
                },
                new
                {
                    IdObject = RandomNumberGenerator.GetInt32(91_000, 91_900),
                    Ticker = "SKIP99",
                    ISIN = "I2",
                    Name = "N2",
                    Description = "D2",
                    Nominal = 1.1,
                    IdObjectType = 1,
                    IdObjectGroup = 1,
                    IdObjectBase = 1,
                    IdObjectFaceUnit = 1,
                    MatDateObject = "2026-08-08",
                    Instruments = new object[]
                    {
                        new { IdFi = 99, RCode = "R99", IsLiquid = false, IdMarketBoard = 1 }
                    }
                }
            }
        });
        await using AssetSocketFake socket = new(text);
        LoggerFake logger = new();
        WsAssetsInfo infos = new(socket, logger);
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"tickers":{"type":"array","description":"Collection of ticker symbols to extract","items":{"type":"string"}}},"required":["tickers"]}"""));
        string value = ticker;
        Dictionary<string, JsonElement> data = new(StringComparer.Ordinal) { ["tickers"] = JsonSerializer.Deserialize<JsonElement>($"[\"{value}\"]") };
        MappedPayload payload = new(data, schema);
        string json = (await infos.Entries(payload)).StructuredContent().ToJsonString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement entry = document.RootElement.GetProperty("assets")[0];
        bool result = entry.GetProperty("Ticker").GetString() == ticker && entry.GetProperty("Instruments")[0].GetProperty("IdFi").GetInt64() == 12;
        Assert.True(result, "WsAssetsInfo does not return asset infos json for matching tickers");
    }
}
