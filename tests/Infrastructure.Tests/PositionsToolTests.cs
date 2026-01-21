using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Verifies output schema matching for positions tool. Usage example: executed by xUnit runner.
/// </summary>
public sealed class PositionsToolTests
{
    /// <summary>
    /// Ensures that positions tool output matches declared schema. Usage example: await tool.Result(data, token).
    /// </summary>
    [Fact(DisplayName = "Positions tool returns structured content matching output schema")]
    public async Task Positions_tool_returns_structured_content_matching_output_schema()
    {
        int count = RandomNumberGenerator.GetInt32(2, 5);
        int attempt = 0;
        bool match = false;
        while (attempt < 3 && !match)
        {
            attempt++;
            Task<bool>[] tasks = new Task<bool>[count];
            for (int index = 0; index < count; index++)
            {
                tasks[index] = Task.Run(async () =>
                {
                    long account = RandomNumberGenerator.GetInt32(10_000, 90_000);
                    string note = $"позиция-{Guid.NewGuid()}-д";
                    string payload = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new
                            {
                                IdPosition = RandomNumberGenerator.GetInt32(1, 100_000),
                                IdAccount = account,
                                IdSubAccount = account + 12,
                                IdRazdel = RandomNumberGenerator.GetInt32(1, 1000),
                                IdObject = RandomNumberGenerator.GetInt32(1, 100_000),
                                IdFiBalance = RandomNumberGenerator.GetInt32(1, 100_000),
                                IdBalanceGroup = RandomNumberGenerator.GetInt32(1, 10_000),
                                AssetsPercent = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                PSTNKD = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                IsMoney = true,
                                IsRur = false,
                                UchPrice = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                TorgPos = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                Price = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                DailyPL = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                DailyPLPercentToMarketCurPrice = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                BackPos = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                PrevQuote = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                TrnIn = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                TrnOut = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                DailyBuyVolume = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                DailySellVolume = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                DailyBuyQuantity = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                DailySellQuantity = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                NKD = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                PriceStep = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                Lot = RandomNumberGenerator.GetInt32(1, 1000),
                                NPLtoMarketCurPrice = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                NPLPercent = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                PlanLong = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                PlanShort = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                Note = note
                            }
                        }
                    });
                    await using BalanceSocketFake terminal = new(payload);
                    LoggerFake logger = new();
                    McpTool tool = new(new WsPositions(terminal, logger), new Tool { Name = "positions", Title = "Account positions", Description = "Returns positions for the given account id.", InputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accountId":{"type":"integer","description":"Account identifier"}},"required":["accountId"]}"""), OutputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"positions":{"type":"array","description":"Account positions for the requested account","items":{"type":"object","properties":{"IdPosition":{"type":"integer","description":"Position identifier"},"IdAccount":{"type":"integer","description":"Client account id"},"IdSubAccount":{"type":"integer","description":"Client subaccount id"},"IdRazdel":{"type":"integer","description":"Portfolio id"},"IdObject":{"type":"integer","description":"Security identifier"},"IdFiBalance":{"type":"integer","description":"Financial instrument used for valuation"},"IdBalanceGroup":{"type":"integer","description":"Portfolio group identifier"},"AssetsPercent":{"type":"number","description":"Position share in subaccount percent"},"PSTNKD":{"type":"number","description":"Accrued coupon income"},"IsMoney":{"type":"boolean","description":"Indicates money position"},"IsRur":{"type":"boolean","description":"Indicates ruble currency position"},"UchPrice":{"type":"number","description":"Accounting price"},"TorgPos":{"type":"number","description":"Current position size"},"Price":{"type":"number","description":"Current price"},"DailyPL":{"type":"number","description":"Daily profit or loss"},"DailyPLPercentToMarketCurPrice":{"type":"number","description":"Daily PnL percent to market price"},"BackPos":{"type":"number","description":"Opening position"},"PrevQuote":{"type":"number","description":"Previous session close price"},"TrnIn":{"type":"number","description":"External credit volume"},"TrnOut":{"type":"number","description":"External debit volume"},"DailyBuyVolume":{"type":"number","description":"Session buy volume"},"DailySellVolume":{"type":"number","description":"Session sell volume"},"DailyBuyQuantity":{"type":"number","description":"Session buy quantity"},"DailySellQuantity":{"type":"number","description":"Session sell quantity"},"NKD":{"type":"number","description":"Accrued coupon income amount"},"PriceStep":{"type":"number","description":"Price step"},"Lot":{"type":"integer","description":"Lot size"},"NPLtoMarketCurPrice":{"type":"number","description":"Nominal profit or loss"},"NPLPercent":{"type":"number","description":"Nominal profit or loss percent"},"PlanLong":{"type":"number","description":"Planned long position"},"PlanShort":{"type":"number","description":"Planned short position"}},"required":["IdPosition","IdAccount","IdSubAccount","IdRazdel","IdObject","IdFiBalance","IdBalanceGroup","AssetsPercent","PSTNKD","IsMoney","IsRur","UchPrice","TorgPos","Price","DailyPL","DailyPLPercentToMarketCurPrice","BackPos","PrevQuote","TrnIn","TrnOut","DailyBuyVolume","DailySellVolume","DailyBuyQuantity","DailySellQuantity","NKD","PriceStep","Lot","NPLtoMarketCurPrice","NPLPercent","PlanLong","PlanShort"],"additionalProperties":false}}},"required":["positions"],"additionalProperties":false}"""), Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } }, new MappedPayloadPlan(new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accountId":{"type":"integer","description":"Account identifier"}},"required":["accountId"]}"""))));
                    Dictionary<string, JsonElement> data = new() { ["accountId"] = JsonSerializer.SerializeToElement(account) };
                    using CancellationTokenSource source = new(TimeSpan.FromSeconds(2));
                    CallToolResult result = await tool.Result(data, source.Token);
                    JsonNode node = result.StructuredContent ?? throw new InvalidOperationException("Structured content is missing");
                    JsonElement schema = tool.Tool().OutputSchema ?? throw new InvalidOperationException("Output schema is missing");
                    SchemaMatch probe = new();
                    return probe.Match(node, schema);
                });
            }
            bool[] list = await Task.WhenAll(tasks);
            match = list.All(item => item);
        }
        Assert.True(match, "Positions tool output does not match schema");
    }
}
